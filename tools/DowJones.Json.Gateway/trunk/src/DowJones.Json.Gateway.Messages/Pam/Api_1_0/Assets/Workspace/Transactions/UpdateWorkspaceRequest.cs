using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Schema;
using System.Xml.Serialization;
using DowJones.Json.Gateway.Attributes;
using DowJones.Json.Gateway.Converters;
using DowJones.Json.Gateway.Interfaces;
using DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace.Transactions
{
    /// <summary>
    /// 	<para>
    /// 	</para>
    /// 	<example>
    /// 		This sample shows how to use class Factiva.Gateway.Messages.Assets.Workspaces.V2_0.UpdateWorkspaceRequest
    /// 		<code>
    ///public void UpdateAutomaticWorkspace()
    ///        {
    ///            UpdateWorkspaceRequest updateWorkspaceRequest = new UpdateWorkspaceRequest();
    ///            AutomaticWorkspace automaticWorkspace = new AutomaticWorkspace();
    ///            automaticWorkspace.Id = 315;
    ///            //Automatic Workspace
    ///            AutomaticWorkspaceProperties automaticWorkspaceProperties = new AutomaticWorkspaceProperties();
    ///            automaticWorkspaceProperties.Name = "dacosta-Updated1";
    ///            automaticWorkspaceProperties.Description = "This is a proxy workspace";
    ///            automaticWorkspaceProperties.Audience = new Audience();
    ///            automaticWorkspaceProperties.Audience.AudienceOptions = AudienceOptions.InternalAccount;
    ///            automaticWorkspaceProperties.EmailDistributionId = 1234;
    ///            automaticWorkspaceProperties.AreFeedsActive = false;
    ///            automaticWorkspaceProperties.HasRssFeed = true;
    ///            automaticWorkspaceProperties.HasPodcastFeed = false;
    ///            automaticWorkspaceProperties.HasWidget = false;
    ///            automaticWorkspace.Properties = automaticWorkspaceProperties;
    ///            //Items Collection
    ///            ContentItemCollection contentItemCollection = new ContentItemCollection();
    ///            ArticleItem articleItem = new ArticleItem();
    ///            articleItem.AccessionNumber = "111";
    ///            articleItem.ContentCategory = ContentCategory.Publications;
    ///            articleItem.Comment = "No Comment";
    ///            articleItem.Importance = Importance.New;
    ///            articleItem.Position = 1;
    ///            //articleItem.IsPublished = false;
    ///            articleItem.Status = 2;
    ///            articleItem.CreationDate = DateTime.Now;
    ///            contentItemCollection.Add(articleItem);
    ///            automaticWorkspace.ItemsCollection = contentItemCollection;
    ///            updateWorkspaceRequest.Workspace = automaticWorkspace;
    ///            ServiceResponse serviceResponse =
    ///                WorkspacesService.UpdateWorkspace(ControlDataManager.Clone(m_ControlData), updateWorkspaceRequest);
    ///            Assert.IsNotNull(serviceResponse);
    ///            if (serviceResponse.rc != 0)
    ///            {
    ///                Console.WriteLine("Return Code: {0}", serviceResponse.rc);
    ///                Assert.Fail("Return Code: {0}", serviceResponse.rc);
    ///            }
    ///            else
    ///            {
    ///                object responseObj;
    ///                long responseObjRC = serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object, out responseObj);
    ///                if (responseObjRC == 0)
    ///                {
    ///                    UpdateWorkspaceResponse objUpdateWorkspaceResponse = (UpdateWorkspaceResponse)responseObj;
    ///                    Assert.IsNotNull(objUpdateWorkspaceResponse);
    ///                    try
    ///                    {
    ///                        using (
    ///                            FileStream stream =
    ///                                GetFileStream(@"Temp", "UpdateAutomaticWorkspace.xml"))
    ///                        {
    ///                            TextWriter writer = new StreamWriter(stream);
    ///                            XmlSerializer serializer = new XmlSerializer(typeof(UpdateWorkspaceResponse));
    ///                            serializer.Serialize(writer, objUpdateWorkspaceResponse);
    ///                            writer.Close();
    ///                        }
    ///                    }
    ///                    catch (Exception ex)
    ///                    {
    ///                        m_Log.Error(ex.Message);
    ///                    }
    ///                }
    ///                else
    ///                {
    ///                    Assert.Fail("responseObjRC: {0}", responseObjRC);
    ///                }
    ///            }
    ///        }
    /// 		</code>
    /// 	</example>
    /// </summary>
    [ServicePath("2.0/Workspace")]
    //[ServicePath("Workspace/")]
    [DataContract(Name = "UpdateWorkspace", Namespace = "")]
    [JsonObject(Title = "UpdateWorkspace")]
    [XmlRoot(ElementName = "UpdateWorkspace", Namespace = Declarations.SchemaVersion, IsNullable = false), Serializable]
    [XmlInclude(typeof(AutomaticWorkspace))]
    [XmlInclude(typeof(ManualWorkspace))]
    public class UpdateWorkspaceRequest : IPostJsonRestRequest
    {
        [JsonProperty(PropertyName = "mode")] 
        [XmlElement(ElementName = "mode", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "mode")]
        public UpdateWorkspaceMode __mode;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool __modeSpecified;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public UpdateWorkspaceMode Mode
        {
            get { return __mode; }
            set { __mode = value; __modeSpecified = true; }
        }

        [JsonProperty(PropertyName = "workspace")] 
        [XmlElement(Type = typeof(Workspace), ElementName = "workspace", IsNullable = false, Form = XmlSchemaForm.Qualified, Namespace = Declarations.SchemaVersion)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DataMember(Name = "workspace")]
        public Workspace __workspace;

        [JsonIgnore]
        [XmlIgnore]
        [IgnoreDataMember]
        public Workspace Workspace
        {
            get { return __workspace; }
            set { __workspace = value; }
        }

        public string ToJson(ISerialize decorator)
        {
            return decorator.Serialize(this);
        }
    }
}
