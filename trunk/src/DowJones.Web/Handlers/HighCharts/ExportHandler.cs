﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DowJones.Exceptions;
using DowJones.Web.Handlers.Proxy.Core;
using DowJones.Web.Properties;
using log4net;

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
            var output = (Task) result;
            if (output.Exception != null && output.Exception.InnerExceptions != null && output.Exception.InnerExceptions.Count > 0)
            {
                throw output.Exception.InnerExceptions[0];
            }
            ((Task)result).Dispose();
        }
    }

    public class ExportHandler : AbstractAsyncHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ExportHandler));
        protected override Task ProcessRequestAsync(HttpContext context)
        {
            Log.Debug("inside process");
            return Task.Factory.StartNew(() => Process(context));
        }
       
        private void Process(HttpContext context)
        {
            Log.Debug("inside process1");
            var exportParams = new ExportParams
            {
                Options = context.Request["options"] ?? string.Empty,
                Svg = context.Request["svg"] ?? string.Empty,
                FileType = !string.IsNullOrEmpty(context.Request["type"]) ? context.Request["type"] :
                            (!string.IsNullOrEmpty(context.Request["mimeType"]) ? context.Request["mimeType"] : string.Empty),
                Width = context.Request["width"] ?? string.Empty,
                Scale = context.Request["scale"] ?? string.Empty,
                Constructor = context.Request["constr"] ?? string.Empty,
                Callback = context.Request["callback"] ?? string.Empty,
                ContentType = context.Request["content"] ?? string.Empty,
                FileName = context.Request["filename"] ?? "chart"
            };
            Process process = null;
            try
            {
                if (string.IsNullOrEmpty(exportParams.Options) && string.IsNullOrEmpty(exportParams.Svg))
                    throw new HttpException(500, "Not a valid request. Either options or svg is required.");

                Guid fileName = Guid.NewGuid();
                exportParams.InputFile = "tmp\\" + fileName;
                exportParams.OutputFile = "tmp\\" + fileName;
              
                exportParams.HighChartConvertJsFile = "highcharts-convert.js";

                var credentials = new NetworkCredential(Settings.Default.UserName,Settings.Default.Password,Settings.Default.Domain);
                CreateInputAndCallbackFile(context, exportParams, credentials);
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
                process = new Process
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
                    var imageData = GetBytesFromFile(context.Request.MapPath(@"~\Scripts\PhantomJs\" + exportParams.OutputFile));
                    if (imageData != null)
                    {
                        context.Response.BinaryWrite(GetBytesFromFile(context.Request.MapPath(@"~\Scripts\PhantomJs\" + exportParams.OutputFile)));
                        context.Response.ContentType = exportParams.FileType;
                        context.Response.Headers.Add("Content-Disposition", "attachment; filename=" + exportParams.FileName + "." + extension);
                    }
                    else
                    {
                        throw new HttpException(((int)HttpStatusCode.InternalServerError), "Unable to export chart. Chart data is not valid.");
                    }
                }
                else
                {
                    throw new HttpException(((int)HttpStatusCode.InternalServerError), "Unable to export chart. Chart data is not valid.");
                }
            }
            catch (DowJonesUtilitiesException dex)
            {
                Logger.WriteEntry("Error while exporting chart :"+dex.InnerException);
                throw new HttpException(((int)HttpStatusCode.InternalServerError), string.Concat("Unable to export chart. [", dex.ReturnCode, "]"));
            }
            catch (Exception exception)
            {
                Logger.WriteEntry("Error while exporting chart :" + exception);
                throw new HttpException(((int)HttpStatusCode.InternalServerError), "General chart export error",exception);
            }
            finally
            {
                if (process != null)
                {
                    process.Close();
                }
                context.ApplicationInstance.CompleteRequest();
                if (File.Exists(exportParams.InputFileAbsolutePath))
                    File.Delete(exportParams.InputFileAbsolutePath);
                if (File.Exists(exportParams.OutputFileAbsolutePath))
                    File.Delete(exportParams.OutputFileAbsolutePath);
                if (File.Exists(exportParams.CallbackFileAbsolutePath))
                    File.Delete(exportParams.CallbackFileAbsolutePath);
            }
        }

        public static void AddUsersAndPermissions(string directoryName, string userAccount, FileSystemRights userRights, AccessControlType accessType)
        {
            try
            {
                // Create a DirectoryInfo object.
                var directoryInfo = new DirectoryInfo(directoryName);

                // Get security settings.
                DirectorySecurity dirSecurity = directoryInfo.GetAccessControl();
                var identity = "NETWORK SERVICE";
                var accessRule = new FileSystemAccessRule(identity,
                              FileSystemRights.Modify,
                              InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                              PropagationFlags.InheritOnly,
                              AccessControlType.Allow);

                // Add the FileSystemAccessRule to the security settings. 
                dirSecurity.AddAccessRule(accessRule);
                // Set the access settings.
                directoryInfo.SetAccessControl(dirSecurity);
            }
            catch (Exception ex)
            {
                Logger.WriteEntry("Error while giving permission to directory :" + ex.InnerException);    
            }
        }

        private static void CreateInputAndCallbackFile(HttpContext context, ExportParams exportParams,NetworkCredential credential)
        {
            try
            {
                if (string.IsNullOrEmpty(exportParams.ContentType))
                {
                    if (!string.IsNullOrEmpty(exportParams.Options))
                        exportParams.ContentType = "options";
                    else if (!string.IsNullOrEmpty(exportParams.Svg))
                        exportParams.ContentType = "svg";
                }

                if (exportParams.ContentType.Equals("options", StringComparison.CurrentCultureIgnoreCase))
                {
                    exportParams.InputFile += ".json";
                    exportParams.InputFileAbsolutePath = context.Request.MapPath(@"~\Scripts\PhantomJs\" + exportParams.InputFile);
                    StreamWriter writer = File.CreateText(exportParams.InputFileAbsolutePath);
                    AddUsersAndPermissions(exportParams.InputFileAbsolutePath, credential.Domain + "\\" + credential.UserName, FileSystemRights.Read | FileSystemRights.ReadAndExecute | FileSystemRights.Write | FileSystemRights.FullControl, AccessControlType.Allow);
                    writer.Write(exportParams.Options);
                    writer.Close();
                }
                else if (exportParams.ContentType.Equals("svg", StringComparison.CurrentCultureIgnoreCase))
                {
                    exportParams.InputFile += ".svg";
                    exportParams.InputFileAbsolutePath = context.Request.MapPath(@"~\Scripts\PhantomJs\" + exportParams.InputFile);
                    StreamWriter writer = File.CreateText(exportParams.InputFileAbsolutePath);
                    AddUsersAndPermissions(exportParams.InputFileAbsolutePath, credential.Domain + "\\" + credential.UserName, FileSystemRights.Read | FileSystemRights.ReadAndExecute | FileSystemRights.Write | FileSystemRights.FullControl, AccessControlType.Allow);
                    writer.Write(exportParams.Svg);
                    writer.Close();
                }

                if (!string.IsNullOrEmpty(exportParams.Callback.Trim('\r').Trim('\t').Trim('\n')))
                {
                    exportParams.CallbackFile = exportParams.InputFile + ".js";
                    exportParams.CallbackFileAbsolutePath = context.Request.MapPath(@"~\Scripts\PhantomJs\" + exportParams.CallbackFile);
                    StreamWriter writer = File.CreateText(exportParams.CallbackFileAbsolutePath);
                    AddUsersAndPermissions(exportParams.InputFileAbsolutePath, credential.Domain + "\\" + credential.UserName, FileSystemRights.Read | FileSystemRights.ReadAndExecute | FileSystemRights.Write | FileSystemRights.FullControl, AccessControlType.Allow);
                    writer.Write(exportParams.Callback);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteEntry("Error while creating input and callback file :" + ex.InnerException);
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
            if (File.Exists(fullFilePath))
            {
                FileStream fs = File.OpenRead(fullFilePath);
                try
                {
                    var bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                    return bytes;
                }
                finally
                {
                    fs.Close();
                }
            }
            return null;
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
