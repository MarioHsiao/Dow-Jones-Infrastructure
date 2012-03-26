using System;
using System.Xml.Serialization;

namespace DowJones.Tools.Managers.Charting
{
    /// ChartType
    ///</summary>
    /// <remark>This enumeration contains the chart types supported.</remark>
    [XmlType(Namespace = "")]
    [Serializable]
    public enum 
        OutputChartType
    {
        /// <summary>
        /// Flash utilizes vector and raster graphics, a native scripting language called ActionScript and bidirectional streaming of video and audio.Html outputed by this format will not have the ActiveX fix for Internet Explorer Browsers.
        /// </summary>
        FLASH = 0,
        /// <summary>
        /// Flash utilizes vector and raster graphics, a native scripting language called ActionScript and bidirectional streaming of video and audio. Html outputed by this format will have the ActiveX fix for Internet Explorer Browsers.
        /// </summary>
        FLASH_WITH_ACTIVEX_FIX,
        /// <summary>
        /// GIF {Graphics Interchange Format} Is a bitmap image format for pictures that use (or fewer) distinct colors. Interativity will be provided by MAP/AREA html tags, if applicable.
        /// </summary>
        GIF,
        /// <summary>
        /// GIF {Graphics Interchange Format} Is a bitmap image format for pictures that use (or fewer) distinct colors. Interativity will be provided by JavaScript solution and backed up by MAP/AREA html tags, if applicable.
        /// </summary>
        GIF_WITH_JAVASCRIPT_INTERACTIVITY,
        /// <summary>
        /// {Scalable Vector Graphics}
        /// </summary>
        SVG,
        /// <summary>
        /// PNG (Portable Network Graphics) is a lossless bitmap image format. Interativity will be provided by MAP/AREA html tags, if applicable.
        /// </summary>
        PNG,
        /// <summary>
        /// PNG (Portable Network Graphics) is a lossless bitmap image format. Interativity will be provided by JavaScript solution and backed up by MAP/AREA html tags, if applicable. 
        /// </summary>
        PNG_WITH_JAVASCRIPT_INTERACTIVITY,
        /// <summary>
        /// JPEG (Joint Photographic Experts Group) loose compression image format for photographs. Interativity will be provided by MAP/AREA html tags, if applicable.
        /// </summary>
        JPEG,
        /// <summary>
        /// JPEG (Joint Photographic Experts Group) loose compression image format for photographs. Interativity will be provided by JavaScript solution and backed up by MAP/AREA html tags, if applicable.
        /// </summary>
        JPEG_WITH_JAVASCRIPT_INTERACTIVITY,
        /*/// <summary>
        /// PDF {Portable Document Format}
        /// </summary>
        PDF,
        /// <summary>
        /// EPS {Encapsulated PostScript}
        /// </summary>
        EPS,
        /// <summary>
        /// TIFF {Tagged Image File Format}
        /// </summary>
        TIFF,
        /// <summary>
        /// WBMP {wireless bitmap}
        /// </summary>
        WBMP,*/
    }

}
