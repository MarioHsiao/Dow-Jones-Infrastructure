// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Core.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the Declarations type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DowJones.Charting.Core.Data
{
    internal struct Declarations
    {
        internal const string CORDA_COLUMN = "<cit:column name=\"{0}\"{1}{2}{3}{4}{5}{6} />";

        internal const string CORDA_DATA_ITEM = "<cit:data-item value=\"{0}\"{1}{2}{3}{4}{5}{6} />";

        internal const string CORDA_TIME_ITEM = "<cit:time-item date=\"{0}\" value=\"{1}\" drilldown=\"{2}\" target=\"{3}\" hover=\"{4}\" note=\"{5}\" note-target=\"{6}\" description=\"{7}\" />";

        internal const string CORDA_STOCK_ITEM = "<cit:stock-data-item date=\"{0}\" high=\"{1}\" low=\"{2}\" open =\"{3}\" close=\"{4}\" hover=\"{5}\" drilldown=\"{6}\" />";
    }
    
}
