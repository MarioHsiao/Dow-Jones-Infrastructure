using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace DowJones.Web.Razor.Common
{
    public class ArrayCodeAttributeArgument<T> : CodeAttributeArgument
    {
        public ArrayCodeAttributeArgument()
        {
        }

        public ArrayCodeAttributeArgument(string name, IEnumerable<T> array)
        {
            Name = name;
            SetValue(array);
        }

        public void SetValue(IEnumerable<T> array)
        {
            CodePrimitiveExpression[] primativeExpressions =
                array
                    .Select(x => new CodePrimitiveExpression(x))
                    .ToArray();

            Value = new CodeArrayCreateExpression(typeof(T[]), primativeExpressions);
        }
    }
}