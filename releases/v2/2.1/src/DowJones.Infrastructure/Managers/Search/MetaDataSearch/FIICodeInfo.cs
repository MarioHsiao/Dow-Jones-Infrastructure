using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Managers.Search.MetaDataSearch
{
    public class FIICodeInfo
    {
        public string Code { get; set; }
        public string Value { get; set; }
        public FIICodeType Type { get; set; }
    }

    public enum FIICodeType
    { 
        Industry,
        Region,
        Subject
    }
}
