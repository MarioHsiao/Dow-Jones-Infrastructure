﻿using DowJones.Infrastructure;

namespace DowJones.Ajax.Article
{
    public class RenderItem : IRenderItem
    {
        public string ItemText { get; set; }

        public string ItemValue { get; set; }

        public MarkUpType ItemMarkUp { get; set; }

        public bool Highlight { get; set; }

        public string Caption { get; set; }

        public string Credit { get; set; }

        public string Source { get; set; }

        public string Title { get; set; }

        public string ItemClass { get; set; }

        public EntityLinkData ItemEntityData { get; set; }

        public PostProcessData ItemPostProcessData { get; set; }
    }
}
