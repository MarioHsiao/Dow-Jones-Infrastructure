using DowJones.Properties;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Web;
using Microsoft.Win32;

namespace DowJones.Web.Handlers.Video
{
    public class ImageHandler : BaseHttpHandler
    {
        private const string ImageUrlParameterName = "url";
        private const string ImageWidthParameterName = "width";
        private const string ImageHeightParameterName = "height";
        private const string ImageAspectRationParameterName = "ar";

        private string _imageUrl;
        private int? _width;
        private int? _height;
        private AspectRatio _ratio;

        public override void HandleRequest(HttpContext context)
        {
            try
            {
                var image = GetImage(_imageUrl);
                var size = CalculateSize(_width, _height, image.Width, image.Height);

                context.Response.ContentType = GetContentType(GetFileExtension(_imageUrl));
                context.Response.Clear();

                using (var bitmap = new Bitmap(image, size))
                {
                    using (var stream = new MemoryStream())
                    {
                        bitmap.Save(stream, ImageFormat.Jpeg);
                        context.Response.BinaryWrite(stream.ToArray());
                    }
                }
            }
            catch (Exception)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            finally
            {
                context.Response.End();
            }

            // Set cache at the end.
            var cache = context.Response.Cache;
            cache.SetCacheability(HttpCacheability.Public);
            cache.VaryByParams[ImageUrlParameterName] = true;
            cache.VaryByParams[ImageWidthParameterName] = true;
            cache.VaryByParams[ImageHeightParameterName] = true;
            cache.VaryByParams[ImageAspectRationParameterName] = true;
            cache.VaryByContentEncodings["gzip"] = true;
            cache.VaryByContentEncodings["deflate"] = true;
            cache.SetOmitVaryStar(true);
            cache.SetExpires(DateTime.Now.AddDays(365));
            cache.SetValidUntilExpires(true);
            cache.SetLastModifiedFromFileDependencies();
            
        }

        public override bool ValidateParameters(HttpContext context)
        {
            _imageUrl = GetImageUrl(context);
            _width = GetWidth(context);
            _height = GetHeight(context);
            _ratio = GetAspectRatio(context);

            if (string.IsNullOrEmpty(_imageUrl))
            {
                return false;
            }

            if (!_width.HasValue && !_height.HasValue)
            {
                return false;
            }

            return (!_width.HasValue || !_height.HasValue) || _ratio != AspectRatio.locked;
        }

        public override bool RequiresAuthentication
        {
            get { return false; }
        }

        public override string ContentMimeType
        {
            get { return GetContentType(GetFileExtension(_imageUrl)); }
        }

        private static Image GetImage(string imageUrl)
        {
            var request = WebRequest.Create(imageUrl);

            if (!String.IsNullOrEmpty(Settings.Default.WebResourcesProxy))
            {
                request.Proxy = new WebProxy(Settings.Default.WebResourcesProxy);
            }

            using (var response = request.GetResponse())
            {
                return  Image.FromStream(response.GetResponseStream());
            }
        }

        private static string GetFileExtension(string imageUrl)
        {
            var uri = new Uri(imageUrl);
            var absolutePath = uri.AbsolutePath;
            var extension = Path.GetExtension(absolutePath);
            return extension;
        }

        private static string GetContentType(string extension)
        {
            var rootKey = Registry.ClassesRoot;
            rootKey = rootKey.OpenSubKey(extension);

            return rootKey == null ? null : rootKey.GetValue("Content Type").ToString();
        }

        private static Size CalculateSize(int? width, int? height, int originalWidth, int originalHeight)
        {
            
            var calculatedWidth = 0;
            var calculatedHeight = 0;

            if (height.HasValue && width.HasValue)
            {
                calculatedHeight = height.Value;
                calculatedWidth = width.Value;
            }
            else if (height.HasValue)
            {
                calculatedHeight = height.Value;
                var multiplier = (height.Value / (float)originalHeight);
                calculatedWidth = CalculateDimension(originalWidth, multiplier);
            }
            else if (width.HasValue)
            {
                calculatedWidth = width.Value;
                var multiplier = (width.Value / (float)originalWidth);
                calculatedHeight = (int)(originalHeight * multiplier);
            }

            return new Size(calculatedWidth, calculatedHeight);
        }

        private static int CalculateDimension(int originalDimension, float multiplier)
        {
            return (int)(originalDimension * multiplier);
        }

        private static string GetImageUrl(HttpContext context)
        {
            return context.Request.QueryString[ImageUrlParameterName];
        }

        private static int? GetWidth(HttpContext context)
        {
            int? width = null;

            int temp;
            if (int.TryParse(context.Request.QueryString[ImageWidthParameterName], out temp))
            {
                width = temp;
            }

            return width;
        }

        private static int? GetHeight(HttpContext context)
        {
            int? height = null;

            int temp;
            if (int.TryParse(context.Request.QueryString[ImageHeightParameterName], out temp))
            {
                height = temp;
            }

            return height;
        }

        private static AspectRatio GetAspectRatio(HttpContext context)
        {
            var ratio = AspectRatio.unlocked;
            try
            {
                ratio = (AspectRatio)Enum.Parse(typeof(AspectRatio), context.Request.QueryString[ImageAspectRationParameterName]);
            }
            catch (Exception) {}

            return ratio;

        }
    }

    enum AspectRatio
    {
        locked,
        unlocked
    }
}
