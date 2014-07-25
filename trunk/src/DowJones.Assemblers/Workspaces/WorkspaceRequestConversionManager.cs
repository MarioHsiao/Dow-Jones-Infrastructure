using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public AddItemsToWorkspaceRequest AddWorkspaceItem(AutomaticWorkspace workspaceContent, WorkspaceRequestDto workspaceRequestDto)
        {
            if (workspaceRequestDto.ContentItemsToAdd.Count == 0)
                throw new DowJonesUtilitiesException(_resourceTextManager.GetString("noAccessionNumbers"));  //TODO add in translate db 
            
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
                    throw new DowJonesUtilitiesException(_resourceTextManager.GetString("itemAlreadyExists"));  
                }
            }

            //Check if workspace already has 100 items
            if (workspaceContent.ItemsCollection.Count >= Properties.Settings.Default.MaxHeadlinesInWorkspace) 
            {
                throw new DowJonesUtilitiesException(_resourceTextManager.GetString("alreadyHaveMaxAllowed"));  
            }

            //Check if more than 100 articles are being added to the existing workspace
            if (contentItemCollection.Count > Properties.Settings.Default.MaxHeadlinesInWorkspace)  
            {
                throw new DowJonesUtilitiesException(_resourceTextManager.GetString("selectMoreThanAllowed"));
            }

            //Check if sum of existing items and items to be added exceeds 100
            var finalCount = contentItemCollection.Count + workspaceContent.ItemsCollection.Count;
            if (finalCount > Properties.Settings.Default.MaxHeadlinesInWorkspace)
            {
                var errorMessage = string.Format("{0} {1} {2}", _resourceTextManager.GetString("newsletterMaxHeadlines-1a"), finalCount,
                    _resourceTextManager.GetString("newsletterMaxHeadlines-2"));
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

        private bool isItemExistInCollection(string an, IEnumerable<ContentItem> itemCollection)
        {
            return itemCollection.Cast<ArticleItem>().Any(item => item.AccessionNumber.Equals(an));
        }

        private ContentCategory MapContentCategory(
            DowJones.Ajax.ContentCategory searchContentCategory)
        {
            switch (searchContentCategory)
            {
                case DowJones.Ajax.ContentCategory.Publication:
                    return ContentCategory.Publications;
                case DowJones.Ajax.ContentCategory.Website:
                    return ContentCategory.WebSites;
                case DowJones.Ajax.ContentCategory.Picture:
                    return ContentCategory.Pictures;
                case DowJones.Ajax.ContentCategory.Multimedia:
                    return ContentCategory.Multimedia;
                //TODO case DowJones.Ajax.ContentCategory.External: 
                case DowJones.Ajax.ContentCategory.Blog:
                    return ContentCategory.Blogs;
                case DowJones.Ajax.ContentCategory.Board:
                    return ContentCategory.Boards;
                //TODO case DowJones.Ajax.ContentCategory.CustomerDoc:
                case DowJones.Ajax.ContentCategory.Internal:
                    return ContentCategory.CustomerDoc;
            }
            return ContentCategory.Unspecified;
        }
    }

    public class WorkspaceRequestDto
    {
        public Dictionary<string, DowJones.Ajax.ContentCategory> ContentItemsToAdd { get; set; }
    }
   
}
