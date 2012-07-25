using System;
using System.Globalization;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace XmlViewEngine
{
    /// <summary>
    /// The XML View Engine
    /// </summary>
    public class XmlViewEngine : BuildManagerViewEngine
    {
        internal static Action<string, object> AddItemToCacheThunk =
            (key, transform) => HttpContext.Current.Cache.Add(key, transform, null, DateTime.Now.AddDays(1), TimeSpan.Zero, CacheItemPriority.High, null);

        internal static Func<string, object> GetItemFromCacheThunk =
            key => HttpContext.Current.Cache.Get(key);


        public XmlViewEngine()
            : this(null)
        {
        }

        public XmlViewEngine(IViewPageActivator viewPageActivator)
            : base(viewPageActivator)
        {
            FileExtensions = new[] {
                "xml", 
                "xsl", 
                "xslt", 
            };

            AreaViewLocationFormats = new[] {
                "~/Areas/{2}/Views/{1}/{0}.xml", 
                "~/Areas/{2}/Views/{1}/{0}.xsl", 
                "~/Areas/{2}/Views/{1}/{0}.xslt", 
                "~/Areas/{2}/Views/Shared/{0}.xml", 
                "~/Areas/{2}/Views/Shared/{0}.xsl", 
                "~/Areas/{2}/Views/Shared/{0}.xslt", 
            };
            AreaMasterLocationFormats = new[] { 
                "~/Areas/{2}/Views/{1}/{0}.xsl", 
                "~/Areas/{2}/Views/{1}/{0}.xslt", 
                "~/Areas/{2}/Views/Shared/{0}.xsl", 
                "~/Areas/{2}/Views/Shared/{0}.xslt", 
            };
            AreaPartialViewLocationFormats = new[] { 
                "~/Areas/{2}/Views/{1}/{0}.xml", 
                "~/Areas/{2}/Views/Shared/{0}.xml", 
            };

            MasterLocationFormats = new[] {
                "~/Views/{1}/{0}.xsl",
                "~/Views/{1}/{0}.xslt",
                "~/Views/Shared/{0}.xsl",
                "~/Views/Shared/{0}.xslt",
            };

            PartialViewLocationFormats =
            ViewLocationFormats = new[] { 
                "~/Views/{1}/{0}.xml",
                "~/Views/{1}/{0}.xsl",
                "~/Views/{1}/{0}.xslt",
                "~/Views/Shared/{0}.xml",
                "~/Views/Shared/{0}.xsl",
                "~/Views/Shared/{0}.xslt",
            };
        }


        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return CreateView(controllerContext, partialPath, masterPath: null);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            XmlView view;
            var layout = GetOrAddToCache(masterPath, LoadTransform);

            if (viewPath.EndsWith(".xml", true, CultureInfo.CurrentCulture))
            {
                var document = GetOrAddToCache(viewPath, LoadView);
                view = new XmlView(document, layout);
            }
            else
            {
                var document = GetXDocument(controllerContext);

                if (document == null)
                {
                    throw new ApplicationException("Unable to render view: view path is not an XML file and Model does not contain XML data");
                }

                var transform = GetOrAddToCache(viewPath, LoadTransform);

                view = new XmlView(document, transform);

                if (layout != null)
                    view = new XmlView(view, layout);
            }

            return view;
        }

        private static XDocument GetXDocument(ControllerContext controllerContext)
        {
            XDocument document = null;

            var model = controllerContext.Controller.ViewData.Model;

            if (model is XDocument)
                document = (XDocument)model;
            
            else if (model is string)
                document = XDocument.Parse((string)model);

            else if (model is XmlDocument)
                document = XDocument.Parse(((XmlDocument)model).OuterXml);
            
            else if (model is XmlReader)
                document = XDocument.Load((XmlReader)model);
            
            return document;
        }

        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            return VirtualPathProvider.FileExists(virtualPath);
        }

        protected virtual XslCompiledTransform LoadTransform(string path)
        {
            XslCompiledTransform transform = null;

            if (VirtualPathProvider.FileExists(path))
            {
                transform = new XslCompiledTransform();
                using (var reader = new XmlTextReader(VirtualPathProvider.GetFile(path).Open()))
                    transform.Load(reader);
            }

            return transform;
        }

        protected virtual XDocument LoadView(string path)
        {
            XDocument view = null;

            if (VirtualPathProvider.FileExists(path))
            {
                using (var stream = VirtualPathProvider.GetFile(path).Open())
                    view = XDocument.Load(stream);
            }

            return view;
        }

        private T GetOrAddToCache<T>(string path, Func<string, T> factory)
        {
            if (string.IsNullOrWhiteSpace(path))
                return default(T);

            var key = string.Format("XmlView:{0}", path.ToLower());

            T item;

            var cached = GetItemFromCacheThunk(key);

            if (cached != null && cached is T)
                item = (T)cached;
            else
            {
                item = factory(path);
                AddItemToCacheThunk(key, item);
            }

            return item;
        }
    }
}
