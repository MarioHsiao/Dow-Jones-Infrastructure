using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Ajax.Newsletter;
using DowJones.Assemblers.Workspaces;
using DowJones.Exceptions;
using DowJones.Extensions;
using DowJones.Globalization;
using Factiva.Gateway.Messages.Assets.Common.V2_0;
using Factiva.Gateway.Messages.Assets.Workspaces.V2_0;

namespace DowJones.Assemblers.Newsletters
{
    public class NewsletterRequestConversionManager : INewsletterRequestConversionManager
    {
        private readonly IResourceTextManager _resourceTextManager;
        private readonly IWorkspaceRequestConversionManager _workspaceRequestConversionManager;
        private int _existingHeadlinesCount = 0;
        
        public NewsletterRequestConversionManager(IResourceTextManager resourceTextManager, IWorkspaceRequestConversionManager workspaceRequestConversionManager)
        {
            _resourceTextManager = resourceTextManager;
            _workspaceRequestConversionManager = workspaceRequestConversionManager;
        }
        public UpdateWorkspaceRequest GetUpdateWorkspaceRequest(ManualWorkspace newsletterContent, WorkspaceRequestDto newsletterRequestDto, int maxHeadlinesInNewsletter = 200)
        {
            var errorMessage = "";
            if (newsletterRequestDto == null || newsletterRequestDto.ContentItemsToAdd == null || newsletterRequestDto.ContentItemsToAdd.Count == 0)
            {
                throw new DowJonesUtilitiesException(_resourceTextManager.GetString("noAccessionNumbers"));
            }

            //Check if more than 200 articles are being added to the existing newsletter
            if (newsletterRequestDto.ContentItemsToAdd.Count > maxHeadlinesInNewsletter)
            {
                errorMessage = string.Format("{0} {1} {2} {3}{4}", _resourceTextManager.GetString("selectMoreThanAllowed-1a"), maxHeadlinesInNewsletter,
                    _resourceTextManager.GetString("selectMoreThanAllowed-1b"), maxHeadlinesInNewsletter, _resourceTextManager.GetString("period"));
                throw new DowJonesUtilitiesException(errorMessage);
            }

            SectionCollection sections = newsletterContent.SectionCollection;

            //Set _existingHeadlinesCount in the newsletter and Check if selected accession number already exists in the newsletter
            if (sections.Count > 0)
            {
                var alreadyExists = CheckIfItemAlreadyExists(sections, newsletterRequestDto);
                if (alreadyExists)
                {
                    throw new DowJonesUtilitiesException(_resourceTextManager.GetString("articleAlreadyExists"));
                }
            }

            //Check if newsletter already has 200 items
            if (_existingHeadlinesCount >= maxHeadlinesInNewsletter)
            {
                errorMessage = string.Format("{0} {1} {2}", _resourceTextManager.GetString("alreadyHaveMaxAllowed-1a"), maxHeadlinesInNewsletter,
                    _resourceTextManager.GetString("alreadyHaveMaxAllowed-1b"));
                throw new DowJonesUtilitiesException(errorMessage);
            }

            //Check if sum of existing items and items to be added exceeds 200
            var finalCount = _existingHeadlinesCount + newsletterRequestDto.ContentItemsToAdd.Count;
            if (finalCount > maxHeadlinesInNewsletter)
            {
                errorMessage = string.Format("{0} {1} {2} {3} {4}", _resourceTextManager.GetString("newsletterMaxHeadlines-1a"), finalCount,
                    _resourceTextManager.GetString("newsletterMaxHeadlines-2a"), maxHeadlinesInNewsletter, _resourceTextManager.GetString("newsletterMaxHeadlines-2b"));
                throw new DowJonesUtilitiesException(errorMessage);
            }

            //UI will pass empty SectionSubsectionIndex where there are no sections in the newsletter
            if (newsletterRequestDto.SectionSubsectionIndex.IsNullOrEmpty())
            {
                //If there are no sections, create a section and insert an article
                if (sections.Count == 0)
                {
                    sections.Add(new Section());
                    AddToSection(sections[0], newsletterRequestDto);
                }
                //Else, insert in the top section
                else
                {
                    AddToSection(sections[sections.Count - 1], newsletterRequestDto);
                }
            }
            else
            {
                //1st element is the index of section and the 2nd element is the index of the sub-section
                string[] itemIndex = newsletterRequestDto.SectionSubsectionIndex.Split('_');
                int secIndex = Convert.ToInt32(itemIndex[0]);

                //If chosen section is out of range, insert in the last section.  make sure sections[secIndex] is not null
                if (secIndex >= sections.Count && sections.Count > 0)
                {
                    secIndex = sections.Count - 1;
                }

                //To insert in the sub-section
                if (itemIndex.Length > 1)
                {
                    int subSecIndex = Convert.ToInt32(itemIndex[1]);

                    //If chosen subsection is out of range, 
                    if (subSecIndex >= sections[secIndex].SubSectionCollection.Count)
                    {
                        //insert in the last sub-section of that section (if exists)
                        if (sections[secIndex].SubSectionCollection.Count > 0)
                        {
                            subSecIndex = sections[secIndex].SubSectionCollection.Count - 1;
                            AddToSubSection(sections[secIndex].SubSectionCollection[subSecIndex], newsletterRequestDto);
                        }
                        //If there are no sub-sections, insert in the section
                        else
                        {
                            AddToSection(sections[secIndex], newsletterRequestDto);
                        }
                    }
                    else
                    {
                        AddToSubSection(sections[secIndex].SubSectionCollection[subSecIndex], newsletterRequestDto);
                    }
                }
                //To insert in the section
                else
                {
                    AddToSection(sections[secIndex], newsletterRequestDto);
                }
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
            _existingHeadlinesCount = _existingHeadlinesCount + itemCollection.Count;
        }

        private void AddToSection(Section section, WorkspaceRequestDto newsletterRequestDto)
        {
            section.ItemCollection = InsertAccessionNumbersAnteOrPost(section.ItemCollection, newsletterRequestDto);
            SetPosition(section.ItemCollection, newsletterRequestDto.PositionIndicator);
        }

        private void AddToSubSection(SubSection subSection, WorkspaceRequestDto newsletterRequestDto)
        {
            subSection.ItemCollection = InsertAccessionNumbersAnteOrPost(subSection.ItemCollection, newsletterRequestDto);
            SetPosition(subSection.ItemCollection, newsletterRequestDto.PositionIndicator);
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
