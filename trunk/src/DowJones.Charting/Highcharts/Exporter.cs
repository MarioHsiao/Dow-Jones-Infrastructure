// Exporter.cs
// Tek4.Highcharts.Exporting.Exporter class.
// Tek4.Highcharts.Exporting assembly.
// ==========================================================================
// <summary>
// .NET chart exporting class for Highcharts JS JavaScript charts.
// </summary>
// ==========================================================================
// Author: Kevin P. Rice, Tek4(TM) (http://Tek4.com/)
//
// Based upon ASP.NET Highcharts export module by Clément Agarini
//
// Copyright (C) 2012 by Tek4(TM) - Kevin P. Rice
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// REVISION HISTORY:
// 2011-07-16 KPR Created.
// 2012-03-03 KPR Bug fix: WriteToStream() PNG requires seekable stream.

using System;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Svg;
using Svg.Transforms;

namespace DowJones.Charting.Highcharts
{
    /// <summary>
    /// .NET chart exporting class for Highcharts JS JavaScript charts.
    /// </summary>
    internal class Exporter
    {
        /// <summary>
        /// Default file name to use for chart exports if not otherwise specified.
        /// </summary>
        private const string DefaultFileName = "Chart";

        /// <summary>
        /// PDF metadata Creator string.
        /// </summary>
        private const string PdfMetaCreator = "DowJones Exporting Module for Highcharts JS";

        /// <summary>
        /// Initializes a new chart Export object using the specified file name, 
        /// output type, chart width and SVG text data.
        /// </summary>
        /// <param name="fileName">The file name (without extension) to be used 
        /// for the exported chart.</param>
        /// <param name="type">The requested MIME type to be generated. Can be
        /// 'image/jpeg', 'image/png', 'application/pdf' or 'image/svg+xml'.</param>
        /// <param name="width">The pixel width of the exported chart image.</param>
        /// <param name="svg">An SVG chart document to export (XML text).</param>
        internal Exporter(
            string fileName,
            string type,
            int width,
            string svg)
        {
            string extension;

            ContentType = type.ToLower();
            Name = fileName;
            Svg = svg;
            Width = width;

            // Validate requested MIME type.
            switch (ContentType)
            {
                case "image/jpeg":
                    extension = "jpg";
                    break;

                case "image/png":
                    extension = "png";
                    break;

                case "application/pdf":
                    extension = "pdf";
                    break;

                case "image/svg+xml":
                    extension = "svg";
                    break;

                    // Unknown type specified. Throw exception.
                default:
                    throw new ArgumentException(
                        string.Format("Invalid type specified: '{0}'.", type));
            }

            // Determine output file name.
            FileName = string.Format(
                "{0}.{1}",
                string.IsNullOrEmpty(fileName) ? DefaultFileName : fileName,
                extension);

            // Create HTTP Content-Disposition header.
            ContentDisposition =
                string.Format("attachment; filename={0}", FileName);
        }

        /// <summary>
        /// Gets the HTTP Content-Disposition header to be sent with an HTTP
        /// response that will cause the client's browser to open a file save
        /// dialog with the proper file name.
        /// </summary>
        public string ContentDisposition { get; private set; }

        /// <summary>
        /// Gets the MIME type of the exported output.
        /// </summary>
        public string ContentType { get; private set; }

        /// <summary>
        /// Gets the file name with extension to use for the exported chart.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets the chart name (same as file name without extension).
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the SVG chart document (XML text).
        /// </summary>
        public string Svg { get; private set; }

        /// <summary>
        /// Gets the pixel width of the exported chart image.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Creates an SvgDocument from the SVG text string.
        /// </summary>
        /// <returns>An SvgDocument object.</returns>
        private SvgDocument CreateSvgDocument()
        {
            SvgDocument svgDoc;

            // Create a MemoryStream from SVG string.
            using (var streamSvg = new MemoryStream(
                Encoding.UTF8.GetBytes(Svg)))
            {
                // Create and return SvgDocument from stream.
                svgDoc = SvgDocument.Open(streamSvg);
            }

            // Scale SVG document to requested width.
            svgDoc.Transforms = new SvgTransformCollection();
            float scalar = Width/svgDoc.Width;
            svgDoc.Transforms.Add(new SvgScale(scalar, scalar));
            svgDoc.Width = new SvgUnit(svgDoc.Width.Type, svgDoc.Width*scalar);
            svgDoc.Height = new SvgUnit(svgDoc.Height.Type, svgDoc.Height*scalar);

            return svgDoc;
        }

        /// <summary>
        /// Exports the chart to the specified HttpResponse object. This method
        /// is preferred over WriteToStream() because it handles clearing the
        /// output stream and setting the HTTP reponse headers.
        /// </summary>
        /// <param name="httpResponse"></param>
        internal void WriteToHttpResponse(HttpResponse httpResponse)
        {
            httpResponse.ClearContent();
            httpResponse.ClearHeaders();
            httpResponse.ContentType = ContentType;
            httpResponse.AddHeader("Content-Disposition", ContentDisposition);
            WriteToStream(httpResponse.OutputStream);
        }

        /// <summary>
        /// Exports the chart to the specified output stream as binary. When 
        /// exporting to a web response the WriteToHttpResponse() method is likely
        /// preferred.
        /// </summary>
        /// <param name="outputStream">An output stream.</param>
        internal void WriteToStream(Stream outputStream)
        {
            switch (ContentType)
            {
                case "image/jpeg":
                    CreateSvgDocument().Draw().Save(
                        outputStream,
                        ImageFormat.Jpeg);
                    break;

                case "image/png":
                    // PNG output requires a seekable stream.
                    using (var seekableStream = new MemoryStream())
                    {
                        CreateSvgDocument().Draw().Save(
                            seekableStream,
                            ImageFormat.Png);
                        seekableStream.WriteTo(outputStream);
                    }
                    break;

                case "application/pdf":
                    var svgDoc = CreateSvgDocument();

                    // Create PDF document.
                    using (var pdfDoc = new Document())
                    {
                        // Scalar to convert from 72 dpi to 150 dpi.
                        const float DpiScalar = 150f/72f;

                        // Set page size. Page dimensions are in 1/72nds of an inch.
                        // Page dimensions are scaled to boost dpi and keep page
                        // dimensions to a smaller size.
                        pdfDoc.SetPageSize(new Rectangle(
                                               svgDoc.Width/DpiScalar,
                                               svgDoc.Height/DpiScalar));

                        // Set margin to none.
                        pdfDoc.SetMargins(0f, 0f, 0f, 0f);

                        // Create PDF writer to write to response stream.
                        using (PdfWriter pdfWriter = PdfWriter.GetInstance(
                            pdfDoc,
                            outputStream))
                        {
                            // Configure PdfWriter.
                            pdfWriter.SetPdfVersion(PdfWriter.PDF_VERSION_1_5);
                            pdfWriter.CompressionLevel = PdfStream.DEFAULT_COMPRESSION;

                            // Add meta data.
                            pdfDoc.AddCreator(PdfMetaCreator);
                            pdfDoc.AddTitle(Name);

                            // Output PDF document.
                            pdfDoc.Open();
                            pdfDoc.NewPage();

                            // Create image element from SVG image.
                            Image image = Image.GetInstance(svgDoc.Draw(), ImageFormat.Bmp);
                            image.CompressionLevel = PdfStream.DEFAULT_COMPRESSION;

                            // Must match scaling performed when setting page size.
                            image.ScalePercent(100f/DpiScalar);

                            // Add image element to PDF document.
                            pdfDoc.Add(image);

                            pdfDoc.CloseDocument();
                        }
                    }

                    break;

                case "image/svg+xml":
                    using (var writer = new StreamWriter(outputStream))
                    {
                        writer.Write(Svg);
                        writer.Flush();
                    }

                    break;

                default:
                    throw new InvalidOperationException(string.Format(
                        "ContentType '{0}' is invalid.", ContentType));
            }

            outputStream.Flush();
        }
    }
}