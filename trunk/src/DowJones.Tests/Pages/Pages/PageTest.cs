using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DowJones.Globalization;
using DowJones.Session;
using DowJones.Infrastructure.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Factiva.Gateway.Messages.Assets.Pages.V1_0;

namespace DowJones.Pages
{
    [TestClass]
    public class PageTest : AbstractUnitTest
    {
        //public PageAssetsManager pageAssetsManager = new PageAssetsManager(ControlDataManager.GetLightWeightUserControlData("snapshot5", "passwd", "16"), new Preferences.Preferences { InterfaceLanguage = "en", ContentLanguages = new ContentLanguageCollection() }, new Product("Np", "SNAPSHOT"));
        public PageAssetsManager pageAssetsManager = new PageAssetsManager(ControlDataManager.GetLightWeightUserControlData("made5204", "made5204", "16"), new Preferences.Preferences { InterfaceLanguage = "en", ContentLanguages = new ContentLanguageCollection() }, new Product("CM", "COMMUNICATOR"));

        private Factiva.Gateway.Messages.Assets.Pages.V1_0.Page GetPageById(string pageId)
        {
            Factiva.Gateway.Messages.Assets.Pages.V1_0.Page page = pageAssetsManager.GetPage(pageId, false, true);

            Console.WriteLine(page.Id + "|" + page.ShareProperties.AssignedScope.ToString());
            Console.WriteLine("");
            if (page.ModuleCollection != null && page.ModuleCollection.Count > 0)
            {
                Console.WriteLine("Module Collection");
                foreach (Module module in page.ModuleCollection)
                {
                    Console.WriteLine("\t" + module.Title + "|" + ((ModuleEx)module).GetType());
                }
            }

            return page;
        }

        private Module GetModuleById(string moduleId)
        {
            ModuleEx module = (ModuleEx)(pageAssetsManager.GetModuleById(moduleId));

            Console.WriteLine(module.Id + "|" + module.Title + "|" + module.Description + "|" + module.ModuleQualifier + "|" + module.GetType());
            Console.WriteLine("");
            Console.WriteLine("QueryFilters");
            if (module.QueryFilters != null)
            {
                Console.WriteLine("\tInherit: " + module.QueryFilters.Inherit);
                if (module.QueryFilters.QueryFilterCollection != null)
                {
                    foreach (QueryFilter qf in module.QueryFilters.QueryFilterCollection)
                    {
                        Console.WriteLine("\t" + qf.Type + "|" + qf.Text);
                    }
                }
            }
            Console.WriteLine("ModuleProperties");
            if (module.ModuleProperties != null && module.ModuleProperties.ModuleMetaData != null)
            {
                if (module.ModuleProperties.ModuleMetaData.CategoryCollection != null)
                {
                    Console.WriteLine("CategoryCollection: " + module.ModuleProperties.ModuleMetaData.CategoryCollection.Count());
                    foreach (MetadataField metadataField in module.ModuleProperties.ModuleMetaData.CategoryCollection)
                    {
                        Console.WriteLine("\t" + metadataField.Text + "|" + metadataField.IsDefault);
                    }
                }
                if (module.ModuleProperties.ModuleMetaData.IndustryCollection != null)
                {
                    Console.WriteLine("IndustryCollection: " + module.ModuleProperties.ModuleMetaData.IndustryCollection.Count());
                    foreach (MetadataField metadataField in module.ModuleProperties.ModuleMetaData.IndustryCollection)
                    {
                        Console.WriteLine("\t" + metadataField.Text + "|" + metadataField.IsDefault);
                    }
                }
                if (module.ModuleProperties.ModuleMetaData.RegionCollection != null)
                {
                    Console.WriteLine("RegionCollection: " + module.ModuleProperties.ModuleMetaData.RegionCollection.Count());
                    foreach (MetadataField metadataField in module.ModuleProperties.ModuleMetaData.RegionCollection)
                    {
                        Console.WriteLine("\t" + metadataField.Text + "|" + metadataField.IsDefault);
                    }
                }


            }
            return module;
        }

        //        [TestMethod]
        public void UpdateModuleTest()
        {
            var moduleId = "24496";
            Module module = GetModuleById(moduleId);
            //Change now
            ModuleEx moduleEx = (ModuleEx)module;

            moduleEx.ShareProperties = new ShareProperties { };

            moduleEx.Title = moduleEx.Title + DateTime.Now.ToShortDateString();
            moduleEx.Description = moduleEx.Description + DateTime.Now.ToShortDateString();

            moduleEx.QueryFilters.Inherit = true;
            moduleEx.QueryFilters.QueryFilterCollection.Add(new QueryFilter { Type = FilterType.Region, Text = "HK" });

            moduleEx.ModuleQualifier = Factiva.Gateway.Messages.Assets.Pages.V1_0.AccessQualifier.Account;
            moduleEx.ModuleProperties = new ModuleProperties();
            moduleEx.ModuleProperties.ModuleMetaData = new Metadata();
            moduleEx.ModuleProperties.ModuleMetaData.IndustryCollection = new MetadataFieldCollection();
            moduleEx.ModuleProperties.ModuleMetaData.IndustryCollection.Add(new MetadataField { IsDefault = true, Text = "icomp" });
            moduleEx.ModuleProperties.ModuleMetaData.RegionCollection = new MetadataFieldCollection();
            moduleEx.ModuleProperties.ModuleMetaData.RegionCollection.Add(new MetadataField { IsDefault = false, Text = "CHINA" });

            pageAssetsManager.UpdateModule(moduleEx);

            GetModuleById(moduleId);
        }


        [TestMethod]
        public void GetPageListTest()
        {
            //ControlData = ControlDataManager.GetLightWeightUserControlData("made5204", "made5204", "16");
            PageListInfoCollection PageList = pageAssetsManager.GetPageListInfoCollection(new List<PageType> { PageType.CommunicatorDashboard }, Factiva.Gateway.Messages.Assets.Common.V2_0.SortOrder.Ascending, SortBy.Name);
            foreach (var page in PageList)
            {
                Console.WriteLine(page.Id + "|" + page.PageProperties.Title);
            }

        }

        //[TestMethod]
        public void GetPageByIdTest()
        {
            GetPageById("12703");
        }
    }
}
