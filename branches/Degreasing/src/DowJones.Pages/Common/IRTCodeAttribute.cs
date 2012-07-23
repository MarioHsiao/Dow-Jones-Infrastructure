using System;

namespace DowJones.Pages
{
    public class IRTCodeAttribute : Attribute
    {
        public readonly string Code;

        public IRTCodeAttribute(string code)
        {
            Code = code;
        }
    }
}