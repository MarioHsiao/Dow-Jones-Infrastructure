using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Assemblers
{
    public interface IAssembler<out TTarget, in TSource>
    {
        TTarget Convert(TSource source);
    }
}
