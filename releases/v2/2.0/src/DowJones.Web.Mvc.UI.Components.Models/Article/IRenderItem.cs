using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Infrastructure;

namespace DowJones.Web.Mvc.UI.Components.Article
{

    public interface IRenderItem_1
    {
        string ItemValue{get;set;}
        
        MarkUpType ItemMarkUp{get;set;}

        string ItemClass{get;set;}

        string ItemText{get;set;}

        EntityLinkData ItemEntityData { get; set; }

        PostProcessData ItemPostProcessData { get; set; }
    }


    public class RenderItem_1:IRenderItem_1
    {
    
        public string ItemText{get;set;}
        
        public string  ItemValue{get;set;}

        public MarkUpType  ItemMarkUp{get;set;}
        
        public string  ItemClass{get;set;}

        public EntityLinkData ItemEntityData { get; set; }

        public PostProcessData ItemPostProcessData{get;set;}
    }

    public class EntityLinkData
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }
    }

    public class PostProcessData
    {
        public PostProcessing Type {get;set;}
        public string ElinkValue {get;set;}
        public string ElinkText{get;set;}
    }
}
