using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace DowJones.Pages.Layout
{
    [DataContract(Name = "gridLayout", Namespace = "")]
    public class GridLayout : PageLayout
    {
        public int DefaultWidth = 3;
        public int DefaultCellWidth = 1;
        public int DefaultCellHeight = 1;

        [DataMember(Name = "width")]
        public int Width { get; set; }

        [DataMember(Name = "cells")]
        public List<Cell> Cells { get; private set; }

        public GridLayout()
        {
            Cells = new List<Cell>();
            Width = DefaultWidth;
        }

        public override void AddModule(int moduleId)
        {
            var lastRow = 0;
            var lastColumn = 0;

            if(Cells.Any())
            {
                lastRow = Cells.Max(x => x.Row);
                lastColumn = Cells.Where(x => x.Row == lastRow).Max(x => x.Column);
            }

            int row = lastRow, column = lastColumn + 1;
            
            if(column > Width)
            {
                column = 0;
                row = lastRow + 1;
            }

            Cells.Add(new Cell {
                ModuleId = moduleId,
                Column = column,
                Row = row,
                Width = DefaultCellWidth,
                Height = DefaultCellHeight,
            });
        }

        public override void RemoveModule(int moduleId)
        {
            Cells.RemoveAll(x => x.ModuleId == moduleId);
        }

        [DataContract(Name = "cell", Namespace = "")]
        public class Cell
        {
            [DataMember(Name = "moduleId")]
            public int ModuleId { get; set; }

            [DataMember(Name = "row")]
            public int Row { get; set; }

            [DataMember(Name = "column")]
            public int Column { get; set; }

            [DataMember(Name = "width")]
            public int Width { get; set; }

            [DataMember(Name = "height")]
            public int Height { get; set; }
        }
    }
}