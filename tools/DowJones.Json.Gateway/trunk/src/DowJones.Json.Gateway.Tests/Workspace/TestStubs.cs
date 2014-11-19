using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace.Transactions;

namespace DowJones.Json.Gateway.Tests.Workspace
{
    internal static class TestStubs
    {
        internal static UpdateWorkspaceRequest getUpdateRequest1()
        {
            ManualWorkspace manualWorkspace = new ManualWorkspace();
            manualWorkspace.Id = 11939;
            manualWorkspace.Code = "UnitTest_NN79O";
            Section baseSection = new Section();
            ArticleItem item;
          
            List<string> accessionNumbers = GetAccessionNumbers();
            foreach (string s in accessionNumbers)
            {
                item = new ArticleItem();
                item.AccessionNumber = string.Empty;
                item.ProviderId = s;
                item.ProviderType = "WSJ";

                baseSection.ItemCollection.Add(item);
            }
            manualWorkspace.SectionCollection.Add(baseSection);

            CollectionWorkspaceProperties collectionWorkspaceProperties = new CollectionWorkspaceProperties();
            collectionWorkspaceProperties.Name = "UnitTest_NN79O";
            manualWorkspace.Properties = collectionWorkspaceProperties;

            UpdateWorkspaceRequest updateWorkspaceRequest = new UpdateWorkspaceRequest();
            updateWorkspaceRequest.Workspace = manualWorkspace;
            updateWorkspaceRequest.Mode = UpdateWorkspaceMode.Save;

            return updateWorkspaceRequest;
        }

        internal static UpdateWorkspaceRequest getUpdateRequest()
        {
            long workspaceId = 11939;
            ManualWorkspace manualWorkspace = new ManualWorkspace();
            manualWorkspace.Id = workspaceId;
            manualWorkspace.Segment = Segment.Unspecified;
            manualWorkspace.Product = Product.Advisor;
            manualWorkspace.Code = "UnitTest_NN79O";

            CollectionWorkspaceProperties collectionWorkspaceProperties = new CollectionWorkspaceProperties();
            collectionWorkspaceProperties.Name = "UnitTest_NN79O";
            collectionWorkspaceProperties.DisplayName = "UnitTest_NN79O";
            collectionWorkspaceProperties.Description = "This is a proxy workspace";
            collectionWorkspaceProperties.CreationDate = DateTime.Now;
            collectionWorkspaceProperties.LastModifiedDate = DateTime.Now;
            collectionWorkspaceProperties.LastContentModifiedDate = DateTime.Now;
            collectionWorkspaceProperties.Audience = new Audience();
            collectionWorkspaceProperties.Audience.AudienceOptions = AudienceOptions.InternalAccount;
            collectionWorkspaceProperties.Audience.ProfileId = "";
            collectionWorkspaceProperties.Audience.ProxyCredentials.AuthenticationScheme =
                AuthenticationScheme.UserId;
            collectionWorkspaceProperties.Audience.ProxyCredentials.EmailAddress = "";
            collectionWorkspaceProperties.Audience.ProxyCredentials.Namespace = "";
            collectionWorkspaceProperties.Audience.ProxyCredentials.UserId = "";
            collectionWorkspaceProperties.Audience.ProxyCredentials.Password = "";
            collectionWorkspaceProperties.Audience.ProxyCredentials.EncryptedToken = "";

            collectionWorkspaceProperties.EmailDistributionId = 1234;
            collectionWorkspaceProperties.AreFeedsActive = false;
            collectionWorkspaceProperties.HasRssFeed = true;
            collectionWorkspaceProperties.HasPodcastFeed = false;
            collectionWorkspaceProperties.HasWidget = false;
            collectionWorkspaceProperties.AccessScope = AccessScope.Platform;
            collectionWorkspaceProperties.Scope = WorkspaceScopeType.Platform;
            collectionWorkspaceProperties.WidgetFeedsActive = false;

            //CollectionMetadata metaData = new CollectionMetadata();
            /*MetaDataCodeCollection metaDataCodeCollection = new MetaDataCodeCollection();
            metaDataCodeCollection.Add("MetaData1");
            metaDataCodeCollection.Add("MetaData2");*/
            collectionWorkspaceProperties.CollectionMetadata = new List<string>(){"MetaData1","MetaData2"};
            manualWorkspace.Properties = collectionWorkspaceProperties;

            Section baseSection = new Section();
            //Items Collection
            List<string> accessionNumbers = GetAccessionNumbers();
            var item1 = new LinkItem();
            item1.Description = "UnitTest_LinkItem Description";
            item1.Comment = "UnitTest_LinkItem Comment";
            item1.PublicationDate = new DateTime(2005, 1, 1);
            item1.Uri = "http://www.medicalnewstoday.com/articles/109875.php";
            item1.Title = "LinkItem Title";
            item1.Position = 1;
            item1.IsPublished = true;
            item1.Type = LinkType.DocUrl;
            item1.CreationDate = DateTime.UtcNow;
            item1.LastModifiedDate = DateTime.UtcNow;


            var item2 = new ImageItem();
            item2.Comment = "UnitTest_ImageItem Comment";
            item2.Uri = "http://insight.int.factiva.com//Pages/getImage.aspx?g=e9e18c87-2b94-4bf7-9d40-aae863c6e041&amp;d=26";
            item2.PostbackUri = "http://insight.int.factiva.com//chartView.aspx?cid=2768&amp;rid=1&amp;pid=9&amp;d=26";
            item2.Title = "ImageItem Title";
            item2.Position = 2;
            item2.Status = 1;
            item2.CreationDate = DateTime.UtcNow;
            item2.LastModifiedDate = DateTime.UtcNow;

            ArticleItem item;
            int i = 0;
            foreach (string s in accessionNumbers)
            {
                item = new ArticleItem();
                item.AccessionNumber = string.Empty;
                item.ContentCategory = ContentCategory.WebSites;
                item.ProviderId = s;
                item.ProviderType = "WSJ";
                item.Importance = Importance.Hot;
                item.Comment = "UnitTest_ImageItem Comment";
                item.Id = 9970395;
                item.Position = 3;
                item.Status = 1;
                item.CreationDate = DateTime.Now;
                item.LastModifiedDate = DateTime.Now;
                switch (i % 4)
                {
                    case 0:
                        item.Importance = Importance.MustRead;
                        break;
                    case 1:
                        item.Importance = Importance.New;
                        break;
                    case 2:
                        item.Importance = Importance.Hot;
                        break;
                    default:
                        item.Importance = Importance.Normal;
                        break;
                }
                baseSection.ItemCollection.Add(item);
                i++;
            }
            baseSection.ElementCollection = new ElementCollection { new Element { StyleName = "test", ImageSize = 1 } };
            manualWorkspace.SectionCollection.Add(baseSection);
             baseSection.SubSectionCollection = new SubSectionCollection();
             SubSection subSection = new SubSection();
             subSection.Id = 2343;
             subSection.Name = "sub section";
             subSection.ItemCollection = new ItemCollection { new SeparatorItem { CreatedBy = "Ruchi", IsPublished = true, Status = 0, TagCollection = new TagCollection { "testtag" } } };
             baseSection.SubSectionCollection.Add(subSection);
            
              FeedAssets feedAssets = new FeedAssets();
              FeedAssetCollection feedAssetCollection = new FeedAssetCollection();
              FeedAsset feedAsset = new FeedAsset();
              feedAsset.AssetIdCollection.Add(2343);
              feedAsset.Type = FeedAssetType.Workspace;
              feedAssetCollection.Add(feedAsset);
              feedAssets.FeedAssetCollection = feedAssetCollection;
              manualWorkspace.FeedAssets = feedAssets;
              DeliveryInfoCollection dInfoCollection = new DeliveryInfoCollection();
              DeliveryInfo oInfo = new DeliveryInfo();
              oInfo.Id = 2323;
              oInfo.AddNewsletterAttachment = true;
              oInfo.AttachmentTemplateId = 1;
              oInfo.AttachmentType = AttachmentType.HTML;
              oInfo.Message = "This is  a sample message";
              oInfo.OutputFormat = OutputFormat.HTMLEmailBody;
              oInfo.Subject = "TEst email";

              Receipient recepient = new Receipient();
              Email email = new Email();
              email.DisplayName = "ruchi goyal";
              email.Id = "ruchi.goyal@dowjones.com";
              recepient.EmailFormat = "HTML";
              recepient.ToCollection.Add(email);
              oInfo.Receipient = recepient;
              dInfoCollection.Add(oInfo);

            //manualWorkspace.DeliveryInfoCollection = dInfoCollection;
            UpdateWorkspaceRequest updateWorkspaceRequest = new UpdateWorkspaceRequest();
            updateWorkspaceRequest.Workspace = manualWorkspace;
            updateWorkspaceRequest.Mode = UpdateWorkspaceMode.Save;

            return updateWorkspaceRequest;
        }

        private static List<string> GetAccessionNumbers()
        {
            return new List<string>(
                new string[]
                    {
                          "SB10001424052702304256404579453731910023274","SB10001424173140724841304579278272302959250","SB10001424052702303945704579391243579396718","SB10001424052702304071004579411433293118344","SB10001424173140724074604579450853316844762","SB10001424173140724743404579374550445530602"
                    }
                );
        }
    }
}
