using DowJones.Infrastructure;

namespace DowJones.Ajax.Article
{
    public interface IRenderItem
    {
        string ItemValue { get; set; }

        MarkUpType ItemMarkUp { get; set; }

        bool Highlight { get; set; }

        string Caption { get; set; }

        string Credit { get; set; }

        string Source { get; set; }

        string Title { get; set; }

        string ItemClass { get; set; }

        string ItemText { get; set; }

        EntityLinkData ItemEntityData { get; set; }

        PostProcessData ItemPostProcessData { get; set; }
    }
}