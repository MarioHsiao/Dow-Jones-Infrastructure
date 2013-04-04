using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DowJones.Web.Properties;

namespace DowJones.Web.Handlers.HighCharts
{
    public abstract class AbstractAsyncHandler : IHttpAsyncHandler
    {
        protected abstract Task ProcessRequestAsync(HttpContext context);

        private Task ProcessRequestAsync(HttpContext context, AsyncCallback cb)
        {
            return ProcessRequestAsync(context)
                .ContinueWith(task => cb(task));
        }

        public void ProcessRequest(HttpContext context)
        {
            ProcessRequestAsync(context).Wait();
        }

        public bool IsReusable
        {
            get { return true; }
        }

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            return ProcessRequestAsync(context, cb);
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            ((Task)result).Dispose();
        }
    }

    public class ExportHandler : AbstractAsyncHandler
    {
        
        protected override Task ProcessRequestAsync(HttpContext context)
        {
            return Task.Factory.StartNew(() => Process(context));
        }
       
        private void Process(HttpContext context)
        {
            var exportParams = new ExportParams
            {
                Options = context.Request["options"] ?? string.Empty,
                Svg = context.Request["svg"] ?? string.Empty,
                FileType = context.Request["type"] ?? string.Empty,
                Width = context.Request["width"] ?? string.Empty,
                Scale = context.Request["scale"] ?? string.Empty,
                Constructor = context.Request["constr"] ?? string.Empty,
                Callback = context.Request["callback"] ?? string.Empty,
                ContentType = context.Request["content"] ?? string.Empty,
                FileName = context.Request["filename"] ?? "chart"
            };
            try
            {
                if (string.IsNullOrEmpty(exportParams.Options) && string.IsNullOrEmpty(exportParams.Svg))
                    throw new HttpException(500, "Not a valid request. Either options or svg is required.");

                Guid fileName = Guid.NewGuid();
                exportParams.InputFile = "tmp\\" + fileName;
                exportParams.OutputFile = "tmp\\" + fileName;
              
                exportParams.HighChartConvertJsFile = "highcharts-convert.js";
               
                CreateInputAndCallbackFile(context,exportParams);
                string extension = GetFileExtension(exportParams.FileType);
                string command = GetPhantomJsCommand(exportParams, extension);
                exportParams.OutputFileAbsolutePath = context.Request.MapPath(@"~\Scripts\PhantomJs\" + exportParams.OutputFile);

                var startInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        WorkingDirectory = context.Request.MapPath(@"~\Scripts\PhantomJs")
                    };
                var process = new Process
                    {
                        StartInfo = startInfo,
                        EnableRaisingEvents = true
                    };

                process.Start();
                process.StandardInput.WriteLine(command);
                process.StandardInput.WriteLine("exit");
                process.WaitForExit(10000);
                if (process.HasExited)
                {
                    context.Response.BinaryWrite(GetBytesFromFile(context.Request.MapPath(@"~\Scripts\PhantomJs\" + exportParams.OutputFile)));
                    context.Response.ContentType =  exportParams.FileType;
                    context.Response.Headers.Add("Content-Disposition", "attachment; filename=" + exportParams.FileName + "." + extension);
                }
                process.Close();
            }
            catch (Exception exception)
            {
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            }
            finally
            {
                context.ApplicationInstance.CompleteRequest();
                if (File.Exists(exportParams.InputFileAbsolutePath))
                    File.Delete(exportParams.InputFileAbsolutePath);
                if (File.Exists(exportParams.OutputFileAbsolutePath))
                    File.Delete(exportParams.OutputFileAbsolutePath);
                if (File.Exists(exportParams.CallbackFileAbsolutePath))
                    File.Delete(exportParams.CallbackFileAbsolutePath);
            }
        }

        private static void CreateInputAndCallbackFile(HttpContext context, ExportParams exportParams)
        {
            if (exportParams.ContentType.Equals("options", StringComparison.CurrentCultureIgnoreCase))
            {
                exportParams.InputFile += ".json";
                exportParams.InputFileAbsolutePath = context.Request.MapPath(@"~\Scripts\PhantomJs\" + exportParams.InputFile);
                StreamWriter writer = File.CreateText(exportParams.InputFileAbsolutePath);
                writer.Write(exportParams.Options);
                writer.Close();
            }
            else if (exportParams.ContentType.Equals("svg", StringComparison.CurrentCultureIgnoreCase))
            {
                exportParams.InputFile += ".svg";
                exportParams.InputFileAbsolutePath = context.Request.MapPath(@"~\Scripts\PhantomJs\" + exportParams.InputFile);
                StreamWriter writer = File.CreateText(exportParams.InputFileAbsolutePath);
                writer.Write(exportParams.Svg);
                writer.Close();
            }
         
            if (!string.IsNullOrEmpty(exportParams.Callback.Trim('\r').Trim('\t').Trim('\n')))
            {
                exportParams.CallbackFile = exportParams.InputFile+".js";
                exportParams.CallbackFileAbsolutePath = context.Request.MapPath(@"~\Scripts\PhantomJs\" + exportParams.CallbackFile);
                StreamWriter writer = File.CreateText(exportParams.CallbackFileAbsolutePath);
                writer.Write(exportParams.Callback);
                writer.Close();
            }
        }

        private static string GetFileExtension(string type)
        {
            if (type.Equals("image/jpeg", StringComparison.CurrentCultureIgnoreCase))
            {
                return "jpg";
            }
            if (type.Equals("application/pdf", StringComparison.CurrentCultureIgnoreCase))
            {
                return "pdf";
            }
            if (type.Equals("image/svg+xml", StringComparison.CurrentCultureIgnoreCase))
            {
                return "svg";
            }
            if (type.Equals("image/png", StringComparison.CurrentCultureIgnoreCase))
            {
                return "png";
            }
            return "png";
        }

        private static string GetPhantomJsCommand(ExportParams exportParams,string extension)
        {
            var command = new StringBuilder();
            command.Append(Settings.Default.PhantomJsExePath);
            command.Append(" '");
            command.Append(exportParams.HighChartConvertJsFile);
            command.Append("' -infile '");
            command.Append(exportParams.InputFile);
            if (!string.IsNullOrEmpty(exportParams.CallbackFile))
            {
                command.Append("' -callback '");
                command.Append(exportParams.CallbackFile);
            }
            command.Append("' -outfile '");
            exportParams.OutputFile = exportParams.OutputFile + "." + extension;
            command.Append(exportParams.OutputFile);
            command.Append("'");
            if (!string.IsNullOrEmpty(exportParams.Constructor))
            {
                command.Append(" -constr ");
                command.Append(exportParams.Constructor);
            }
            if (!string.IsNullOrEmpty(exportParams.Width))
            {
                command.Append(" -width ");
                command.Append(exportParams.Width);
            }
            if (!string.IsNullOrEmpty(exportParams.Scale))
            {
                command.Append(" -scale ");
                command.Append(exportParams.Scale);
            }
            return command.ToString();
        }

        private static byte[] GetBytesFromFile(string fullFilePath)
        {
            // this method is limited to 2^32 byte files (4.2 GB)

            FileStream fs = File.OpenRead(fullFilePath);
            try
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                return bytes;
            }
            finally
            {
                fs.Close();
            }

        }

        private class ExportParams
        {
            public string Options { get; set; }
            public string Svg { get; set; }
            public string Callback { get; set; }
            public string Constructor { get; set; }
            public string InputFile { get; set; }
            public string InputFileAbsolutePath { get; set; }
            public string HighChartConvertJsFile { get; set; }
            public string OutputFile { get; set; }
            public string OutputFileAbsolutePath { get; set; }
            public string CallbackFile { get; set; }
            public string CallbackFileAbsolutePath { get; set; }
            public string FileType { get; set; }
            public string Width { get; set; }
            public string Scale { get; set; }
            public string ContentType { get; set; }
            public string FileName { get; set; }
        }
    }
}
