// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseLine.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the BaseLine type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DowJones.Charting.Core.Data
{
    public class BaseLine : Line
    {
        public BaseLine()
        {
            FillType = FillType.None;
        }

        public double Value { get; set; }

        public bool IsEnabled { get; set; }
    }
}
