using System;
using DowJones.Formatters;

namespace DowJones.Models.Charting
{
    public interface IDataPoint
    {
        DateTime? Date { get; set; }

        string DateDisplay { get; set; }
        
        Number DataPoint { get; set; }
    }
}