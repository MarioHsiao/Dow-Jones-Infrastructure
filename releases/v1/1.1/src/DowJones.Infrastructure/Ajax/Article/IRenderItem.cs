using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Infrastructure;

namespace DowJones.Ajax.Article
{

    public interface IRenderItem
    {
        string ItemValue{get;set;}
        
        MarkUpType ItemMarkUp{get;set;}

        string ItemClass{get;set;}

        string ItemText{get;set;}

        EntityLinkData ItemEntityData { get; set; }

        PostProcessData ItemPostProcessData { get; set; }
    }
   
}
