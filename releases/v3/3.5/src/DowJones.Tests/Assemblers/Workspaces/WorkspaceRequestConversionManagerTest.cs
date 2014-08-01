using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Exceptions;
using DowJones.Globalization;
using Factiva.Gateway.Messages.Assets.Common.V2_0;
using Factiva.Gateway.Messages.Assets.Workspaces.V2_0;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ContentCategory = DowJones.Ajax.ContentCategory;

namespace DowJones.Assemblers.Workspaces
{
    [TestClass]
    public class WorkspaceRequestConversionManagerTest
    {
        private readonly Mock<IResourceTextManager> _resourceTextManagerMock = new Mock<IResourceTextManager>();
        private int _maxHeadlinesInWorkspace = 100;

        protected WorkspaceRequestConversionManager ConversionManager
        {
            get { return new WorkspaceRequestConversionManager(_resourceTextManagerMock.Object); }
        }

        [TestInitialize]
        public void TestFixtureSetUp()
        {
            _resourceTextManagerMock.Setup(t => t.GetString(It.IsAny<string>())).Returns((string name) => "{" + name + "}");
        }

        [TestMethod]
        public void WorkspaceReqConvManagerShouldThrowExceptionIfRequestDoesNotHaveAccessionNumber()
        {
            try
            {
                var response = ConversionManager.AddWorkspaceItem(new AutomaticWorkspace(), new WorkspaceRequestDto());
                Assert.Fail("Should throw exception");
            }
            catch (DowJonesUtilitiesException ex)
            {
                Assert.AreEqual(ex.Message, "{noAccessionNumbers}", "because no accession number is given in the request");
            }
        }

        [TestMethod]
        public void WorkspaceReqConvManagerShouldThrowExceptionIfWorkspaceAlreadyHasMaxHeadlines()
        {
            _maxHeadlinesInWorkspace = 1;
            try
            {
                var response = ConversionManager.AddWorkspaceItem(getAutomaticWorkspace("an"), getWorkspaceRequestDto("an_1"), _maxHeadlinesInWorkspace);
                Assert.Fail("Should throw exception");
            }
            catch (DowJonesUtilitiesException ex)
            {
                var errorMessage = string.Format("{0} {1} {2}", "{alreadyHaveMaxAllowed-1a}", _maxHeadlinesInWorkspace,
                    "{alreadyHaveMaxAllowed-1b}");
                Assert.AreEqual(ex.Message, errorMessage, "Cannot insert more than the maximum allowed");
            }
        }

        [TestMethod]
        public void WorkspaceReqConvManagerShouldThrowExceptionIfAccessionNumberAlreadyExists()
        {
            try
            {
                var response = ConversionManager.AddWorkspaceItem(getAutomaticWorkspace("an"), getWorkspaceRequestDto("an"));
                Assert.Fail("Should throw exception");
            }
            catch (DowJonesUtilitiesException ex)
            {
                Assert.AreEqual(ex.Message, "{articleAlreadyExists}", "because accession number already exists in the workspace");
            }
        }
        
        [TestMethod]
        public void WorkspaceReqConvManagerShouldThrowExceptionIfSelectedArticlesIsGreaterThanMaxHeadlines()
        {
            _maxHeadlinesInWorkspace = 1;
            try
            {
                var response = ConversionManager.AddWorkspaceItem(getAutomaticWorkspace("none"), getWorkspaceRequestDto("array"), 1);
                Assert.Fail("Should throw exception");
            }
            catch (DowJonesUtilitiesException ex)
            {
                var errorMessage = string.Format("{0} {1} {2} {3}{4}", "{selectMoreThanAllowed-1a}", _maxHeadlinesInWorkspace,
                    "{selectMoreThanAllowed-1b}", _maxHeadlinesInWorkspace, "{period}");
                Assert.AreEqual(ex.Message, errorMessage, "Cannot select articles more than the max headlines");
            }
        }

        [TestMethod]
        public void WorkspaceReqConvManagerShouldThrowExceptionIfSumOfSelectedAndExistingArticlesIsGreaterThanMaxHeadlines()
        {
            _maxHeadlinesInWorkspace = 8;
            var request = getWorkspaceRequestDto("array");
            var autoWorkspaceRequest = getAutomaticWorkspace("an");
            try
            {
                var response = ConversionManager.AddWorkspaceItem(autoWorkspaceRequest, request, _maxHeadlinesInWorkspace);
                Assert.Fail("Should throw exception");
            }
            catch (DowJonesUtilitiesException ex)
            {
                var errorMessage = string.Format("{0} {1} {2} {3} {4}", "{newsletterMaxHeadlines-1a}", request.ContentItemsToAdd.Count + autoWorkspaceRequest.ItemsCollection.Count,
                    "{newsletterMaxHeadlines-2a}", _maxHeadlinesInWorkspace, "{newsletterMaxHeadlines-2b}");
                Assert.AreEqual(ex.Message, errorMessage, "Sum of selected and existing cannot exceed max headlines");
            }
        }

        [TestMethod]
        public void WorkspaceReqConvManagerShouldReturnAddItemsToWorkspaceRequestForValidRequest()
        {

            var request = getWorkspaceRequestDto("array");
            var autoWorkspaceRequest = getAutomaticWorkspace("an");
            try
            {
                var addItemsToWorkspaceRequest = ConversionManager.AddWorkspaceItem(autoWorkspaceRequest, request, _maxHeadlinesInWorkspace);
                var response = (AddItemsToAutomaticWorkspaceDTO)(addItemsToWorkspaceRequest.DataTransferObject);
                Assert.AreEqual(response.ItemCollection.Count, request.ContentItemsToAdd.Count, "Since articles are appended at the end");
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
                        {an+"3", ContentCategory.Picture},
                        {an+"4", ContentCategory.Multimedia},
                        {an+"5", ContentCategory.Blog},
                        {an+"6", ContentCategory.Board},
                        {an+"7", ContentCategory.Internal},
                        {an+"8", ContentCategory.External}
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

        private AutomaticWorkspace getAutomaticWorkspace(string an)
        {
            if (an == "none")
            {
                return new AutomaticWorkspace();
            }
            return new AutomaticWorkspace
            {
                Id = 12,
                ItemsCollection = new ContentItemCollection
                {
                    new ArticleItem
                    {
                        AccessionNumber = an
                    }
                }
            };
        }
        #endregion
    }
}


