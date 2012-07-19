using System;
using System.Collections.Generic;
using DowJones.Globalization;
using DowJones.Managers.AutomatedTranslation.Core;
using Factiva.Gateway.Messages.Archive.V1_0;

namespace DowJones.Managers.AutomatedTranslation
{
    class TranslateArticle : TranslateItem, ITranslateArticle
    {
        private readonly Article m_Article;

        public TranslateArticle(Article article, ContentLanguage intoLanguage) 
            : base(intoLanguage)
        {
            m_Article = (Article)article.Clone();
        }

        public Article Article
        {
            get
            {
                return m_Article;
            }
        }

        public override ContentLanguage GetLanguage()
        {
            return (ContentLanguage) Enum.Parse(typeof (ContentLanguage), m_Article.baseLanguage);
        }

        public override TextFormat GetFormat()
        {
            return TextFormat.Plain;
        }

        public override string[] GetFragments()
        {
            List<string> fragments = new List<string>();

            if(m_Article.leadParagraph != null)
            {
                foreach (Paragraph leadParagraph in m_Article.leadParagraph)
                {
                    fragments.AddRange(GetParagraphFragments(leadParagraph));
                }
            }
            
            if(m_Article.tailParagraphs != null)
            {
                foreach (Paragraph tailParagraph in m_Article.tailParagraphs)
                {
                    fragments.AddRange(GetParagraphFragments(tailParagraph));
                }
            }

            return fragments.ToArray();
                 
        }

        public override void SetFragments(string[] fragments)
        {
            int index = 0;
            if(m_Article.leadParagraph != null)
            {
                for (int i = 0; i < m_Article.leadParagraph.Length; i++)
                {
                    SetParagraphFragments(m_Article.leadParagraph[i], fragments, ref index);
                }    
            }
            
            if(m_Article.tailParagraphs != null)
            {
                for (int i = 0; i < m_Article.tailParagraphs.Length; i++)
                {
                    SetParagraphFragments(m_Article.tailParagraphs[i], fragments, ref index);
                }
            }
        }

        public override object Clone()
        {
           Article article = (Article)m_Article.Clone();
           return new TranslateArticle(article, IntoLanguage);
        }

        private static List<string> GetParagraphFragments(Paragraph paragraph)
        {
            if (paragraph == null || paragraph.Items == null) return null;

            List<string> fragments = new List<string>();
            foreach (object item in paragraph.Items)
            {
                HighlightedText highlight = item as HighlightedText;
                if (highlight != null && highlight.text != null && highlight.text.Value != null)
                {
                    fragments.Add(highlight.text.Value);
                }
                
                Text text = item as Text;
                if (text != null && text.Value != null)
                {
                    fragments.Add(text.Value);
                }
            }

            return fragments;
        }

        private static void SetParagraphFragments(Paragraph paragraph, string[] fragments, ref int index)
        {
            if (paragraph == null || paragraph.Items == null) return;

            foreach (object item in paragraph.Items)
            {
                HighlightedText highlight = item as HighlightedText;
                if (highlight != null && highlight.text != null && highlight.text.Value != null)
                {
                    highlight.text.Value = index < fragments.Length ? fragments[index] : string.Empty;
                    index++;
                    continue;
                }
                
                ELink elink = item as ELink;
                if (elink != null && elink.text != null && elink.text.Value != null)
                {
                    if(index > fragments.Length)
                    {
                        elink.text.Value =  string.Empty;
                    }
                    continue;
                }

                EntityReference entityReference = item as EntityReference;
                if (entityReference != null)
                {
                    SetEntityReferenceFragments(entityReference, fragments, ref index);
                    continue;
                }

                Text text = item as Text;
                if (text != null && text.Value != null)
                {
                    text.Value = index < fragments.Length ? fragments[index] : string.Empty;
                    index++;

                }
            }
        }

        private static void SetEntityReferenceFragments(EntityReference entityReference, ICollection<string> fragments, ref int index)
        {
            if (entityReference == null || entityReference.Items == null) return;

            for (int i = 0; i < entityReference.Items.Length; i++)
            {
                if (index > fragments.Count)
                {
                    entityReference.Items[i] = string.Empty;
                }
            }
        }
    }
}
