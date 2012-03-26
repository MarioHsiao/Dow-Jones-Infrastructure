using DowJones.Properties;

namespace DowJones.Utilities.Handlers.Video
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Net;
    using System.Web;
    using Microsoft.Win32;

    public class ImageHandler : BaseHttpHandler
    {
        private const string ImageUrlParameterName = "url";
        private const string ImageWidthParameterName = "width";
        private const string ImageHeightParameterName = "height";
        private const string ImageAspectRationParameterName = "ar";

        private string m_ImageUrl;
        private int? m_Width;
        private int? m_Height;
        private AspectRatio m_Ratio;

        public override void HandleRequest(HttpContext context)
        {
            try
            {
                Image image = GetImage(m_ImageUrl);
                Size size = CalculateSize(m_Width, m_Height, image.Width, image.Height);

                context.Response.ContentType = GetContentType(GetFileExtension(m_ImageUrl));
                context.Response.Clear();

                using (Bitmap bitmap = new Bitmap(image, size))
                {
                    using (MemoryStream stream = new MemoryStream())
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
            m_ImageUrl = GetImageUrl(context);
            m_Width = GetWidth(context);
            m_Height = GetHeight(context);
            m_Ratio = GetAspectRatio(context);

            if (string.IsNullOrEmpty(m_ImageUrl))
            {
                return false;
            }

            if (!m_Width.HasValue && !m_Height.HasValue)
            {
                return false;
            }

            if ((m_Width.HasValue && m_Height.HasValue) && m_Ratio == AspectRatio.locked)
            {
                return false;
            }

            if ((!m_Width.HasValue && !m_Height.HasValue) && m_Ratio == AspectRatio.unlocked)
            {
                return false;
            }

            return true;
        }

        public override bool RequiresAuthentication
        {
            get { return false; }
        }

        public override string ContentMimeType
        {
            get { return GetContentType(GetFileExtension(m_ImageUrl)); }
        }

        private static Image GetImage(string imageUrl)
        {
            WebRequest request = WebRequest.Create(imageUrl);

            if (!String.IsNullOrEmpty(Settings.Default.WebResourcesProxy))
                request.Proxy = new WebProxy(Settings.Default.WebResourcesProxy);

            Image image;
            using (WebResponse response = request.GetResponse())
            {
                image = Image.FromStream(response.GetResponseStream());
            }

            return image;
        }

        private static string GetFileExtension(string imageUrl)
        {
            Uri uri = new Uri(imageUrl);

            string absolutePath = uri.AbsolutePath;

            string extension = Path.GetExtension(absolutePath);

            return extension;
        }

        private static string GetContentType(string extension)
        {
            RegistryKey rootKey = Registry.ClassesRoot;

            rootKey = rootKey.OpenSubKey(extension);

            if (rootKey == null) return null;

            return rootKey.GetValue("Content Type").ToString();
        }

        private static Size CalculateSize(int? width, int? height, int originalWidth, int originalHeight)
        {
            Debug.Assert(height.HasValue || width.HasValue);

            int calculatedWidth;
            int calculatedHeight;

            if (height.HasValue && width.HasValue)
            {
                calculatedHeight = height.Value;
                calculatedWidth = width.Value;
            }
            else if (height.HasValue)
            {
                calculatedHeight = height.Value;

                float multiplier = (height.Value / (float)originalHeight);

                calculatedWidth = CalculateDimension(originalWidth, multiplier);
            }
            else
            {
                calculatedWidth = width.Value;

                float multiplier = (width.Value / (float)originalWidth);

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
            AspectRatio ratio = AspectRatio.unlocked;
            try
            {
                ratio = (AspectRatio)Enum.Parse(typeof(AspectRatio), context.Request.QueryString[ImageAspectRationParameterName]);
            }
            catch
            {

            }

            return ratio;

        }
    }

    enum AspectRatio
    {
        locked,
        unlocked
    }
}
