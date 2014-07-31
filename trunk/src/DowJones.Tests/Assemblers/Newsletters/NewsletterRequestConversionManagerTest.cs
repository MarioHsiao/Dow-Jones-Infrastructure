using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Assemblers.Workspaces;
using DowJones.Exceptions;
using DowJones.Globalization;
using Factiva.Gateway.Messages.Assets.Common.V2_0;
using Factiva.Gateway.Messages.Assets.Workspaces.V2_0;
using Factiva.Gateway.Messages.FCE.Assets.V1_0;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ContentCategory = DowJones.Ajax.ContentCategory;
using ItemCollection = Factiva.Gateway.Messages.Assets.Common.V2_0.ItemCollection;

namespace DowJones.Assemblers.Newsletters
{
    [TestClass]
    public class NewsletterRequestConversionManagerTest
    {
        private readonly Mock<IResourceTextManager> _resourceTextManagerMock = new Mock<IResourceTextManager>();
        private readonly Mock<IWorkspaceRequestConversionManager> _workspaceRequestConversionManagerMock = new Mock<IWorkspaceRequestConversionManager>();
        private int _maxHeadlinesInNewsletter = 200;

        protected NewsletterRequestConversionManager ConversionManager
        {
            get { return new NewsletterRequestConversionManager(_resourceTextManagerMock.Object, _workspaceRequestConversionManagerMock.Object); }
        }

        [TestInitialize]
        public void TestFixtureSetUp()
        {
            _resourceTextManagerMock.Setup(t => t.GetString(It.IsAny<string>())).Returns((string name) => "{" + name + "}");
        }

        [TestMethod]
        public void NlReqConvMgrShouldThrowExceptionIfRequestDoesNotHaveAccessionNumber()
        {
            try
            {
                var response = ConversionManager.GetUpdateWorkspaceRequest(new ManualWorkspace(), new WorkspaceRequestDto());
                Assert.Fail("Should throw exception");
            }
            catch (DowJonesUtilitiesException ex)
            {
                Assert.AreEqual(ex.Message, "{noAccessionNumbers}", "because no accession number is given in the request");
            }
        }

        [TestMethod]
        public void NlReqConvMgrShouldThrowExceptionIfWorkspaceAlreadyHasMaxHeadlines()
        {
            _maxHeadlinesInNewsletter = 1;
            try
            {
                var response = ConversionManager.GetUpdateWorkspaceRequest(getManualWorkspace("an"), getWorkspaceRequestDto("an_1"), _maxHeadlinesInNewsletter);
                Assert.Fail("Should throw exception");
            }
            catch (DowJonesUtilitiesException ex)
            {
                var errorMessage = string.Format("{0} {1} {2}", "{alreadyHaveMaxAllowed-1a}", _maxHeadlinesInNewsletter,
                    "{alreadyHaveMaxAllowed-1b}");
                Assert.AreEqual(ex.Message, errorMessage, "Cannot insert more than the maximum allowed");
            }
        }

        [TestMethod]
        public void NlReqConvMgrShouldThrowExceptionIfAccessionNumberAlreadyExists()
        {
            try
            {
                var response = ConversionManager.GetUpdateWorkspaceRequest(getManualWorkspace("an"), getWorkspaceRequestDto("an"));
                Assert.Fail("Should throw exception");
            }
            catch (DowJonesUtilitiesException ex)
            {
                Assert.AreEqual(ex.Message, "{articleAlreadyExists}", "because accession number already exists in the workspace");
            }
        }

        [TestMethod]
        public void NlReqConvMgrShouldThrowExceptionIfSelectedArticlesIsGreaterThanMaxHeadlines()
        {
            _maxHeadlinesInNewsletter = 1;
            try
            {
                var response = ConversionManager.GetUpdateWorkspaceRequest(getManualWorkspace("none"), getWorkspaceRequestDto("array"), 1);
                Assert.Fail("Should throw exception");
            }
            catch (DowJonesUtilitiesException ex)
            {
                var errorMessage = string.Format("{0} {1} {2} {3}{4}", "{selectMoreThanAllowed-1a}", _maxHeadlinesInNewsletter,
                    "{selectMoreThanAllowed-1b}", _maxHeadlinesInNewsletter, "{period}");
                Assert.AreEqual(ex.Message, errorMessage, "Cannot select articles more than the max headlines");
            }
        }

        [TestMethod]
        public void NlReqConvMgrShouldThrowExceptionIfSumOfSelectedAndExistingArticlesIsGreaterThanMaxHeadlines()
        {
            _maxHeadlinesInNewsletter = 3;
            var request = getWorkspaceRequestDto("array");
            var autoWorkspaceRequest = getManualWorkspace("an");
            try
            {
                var response = ConversionManager.GetUpdateWorkspaceRequest(autoWorkspaceRequest, request, _maxHeadlinesInNewsletter);
                Assert.Fail("Should throw exception");
            }
            catch (DowJonesUtilitiesException ex)
            {
                var errorMessage = string.Format("{0} {1} {2} {3} {4}", "{newsletterMaxHeadlines-1a}", request.ContentItemsToAdd.Count + 2,
                    "{newsletterMaxHeadlines-2a}", _maxHeadlinesInNewsletter, "{newsletterMaxHeadlines-2b}");
                Assert.AreEqual(ex.Message, errorMessage, "Sum of selected and existing cannot exceed max headlines");
            }
        }

        [TestMethod]
        public void NlReqConvMgrShouldCreateSectionAndInsertArticlesIfNewsletterIsEmpty()
        {
            var request = getWorkspaceRequestDto("array");
            var autoWorkspaceRequest = getManualWorkspace("none");
            try
            {
                var updateWorkspaceRequest = ConversionManager.GetUpdateWorkspaceRequest(autoWorkspaceRequest, request, _maxHeadlinesInNewsletter);
                var manualWorkspace = ((ManualWorkspace)(updateWorkspaceRequest.Workspace));
                Assert.AreEqual(manualWorkspace.SectionCollection[0].ItemCollection.Count, request.ContentItemsToAdd.Count, "Since articles are added, total count is the sum of existing and the selected articles");
            }
            catch (DowJonesUtilitiesException)
            {
                Assert.Fail("Should not throw exception");
            }
        }

        [TestMethod]
        public void NlReqConvMgrShouldCreateSectionAndInsertArticlesEvenIfSecSubSecIndexIsEmptyAndSectionDoesnotExists()
        {
            var request = getWorkspaceRequestDto("array");
            var autoWorkspaceRequest = getManualWorkspace("none");
            try
            {
                var updateWorkspaceRequest = ConversionManager.GetUpdateWorkspaceRequest(autoWorkspaceRequest, request, _maxHeadlinesInNewsletter);
                var manualWorkspace = ((ManualWorkspace)(updateWorkspaceRequest.Workspace));
                Assert.AreEqual(manualWorkspace.SectionCollection[0].ItemCollection.Count, request.ContentItemsToAdd.Count, "Create section is newsletter is empty and insert articles");
            }
            catch (DowJonesUtilitiesException)
            {
                Assert.Fail("Should not throw exception");
            }
        }

        [TestMethod]
        public void NlReqConvMgrShouldInsertArticlesInTheLastSectionEvenIfSecSubSecIndexIsEmpty()
        {
            var request = getWorkspaceRequestDto("array");
            var autoWorkspaceRequest = getManualWorkspace("an");
            try
            {
                var updateWorkspaceRequest = ConversionManager.GetUpdateWorkspaceRequest(autoWorkspaceRequest, request, _maxHeadlinesInNewsletter);
                var manualWorkspace = ((ManualWorkspace)(updateWorkspaceRequest.Workspace));
                Assert.AreEqual(manualWorkspace.SectionCollection[manualWorkspace.SectionCollection.Count-1].ItemCollection.Count, request.ContentItemsToAdd.Count, "Create section is newsletter is empty and insert articles");
            }
            catch (DowJonesUtilitiesException)
            {
                Assert.Fail("Should not throw exception");
            }
        }

        [TestMethod]
        public void NlReqConvMgrShouldInsertArticlesInTheSectionForTheGivenSecIndex()
        {
            var request = getWorkspaceRequestDto("array");
            request.SectionSubsectionIndex = "1";
            request.PositionIndicator = "a";
            var autoWorkspaceRequest = getManualWorkspace("an");
            try
            {
                var updateWorkspaceRequest = ConversionManager.GetUpdateWorkspaceRequest(autoWorkspaceRequest, request, _maxHeadlinesInNewsletter);
                var manualWorkspace = ((ManualWorkspace)(updateWorkspaceRequest.Workspace));
                Assert.AreEqual(manualWorkspace.SectionCollection[1].ItemCollection.Count, request.ContentItemsToAdd.Count, "Insert articles in the 2nd section");
                //var articleItem = (ArticleItem)(manualWorkspace.SectionCollection[1].ItemCollection[0]);
                //Assert.AreEqual(articleItem.AccessionNumber, "an", "Since positionIndicator is a, existing items will remain on the top");
                Assert.AreEqual(manualWorkspace.SectionCollection[1].ItemCollection[manualWorkspace.SectionCollection[1].ItemCollection.Count-1].Position, 2, "Since position of the items in that section should be 0, 1 and 2");
            }
            catch (DowJonesUtilitiesException)
            {
                Assert.Fail("Should not throw exception");
            }
        }

        [TestMethod]
        public void NlReqConvMgrShouldInsertArticlesInTheSectionForTheGivenSecIndexAndResetPositions()
        {
            var request = getWorkspaceRequestDto("array");
            request.SectionSubsectionIndex = "0";
            request.PositionIndicator = "a";
            var autoWorkspaceRequest = getManualWorkspace("an");
            try
            {
                var updateWorkspaceRequest = ConversionManager.GetUpdateWorkspaceRequest(autoWorkspaceRequest, request, _maxHeadlinesInNewsletter);
                var manualWorkspace = ((ManualWorkspace)(updateWorkspaceRequest.Workspace));
                Assert.AreEqual(manualWorkspace.SectionCollection[0].ItemCollection.Count, request.ContentItemsToAdd.Count+1, "Insert articles in the 1st section");
                var articleItem = (ArticleItem)(manualWorkspace.SectionCollection[0].ItemCollection[manualWorkspace.SectionCollection[0].ItemCollection.Count - 1]);
                Assert.AreEqual(articleItem.AccessionNumber, "an", "Since positionIndicator is a, existing items will be pushed to the end");
                Assert.AreEqual(manualWorkspace.SectionCollection[0].ItemCollection[manualWorkspace.SectionCollection[0].ItemCollection.Count - 1].Position, 3, "Since position of the items in that section should be 0, 1, 2 and 3");
            }
            catch (DowJonesUtilitiesException)
            {
                Assert.Fail("Should not throw exception");
            }
        }

        [TestMethod]
        public void NlReqConvMgrShouldInsertArticlesInTheLastSectionIfSecIndexIsOutOfRange()
        {
            var request = getWorkspaceRequestDto("array");
            request.SectionSubsectionIndex = "2";
            var autoWorkspaceRequest = getManualWorkspace("an");
            try
            {
                var updateWorkspaceRequest = ConversionManager.GetUpdateWorkspaceRequest(autoWorkspaceRequest, request, _maxHeadlinesInNewsletter);
                var manualWorkspace = ((ManualWorkspace)(updateWorkspaceRequest.Workspace));
                Assert.AreEqual(manualWorkspace.SectionCollection[manualWorkspace.SectionCollection.Count-1].ItemCollection.Count, request.ContentItemsToAdd.Count, "Insert articles in the 2nd section");
            }
            catch (DowJonesUtilitiesException)
            {
                Assert.Fail("Should not throw exception");
            }
        }

        [TestMethod]
        public void NlReqConvMgrShouldInsertArticlesInTheSubSectionForTheGivenSecSubSecIndex()
        {
            var request = getWorkspaceRequestDto("array");
            request.SectionSubsectionIndex = "1_0";
            request.PositionIndicator = "p";
            var autoWorkspaceRequest = getManualWorkspace("an");
            try
            {
                var updateWorkspaceRequest = ConversionManager.GetUpdateWorkspaceRequest(autoWorkspaceRequest, request, _maxHeadlinesInNewsletter);
                var manualWorkspace = ((ManualWorkspace)(updateWorkspaceRequest.Workspace));
                Assert.AreEqual(manualWorkspace.SectionCollection[1].SubSectionCollection[0].ItemCollection.Count, request.ContentItemsToAdd.Count + 1 , "Insert articles in the sub-section of the 2nd section");
                var articleItem = (ArticleItem) manualWorkspace.SectionCollection[1].SubSectionCollection[0].ItemCollection[0];
                Assert.AreEqual(articleItem.AccessionNumber, "an2", "Since positionIndicator is p, existing items will remain on the top");
                Assert.AreEqual(manualWorkspace.SectionCollection[1].SubSectionCollection[0].ItemCollection[manualWorkspace.SectionCollection[1].SubSectionCollection[0].ItemCollection.Count - 1].Position, 4, "Since position of the items in that sub-section should be 0 and 1");
            }
            catch (DowJonesUtilitiesException)
            {
                Assert.Fail("Should not throw exception");
            }
        }

        [TestMethod]
        public void NlReqConvMgrShouldInsertArticlesInTheSectionIfSubSecIndexIsOutOfRangeAndNoSubSectionExists()
        {
            var request = getWorkspaceRequestDto("array");
            request.SectionSubsectionIndex = "0_1";
            var autoWorkspaceRequest = getManualWorkspace("an");
            try
            {
                var updateWorkspaceRequest = ConversionManager.GetUpdateWorkspaceRequest(autoWorkspaceRequest, request, _maxHeadlinesInNewsletter);
                var manualWorkspace = ((ManualWorkspace)(updateWorkspaceRequest.Workspace));
                Assert.AreEqual(manualWorkspace.SectionCollection[0].ItemCollection.Count, request.ContentItemsToAdd.Count + 1, "Insert articles in the sub-section of the 2nd section");
            }
            catch (DowJonesUtilitiesException)
            {
                Assert.Fail("Should not throw exception");
            }
        }

        [TestMethod]
        public void NlReqConvMgrShouldInsertArticlesInTheLastSubSectionIfSubSecIndexIsOutOfRange()
        {
            var request = getWorkspaceRequestDto("array");
            request.SectionSubsectionIndex = "1_1";
            var autoWorkspaceRequest = getManualWorkspace("an");
            try
            {
                var updateWorkspaceRequest = ConversionManager.GetUpdateWorkspaceRequest(autoWorkspaceRequest, request, _maxHeadlinesInNewsletter);
                var manualWorkspace = ((ManualWorkspace)(updateWorkspaceRequest.Workspace));
                Assert.AreEqual(manualWorkspace.SectionCollection[1].SubSectionCollection[manualWorkspace.SectionCollection[1].SubSectionCollection.Count-1].ItemCollection.Count, request.ContentItemsToAdd.Count + 1, "Insert articles in the sub-section of the 2nd section");
            }
            catch (DowJonesUtilitiesException)
            {
                Assert.Fail("Should not throw exception");
            }
        }

        #region StubData

        private WorkspaceRequestDto getWorkspaceRequestDto(string an)
        {
            if (an == "array")
            {
                return new WorkspaceRequestDto
                {
                    ContentItemsToAdd = new Dictionary<string, ContentCategory>
                    {
                        {an, ContentCategory.Publication},
                        {an+"2", ContentCategory.Website},
                        {an+"3", ContentCategory.External}
                    }
                };
            }
            return new WorkspaceRequestDto
            {
                ContentItemsToAdd = new Dictionary<string, ContentCategory>
                {
                    {an, ContentCategory.Publication}
                }
            };
        }

        private ManualWorkspace getManualWorkspace(string an)
        {
            if (an == "none")
            {
                return new ManualWorkspace();
            }
            return new ManualWorkspace
            {
                
                Id = 12,
                SectionCollection = new SectionCollection
                {
                    new Section
                    {
                        Name = "Section 1",
                        ItemCollection = new ItemCollection
                        {
                            new ArticleItem
                            {
                                AccessionNumber = an
                            }
                        }
                    },
                    new Section
                    {
                        Name = "Section 2",
                        SubSectionCollection = new SubSectionCollection
                        {
                            new SubSection
                            {
                                Name = "Sub-Section 1",
                                ItemCollection = new ItemCollection
                                {
                                    new ArticleItem
                                    {
                                        AccessionNumber = an+"2"
                                    }
                                }
                            }
                        }
                    }
                    
                }
                
            };
        }
        #endregion
    }
}
