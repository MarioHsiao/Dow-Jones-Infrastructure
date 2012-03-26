using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Infrastructure;

namespace DowJones.Ajax.Article
{
    public class RenderItem : IRenderItem
    {

        public string ItemText { get; set; }

        public string ItemValue { get; set; }

        public MarkUpType ItemMarkUp { get; set; }

        public string ItemClass { get; set; }

        public EntityLinkData ItemEntityData { get; set; }

        public PostProcessData ItemPostProcessData { get; set; }
    }
}
