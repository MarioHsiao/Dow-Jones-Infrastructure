using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DowJones.Web.Showcase.Models
{
    public class SearchTerm
    {
        public string type { get; set; }
        public string name { get; set; }
        public string value { get; set; }


        public override string ToString()
        {

            string output = "";
            switch (this.type.ToLower())
            {
                case "connector":
                    output = " " + this.value + " ";
                    break;
                case "text":
                    output = this.value;
                    break;
                default:
                    output = this.type + ":" + this.value;
                    break;

            }

            return output;
        }
    }
}