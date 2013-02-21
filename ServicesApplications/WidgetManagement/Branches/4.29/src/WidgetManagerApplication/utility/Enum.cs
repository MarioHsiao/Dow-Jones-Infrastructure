// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Enum.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using MimeTypeAttribute = EMG.widgets.ui.attributes.MimeType;

namespace EMG.widgets.ui.utility
{
    /// <summary>
    /// MimeType enumeration used to add this types to pages,
    /// </summary>
    public enum MimeType
    {
        /// <summary>
        /// File Extention: .atom, .xml
        /// MIME Type: application/atom+xml
        /// Type of format: Syndication
        /// Extended form: XML
        /// Summary: <p>Atom applies to a pair of related standards. The Atom Syndication Format is an XML Language
        /// used for web feeds, while Atom Publishing Protocol is a simple HTTP-based protocol for creating and
        /// updating web resources.</p>
        /// </summary>
        [MimeTypeAttribute("application/atom+xml")] ATOM = 0, 

        /// <summary>
        /// File Extention: .rss, .xml
        /// MIME Type: application/rss+xml
        /// Type of format: Syndication
        /// Extended form: XML
        /// Summary: <p>RSS (Really Simple Syndication) is a family of Web feed formats used to publish requently updated
        /// content.</p>
        /// <ul>
        ///     <li>Really Simple Syndication (RSS 2.0)</li>
        ///     <li>RDF Site Summary (RSS 1.0 and RSS 0.90)</li>
        ///     <li>Rich Site Summary (RSS 0.91)</li>
        /// </ul> 
        /// </summary>
        [MimeTypeAttribute("application/rss+xml")] RSS, 

        /// <summary>
        /// File Extention: .js
        /// MIME Type: application/javascript or application/json
        /// Type of format: Data Interchange
        /// Summary: <div>A lightweight computer data interchange format. It is a text based
        /// human readable format for representing simple structures and associative arrays (called objects).</div>
        /// </summary>
        [MimeTypeAttribute("application/x-javascript")] JSON, 

        /// <summary>
        /// File Extention: .xml
        /// MIME Type: text/xml
        /// Type of format: Markup Language
        /// Summary: <div>The Extensible Markup Language (XML) is a general-purpose markup language. Its primary
        /// purpose is to facilitate teh  sharing of structured data across different information systems. </div>
        /// </summary>
        [MimeTypeAttribute("text/xml")] XML, 

        /// <summary>
        /// File Extention: .xhtml, .xht, .html, .htm
        /// MIME Type: application/xhtml+xml
        /// Type of format: Markup Language
        /// Extended form: XML
        /// Summary: <div>The Extensible HyperText Markup Language (XHTML) is a markup language that has the same 
        /// depth of expression as HTML, but also comforms to XML syntax.</div>
        /// </summary>
        [MimeTypeAttribute("application/xhtml+xml")] XHTML, 

        /// <summary>
        /// File Extention: .html, .htm
        /// MIME Type: text/html
        /// Type of format: Markup Language
        /// Extended to: XHTML
        /// Summary: <div>Predominant markup language for web pages.</div>
        /// </summary>
        [MimeTypeAttribute("text/html")] HTML, 

        /// <summary>
        /// File Extention: .xaml
        /// MIME Type: application/xaml+xml
        /// Type of format: Markup Language
        /// Extended from: XML
        /// Summary: <div>A declaritive XML-based language used to initialize structured values and objects. </div>
        /// </summary>
        [MimeTypeAttribute("text/html")] XAML, 

        /// <summary>
        /// File Extention: .doc
        /// MIME Type: application/msword
        /// Type of format: Word Document
        /// Summary: <div>A file extention for word processing documents</div>
        /// </summary>
        [MimeTypeAttribute("application/msword")] DOC, 

        /// <summary>
        /// File Extention: .docx
        /// MIME Type: application/vnd.openxmlformats-officedocument.wordprocessingml.document
        /// Type of format: Word Document
        /// Summary: <div>A file extention for word processing documents</div>
        /// </summary>
        [MimeTypeAttribute("application/vnd.openxmlformats-officedocument.wordprocessingml.document")] DOCX, 

        /// <summary>
        /// The rtf.
        /// </summary>
        [MimeTypeAttribute("application/rtf")] RTF, 

        /// <summary>
        /// The pdf.
        /// </summary>
        [MimeTypeAttribute("application/pdf")] PDF, 

        /// <summary>
        /// The xls.
        /// </summary>
        [MimeTypeAttribute("application/vnd.ms-excel")] XLS, 

        /// <summary>
        /// The xlsx.
        /// </summary>
        [MimeTypeAttribute("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")] XLSX, 

        /// <summary>
        /// The wsdl.
        /// </summary>
        [MimeTypeAttribute("text/xml")] WSDL, 

        /// <summary>
        /// The swf.
        /// </summary>
        [MimeTypeAttribute("application/x-shockwave-flash")] SWF, 

        /// <summary>
        /// <para>File Extention: .js</para>
        /// <para>MIME Type: application/javascript or application/json</para>
        /// <para>Type of format: Javascript file</para>
        /// <para>Summary: <para>A JavaScript file representation</para></para>
        /// </summary>
        [MimeTypeAttribute("text/javascript")] JS,

        /// <summary>
        /// <para>File Extention: .js</para>
        /// <para>MIME Type: application/javascript or application/json</para>
        /// <para>Type of format: Javascript file</para>
        /// <para>Summary: <para>A JavaScript file representation</para></para>
        /// </summary>
        [MimeTypeAttribute("text/css")] CSS,

        /// <summary>
        /// <para>File Extention: .js</para>
        /// <para>MIME Type: application/javascript or application/json</para>
        /// <para>Type of format: Javascript file</para>
        /// <para>Summary: <para>A JavaScript file representation</para></para>
        /// </summary>
        [MimeTypeAttribute("text/xml")] WEBPART, 
    }
}