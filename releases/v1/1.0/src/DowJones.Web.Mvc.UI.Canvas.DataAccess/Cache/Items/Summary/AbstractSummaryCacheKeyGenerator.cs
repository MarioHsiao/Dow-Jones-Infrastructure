// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractSummaryCacheKeyGenerator.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Canvas.DataAccess.Cache.Items.Summary
{
    public class AbstractSummaryCacheKeyGenerator<T> : AbstractModuleCacheKeyGenerator<T> where T : AbstractModuleCacheKeyGenerator<T>
    {
        protected AbstractSummaryCacheKeyGenerator() 
            : base(string.Empty)
        {
        }

        protected AbstractSummaryCacheKeyGenerator(string moduleId, Product product = Product.Np)
            : base(moduleId, product)
        {
            Guid = moduleId;
        }

        [JsonProperty(PropertyName = CacheKeyConstants.Guid)]
        public string Guid { get; set; }
    }
}
