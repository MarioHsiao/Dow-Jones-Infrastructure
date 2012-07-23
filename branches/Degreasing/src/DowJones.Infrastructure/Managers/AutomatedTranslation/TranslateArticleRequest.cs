using System;
using DowJones.Globalization;
using DowJones.Managers.AutomatedTranslation.Core;
using Factiva.Gateway.Messages.Archive.V1_0;

namespace DowJones.Managers.AutomatedTranslation
{
    public class TranslateArticleRequest : TranslateRequest
    {
        private readonly Article m_Article;

        public TranslateArticleRequest(Article article, ContentLanguage targetLanguage)
            :base(targetLanguage)
        {
            if(article == null)
            {
                throw new ArgumentNullException("article");
            }

            m_Article = article;
        }

        public override ITranslateItem GetTranslateItem()
        {
            return new TranslateArticle(m_Article, TargetLanguage);
        }
    }
}
