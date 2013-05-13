// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseLine.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the BaseLine type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace DowJones.Charting.Core.Data
{
    [Obsolete("This class should no longer be used due to Corda replacement project")] 
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
