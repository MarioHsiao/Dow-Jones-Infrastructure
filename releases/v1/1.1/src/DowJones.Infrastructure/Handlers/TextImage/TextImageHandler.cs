using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace DowJones.Utilities.Handlers.TextImage
{
    public class TextImageHandler : BaseHttpHandler
    {
        private const string CONTENT_MIME_TYPE = "text/html";
        private const bool REQUIRES_AUTHENTICATION = false;

        private int direction;
        private int fontsize;
        string text;
        string fontname;
        private FontStyle fontstyle;
        private Brush textBrush;
        private Font font;
        private Color backColor;
        Color foreColor;
        const int penWidth = 6;

        public override void HandleRequest(HttpContext context)
        {
            //context.Response.Expires = 1440;

            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetExpires(DateTime.Now.AddMinutes(1440));
            context.Response.Cache.VaryByParams["text;direction"] = true;
            context.Response.Cache.SetValidUntilExpires(true);
            context.Response.Cache.SetLastModifiedFromFileDependencies();

            context.Response.ContentType = "image/png";
            
            text = !String.IsNullOrEmpty(context.Request["text"]) ? context.Request["text"] : " ";

            var stream = new MemoryStream();
            setDirection(context.Request["direction"]);
            setFont(context.Request["fontname"], context.Request["fontsize"], context.Request["fontstyle"]);
            setFontColor(context.Request["fcolor"]);
            setBGColor(context.Request["bcolor"]);

            var oCanvas = new Bitmap(1, 1);

            using (var g = Graphics.FromImage(oCanvas))
            {
                var iWidth = (int) g.MeasureString(text, font).Width;
                var iHeight = (int) g.MeasureString(text, font).Height;

                textBrush = new SolidBrush(foreColor);

                if (direction == 1)
                {
                    drawVerticalTextImage(ref oCanvas, iWidth, iHeight);
                }
                else
                {
                    drawHorizontalTextImage(ref oCanvas, iWidth, iHeight);
                }

                oCanvas.Save(stream, ImageFormat.Png);
                stream.WriteTo(context.Response.OutputStream);

                context.Response.End();
            }

            oCanvas.Dispose();
            context.ApplicationInstance.CompleteRequest();
            
        }

        private void drawHorizontalTextImage(ref Bitmap oCanvas, int width, int height)
        {
            if (oCanvas == null)
                throw new ArgumentNullException("oCanvas");
            oCanvas = new Bitmap(width, height);

            var matrix = new Matrix();
            matrix.Translate(-5, -3);

            oCanvas.MakeTransparent();
            using (var g = Graphics.FromImage(oCanvas))
            {
                g.Transform = matrix;
                g.Clear(backColor);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                g.DrawString(text, font, textBrush, penWidth, 3);
            }
        }
        private void drawVerticalTextImage(ref Bitmap oCanvas, int width, int height)
        {
            if (oCanvas == null)
                throw new ArgumentNullException("oCanvas");
            oCanvas = new Bitmap(height,width);
            var matrix = new Matrix();
            matrix.Translate(-5, width + 5);
            matrix.Rotate(270);

            oCanvas.MakeTransparent();
            using (var g = Graphics.FromImage(oCanvas))
            {
                g.Clear(backColor);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                g.Transform = matrix;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawString(text, font, textBrush, penWidth, 3);
            }
        }


        public override bool ValidateParameters(HttpContext context)
        {
            return true;
        }

        public override bool RequiresAuthentication
        {
            get { return REQUIRES_AUTHENTICATION; }
        }

        /// <summary>
        /// Gets the content MIME type.
        /// </summary>
        /// <value></value>
        public override string ContentMimeType
        {
            get { return CONTENT_MIME_TYPE; }
        }

        private void setFontColor(string col)
        {
            foreColor = col == null ? HexStringToColor("232624") : HexStringToColor(col);
        }

        private void setBGColor(string col)
        {
            backColor = col == null ? Color.Transparent : HexStringToColor(col);
        }

        private void setFont(string fnt, string size, string style)
        {
            fontname = fnt ?? "Arial";
            fontsize = size != null ? Convert.ToInt32(size) : 9;

            if(style!= null)
                switch(style.ToUpper())
                {
                    case "REGULAR":
                        fontstyle = FontStyle.Regular;
                        break;
                    case "BOLD":
                        fontstyle = FontStyle.Bold;
                        break;
                    case "ITALIC":
                        fontstyle = FontStyle.Italic;
                        break;
                    case "UNDERLINE":
                        fontstyle = FontStyle.Underline;
                        break;
                    case "STRIKEOUT":
                        fontstyle = FontStyle.Strikeout;
                        break;
                    default:
                        fontstyle = FontStyle.Regular;
                        break;

                }
            else
                fontstyle =  FontStyle.Regular;


            font = new Font(fontname, fontsize, fontstyle);
        }
        private void setDirection(string ang)
        {
            if (ang != null && ang == "1")
            {
                direction = 1;
            }
            else direction = 0;
        }

      

        public static Color HexStringToColor(string hexColor)
        {
            var hc = ExtractHexDigits(hexColor);
            if (hc.Length != 6)
            {
                return Color.Black;
            }
            var r = hc.Substring(0, 2);
            var g = hc.Substring(2, 2);
            var b = hc.Substring(4, 2);
            Color color;
            try
            {
                var ri = Int32.Parse(r, NumberStyles.HexNumber);
                var gi = Int32.Parse(g, NumberStyles.HexNumber);
                var bi = Int32.Parse(b, NumberStyles.HexNumber);
                color = Color.FromArgb(ri, gi, bi);
            }
            catch
            {
                return Color.Black;
            }
            return color;
        }

        public static string ExtractHexDigits(string input)
        {
            var isHexDigit = new Regex("[abcdefABCDEF\\d]+", RegexOptions.Compiled);
            var newnum = "";
            if (input != null)
            {
                foreach (char c in input)
                {
                    if (isHexDigit.IsMatch(c.ToString()))
                        newnum += c.ToString();
                }
            }
            return newnum;
        }

    }
}
