using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Ajax.Newsletter;
using DowJones.Assemblers.Workspaces;
using DowJones.Exceptions;
using DowJones.Globalization;
using Factiva.Gateway.Messages.Assets.Common.V2_0;
using Factiva.Gateway.Messages.Assets.Workspaces.V2_0;

namespace DowJones.Assemblers.Newsletters
{
    public class NewsletterRequestConversionManager : INewsletterRequestConversionManager
    {
        private readonly IResourceTextManager _resourceTextManager;
        private readonly IWorkspaceRequestConversionManager _workspaceRequestConversionManager;
        private int _noOfHeadlines = 0;

        public NewsletterRequestConversionManager(IResourceTextManager resourceTextManager, IWorkspaceRequestConversionManager workspaceRequestConversionManager)
        {
            _resourceTextManager = resourceTextManager;
            _workspaceRequestConversionManager = workspaceRequestConversionManager;
        }
        public UpdateWorkspaceRequest GetUpdateWorkspaceRequest(ManualWorkspace newsletterContent, WorkspaceRequestDto newsletterRequestDto, int maxHeadlinesInNewsletter)
        {
            if (newsletterRequestDto == null || newsletterContent == null)
            {
                throw new DowJonesUtilitiesException();
            }
            if (newsletterRequestDto.ContentItemsToAdd.Count <= 0)
            {
                throw new DowJonesUtilitiesException();
            }
           
            //1st element is the index of section and the 2nd element is the index of the sub-section
            string[] itemIndex = newsletterRequestDto.SectionSubsectionIndex.Split('_');
            int secIndex = Convert.ToInt32(itemIndex[0]);

            SectionCollection section = newsletterContent.SectionCollection;
            
            //Check if selected accession number already exists in the newsletter
            var alreadyExists = CheckIfItemAlreadyExists(section, newsletterRequestDto);
            if (alreadyExists)
            {
                throw new DowJonesUtilitiesException(_resourceTextManager.GetString("itemAlreadyExists"));    
            }

            //Check if newsletter already has 200 items
            if (_noOfHeadlines >= maxHeadlinesInNewsletter)
            {
                throw new DowJonesUtilitiesException(_resourceTextManager.GetString("alreadyHaveMaxAllowed"));   
            }

            //Check if more than 200 articles are being added to the existing newsletter
            if (newsletterRequestDto.ContentItemsToAdd.Count >= maxHeadlinesInNewsletter)
            {
                throw new DowJonesUtilitiesException(_resourceTextManager.GetString("selectMoreThanAllowed"));
            }

            //Check if sum of existing items and items to be added exceeds 100
            var finalCount = _noOfHeadlines + newsletterRequestDto.ContentItemsToAdd.Count;
            if (finalCount > maxHeadlinesInNewsletter)
            {
                var errorMessage = string.Format("{0} {1} {2}", _resourceTextManager.GetString("newsletterMaxHeadlines-1a"), finalCount,
                    _resourceTextManager.GetString("newsletterMaxHeadlines-2"));  //TODO
                throw new DowJonesUtilitiesException(errorMessage);
            }

            //If section doesn't exist at the chosen index, insert in the last section
            if (secIndex >= section.Count && section.Count > 0)
            {
                secIndex = section.Count - 1;
            }

            //To insert in the sub-section
            if (itemIndex.Length > 1)
            {
                int subSecIndex = Convert.ToInt32(itemIndex[1]);
                section[secIndex].SubSectionCollection[subSecIndex].ItemCollection = InsertAccessionNumbersAnteOrPost(section[secIndex].SubSectionCollection[subSecIndex].ItemCollection, newsletterRequestDto);
                SetPosition(section[secIndex].SubSectionCollection[subSecIndex].ItemCollection, newsletterRequestDto.PositionIndicator);
            }

            //To insert in the section
            else
            {
                section[secIndex].ItemCollection = InsertAccessionNumbersAnteOrPost(section[secIndex].ItemCollection, newsletterRequestDto);
                SetPosition(section[secIndex].ItemCollection, newsletterRequestDto.PositionIndicator);
            }
                
            var updateWorkspaceRequest = new UpdateWorkspaceRequest
            {
                Workspace = newsletterContent,
                Mode = UpdateWorkspaceMode.Save,
            };
            return updateWorkspaceRequest;
        }

        private bool CheckIfItemAlreadyExists(IEnumerable<Section> sections, WorkspaceRequestDto newsletterRequestDto)
        {
            var existingAns = new List<string>();
            foreach (var section in sections)
            {
                GetExistingAccessionNumbersList(section.ItemCollection, existingAns);
                foreach (var subSection in section.SubSectionCollection)
                {
                    GetExistingAccessionNumbersList(subSection.ItemCollection, existingAns);
                }
            }
            for (var i=0; i< newsletterRequestDto.ContentItemsToAdd.Count; i++)
            {
                var item = newsletterRequestDto.ContentItemsToAdd.ElementAt(i);
                if (existingAns.Contains(item.Key))
                {
                    return true;
                }
            }
            return false;
        }

        private void GetExistingAccessionNumbersList(ItemCollection itemCollection, List<string> existingAns)
        {
            existingAns.AddRange(itemCollection.OfType<ArticleItem>().Select(articleItem => articleItem.AccessionNumber));
            _noOfHeadlines = _noOfHeadlines + itemCollection.Count;
        }

        private ItemCollection InsertAccessionNumbersAnteOrPost(ItemCollection itemCollection, WorkspaceRequestDto newsletterRequestDto)
        {
            int existingContentItemsLength = itemCollection != null ? itemCollection.Count : 0;

            var newContentItemsList = new ItemCollection();  //merged list

            IEnumerable<Item> anContentItemsList = PopulateContentItemsWithAccessionNumbers(newsletterRequestDto);

            if (existingContentItemsLength == 0)
            {
                newContentItemsList.InsertRange(0, anContentItemsList);
            }
            else
            {
                if (itemCollection != null)
                {
                    newContentItemsList.InsertRange(0, itemCollection); //Copy existing items
                }
                if (newsletterRequestDto.PositionIndicator == "a")   //Insert accession numbers at the beginning 
                {
                    newContentItemsList.InsertRange(0, anContentItemsList);
                }
                else  //Insert accession numbers at the end
                {
                    newContentItemsList.AddRange(anContentItemsList);
                }
            }
            return newContentItemsList;
        }

        private IEnumerable<Item> PopulateContentItemsWithAccessionNumbers(WorkspaceRequestDto newsletterRequestDto)
        {

            //TODO check if the an is Chart Image item/Chart item/link item

            var nuNewsletterContentItemList = new List<ArticleItem>();

            for (int i = 0; i < newsletterRequestDto.ContentItemsToAdd.Count; i++)
            {
                var item = newsletterRequestDto.ContentItemsToAdd.ElementAt(i);
                
                var nuNewsletterContentItem = new ArticleItem
                {
                    AccessionNumber = item.Key,
                    ContentCategory = _workspaceRequestConversionManager.MapContentCategory(item.Value)
                };
                nuNewsletterContentItemList.Add(nuNewsletterContentItem);
            }

            return nuNewsletterContentItemList;
        }

        private void SetPosition(ItemCollection itemCollection, string antiOrPostIndicator)
        {
            //If new AN is being inserted in the beginning, reset the position of all the items in that section/sub-section
            if (antiOrPostIndicator == "a")
            {
                var position = 0;
                foreach (var item in itemCollection)
                {
                    item.Position = position++;
                }
            }
            //If new AN is being inserted at the end, just set the position of the new item
            else
            {
                itemCollection[itemCollection.Count - 1].Position = itemCollection.Count;
            }
        }
        
    }
}
