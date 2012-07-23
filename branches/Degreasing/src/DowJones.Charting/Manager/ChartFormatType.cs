/* 
 * Author: Infosys
 * Date: May/11/2010
 * Purpose: Chart Format type
 * 
 * 
 * Mod Log
 * -----------------------------------------------------------------------------
 * Modified by                          Date                    Purpose
 * -----------------------------------------------------------------------------
 */
using System;
using System.Xml.Serialization;

namespace DowJones.Charting.Manager
{
    [XmlType(Namespace = "")]
    [Serializable]
    public enum ChartFormatType
    {
        // Defaulted for any online view
        Online = 0,
        // Used in export or print functionality
        Print
    }
}
