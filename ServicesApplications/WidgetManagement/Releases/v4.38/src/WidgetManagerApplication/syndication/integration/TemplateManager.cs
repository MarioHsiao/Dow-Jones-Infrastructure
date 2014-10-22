using System;
using System.IO;
using System.Web;
using System.Web.Caching;

namespace EMG.widgets.ui.syndication.integration.alertWidget
{
    /// <summary>
    /// 
    /// </summary>
    internal class TemplateManager
    {
        private static readonly TemplateManager instance = new TemplateManager();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static TemplateManager Instance
        {
            get { return instance; }
        }

        private TemplateManager()
        {
           
        }

        /// <summary>
        /// Gets the template.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public string GetTemplate(Type type, string filePath)
        {
            Cache cache = HttpContext.Current.Cache;
            string initialValue = (string)cache.Get(type.ToString());
            if (string.IsNullOrEmpty(initialValue) || string.IsNullOrEmpty(initialValue.Trim()))
            {
                  initialValue = AddToCache(type, filePath);
            }
            return initialValue;
        }

        /// <summary>
        /// Gets the template contents.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private static string GetTemplateContents(string path)
        {
            return File.ReadAllText(HttpContext.Current.Server.MapPath(path));
        }

        /// <summary>
        /// Adds to cache.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="filePath">The file path.</param>
        private static string AddToCache(Type type, string filePath)
        {
            // Add that value to 
            Cache cache = HttpContext.Current.Cache;
            string data = GetTemplateContents(filePath);
            
            if (!string.IsNullOrEmpty(data) && !string.IsNullOrEmpty(data.Trim()) )
            {
                cache.Add(
                type.Name,
                data.Trim(),
                new CacheDependency(
                    HttpContext.Current.Server.MapPath(filePath)
                    ),
                Cache.NoAbsoluteExpiration,
                Cache.NoSlidingExpiration,
                CacheItemPriority.High,
                null
                );
                return data;
            }
            return null;
        }
    }
}