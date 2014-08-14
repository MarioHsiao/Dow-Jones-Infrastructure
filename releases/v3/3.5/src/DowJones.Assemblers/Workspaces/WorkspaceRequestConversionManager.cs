using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using DowJones.Ajax.Newsletter;
using DowJones.Exceptions;
using DowJones.Globalization;
using DowJones.Mapping;
using Factiva.Gateway.Messages.Assets.Workspaces.V2_0;
using Factiva.Gateway.Messages.Assets.Common.V2_0;


namespace DowJones.Assemblers.Workspaces
{
    public class WorkspaceRequestConversionManager: IWorkspaceRequestConversionManager
    {
        private readonly IResourceTextManager _resourceTextManager;

        public WorkspaceRequestConversionManager(IResourceTextManager resourceTextManager)
        {
            _resourceTextManager = resourceTextManager;
        }
        public AddItemsToWorkspaceRequest AddWorkspaceItem(AutomaticWorkspace workspaceContent, WorkspaceRequestDto workspaceRequestDto, int maxHeadlinesInWorkspace = 100)
        {
            var errorMessage = "";
            if (workspaceRequestDto == null || workspaceRequestDto.ContentItemsToAdd == null || workspaceRequestDto.ContentItemsToAdd.Count == 0)
            {
                throw new DowJonesUtilitiesException(_resourceTextManager.GetString("noAccessionNumbers")); 
            }

            //Check if more than 100 articles are being added to the existing workspace
            if (workspaceRequestDto.ContentItemsToAdd.Count > maxHeadlinesInWorkspace)
            {
                errorMessage = string.Format("{0} {1} {2} {3}{4}", _resourceTextManager.GetString("selectMoreThanAllowed-1a"), maxHeadlinesInWorkspace,
                    _resourceTextManager.GetString("selectMoreThanAllowed-1b"), maxHeadlinesInWorkspace, _resourceTextManager.GetString("period"));
                throw new DowJonesUtilitiesException(errorMessage);
            }

            var contentItemCollection = new ContentItemCollection();

            for (var i = 0; i < workspaceRequestDto.ContentItemsToAdd.Count; i++)
            {
                var item = workspaceRequestDto.ContentItemsToAdd.ElementAt(i);
                if (!isItemExistInCollection(item.Key, workspaceContent.ItemsCollection))
                {
                    var articleItem = new ArticleItem
                    {
                        AccessionNumber = item.Key,
                        ContentCategory = MapContentCategory(item.Value)
                    };
                    contentItemCollection.Add(articleItem);
                }
                else
                {
                    throw new DowJonesUtilitiesException(_resourceTextManager.GetString("articleAlreadyExists"));  
                }
            }

            //Check if workspace already has 100 items
            if (workspaceContent.ItemsCollection.Count >= maxHeadlinesInWorkspace) 
            {
                errorMessage = string.Format("{0} {1} {2}", _resourceTextManager.GetString("alreadyHaveMaxAllowed-1a"), maxHeadlinesInWorkspace,
                    _resourceTextManager.GetString("alreadyHaveMaxAllowed-1b"));
                throw new DowJonesUtilitiesException(errorMessage);  
            }

            //Check if sum of existing items and items to be added exceeds 100
            var finalCount = contentItemCollection.Count + workspaceContent.ItemsCollection.Count;
            if (finalCount > maxHeadlinesInWorkspace)
            {
                errorMessage = string.Format("{0} {1} {2} {3} {4}", _resourceTextManager.GetString("newsletterMaxHeadlines-1a"), finalCount,
                    _resourceTextManager.GetString("newsletterMaxHeadlines-2a"), maxHeadlinesInWorkspace, _resourceTextManager.GetString("newsletterMaxHeadlines-2b"));
                throw new DowJonesUtilitiesException(errorMessage); 
            }

            var addItemsToAutomaticWorkspaceDto = new AddItemsToAutomaticWorkspaceDTO();
            var request = new AddItemsToWorkspaceRequest();
            if (contentItemCollection.Count > 0)
            {
                addItemsToAutomaticWorkspaceDto.Id = workspaceContent.Id;
                addItemsToAutomaticWorkspaceDto.ItemCollection = contentItemCollection;
                //addItemsToAutomaticWorkspaceDto.AddAtPositionMode = Factiva.Gateway.Messages.Assets.Workspaces.V2_0.AddAtPositionMode.StartOfList;  //TODO check if this is required
                request.DataTransferObject = addItemsToAutomaticWorkspaceDto;
            }
            return request;
        }

        public Factiva.Gateway.Messages.Assets.Common.V2_0.ContentCategory MapContentCategory(
            DowJones.Ajax.ContentCategory searchContentCategory)
        {
            switch (searchContentCategory)
            {
                case DowJones.Ajax.ContentCategory.Publication:
                    return ContentCategory.Publications;
                case DowJones.Ajax.ContentCategory.External: 
                case DowJones.Ajax.ContentCategory.Website:
                    return ContentCategory.WebSites;
                case DowJones.Ajax.ContentCategory.Picture:
                    return ContentCategory.Pictures;
                case DowJones.Ajax.ContentCategory.Multimedia:
                    return ContentCategory.Multimedia;
                case DowJones.Ajax.ContentCategory.Blog:
                    return ContentCategory.Blogs;
                case DowJones.Ajax.ContentCategory.Board:
                    return ContentCategory.Boards;
                case DowJones.Ajax.ContentCategory.CustomerDoc:
                case DowJones.Ajax.ContentCategory.Internal:
                    return ContentCategory.CustomerDoc;
            }
            return ContentCategory.Unspecified;
        }

        private bool isItemExistInCollection(string an, IEnumerable<ContentItem> itemCollection)
        {
            return itemCollection.Cast<ArticleItem>().Any(item => item.AccessionNumber.Equals(an));
        }
    }

    public class WorkspaceRequestDto
    {
        public Dictionary<string, DowJones.Ajax.ContentCategory> ContentItemsToAdd { get; set; }
        public string PositionIndicator { get; set; }  //For adding items to newsletter
        public string SectionSubsectionIndex { get; set; }  //For adding items to newsletter
    }
}
