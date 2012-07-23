using System;
using System.Web.Mvc;
using DowJones.AlertEditor;
using DowJones.Managers.Alert;
using DowJones.Search;
using Factiva.Gateway.Messages.Track.V1_0;
using ControllerBase = DowJones.Web.Mvc.ControllerBase;

namespace DowJones.Web.Showcase.Controllers
{
    public class AlertEditorController : ControllerBase
    {
        private readonly AlertManager _alertManager;

        public AlertEditorController(AlertManager alertManager)
        {
            _alertManager = alertManager;
        }

        //
        // GET: /AlertEditor/

        public ActionResult Index()
        {
            var model = new Mvc.UI.Components.AlertEditor.AlertEditorModel
                            {
                                ID = "1234"
                            };
            return View(model);
        }


        public ActionResult Create(string name, string keywords, string type)
        {
            var q = new FreeTextSearchQuery {FreeText = keywords};


            var f = new SourceQueryFilters();
            var eachPill = new SourceQueryFilterEntities
                               {
                                   new SourceQueryFilterEntity
                                       {SourceCode = "PRN", SourceType = SourceFilterType.Restrictor},
                                   new SourceQueryFilterEntity
                                       {SourceCode = "Newswire", SourceType = SourceFilterType.SourceName},
                               };
            f.Add(eachPill);
            eachPill = new SourceQueryFilterEntities
                           {
                               new SourceQueryFilterEntity {SourceCode = "J", SourceType = SourceFilterType.Restrictor},
                           };
            f.Add(eachPill);

            q.Source = new CompoundQueryFilter {Include = f};

            var entity = new EntitiesQueryFilter(EntityType.Company);
            entity.AddRange(new[] {"IBM", "ORCLE", "YAHCOR", "GOOG", "MCROST"});

            q.Company = new CompoundQueryFilter { Include = new[] { entity } };

            q.ProductId = "communicator";

            var request = new AlertRequestBase
                              {
                                  Properties = new AlertProperties
                                                   {
                                                       AlertName = name,
                                                       ProductType =
                                                           (ProductType) Enum.Parse(typeof (ProductType), type),
                                                       DeliveryMethod = DeliveryMethod.Batch,
                                                       EmailAddress = "RxNp237@yahoo.com",
                                                       AdjustToDaylightSavingsTime = true,
                                                       TimeZoneOffset = "-05:00",
                                                       DocumentType = DocumentType.FULL,
                                                       DeliveryTime = DeliveryTimes.Both,
                                                       DocumentFormat = DocumentFormat.TextHtml,
                                                       RemoveDuplicate = DeduplicationLevel.Similar
                                                   },
                                  SearchQuery = q
                              };

            FolderIDResponse response = _alertManager.CreateAlert(request);
            response.folderName = name;

            return View(response);
        }

        public ActionResult Update(string id, string name, string keywords, string type)
        {
            var q = new SimpleSearchQuery {Keywords = keywords};
            q.ProductId = "communicator";
            var request = new AlertRequestBase
                              {
                                  Properties = new AlertProperties
                                                   {
                                                       AlertId = id,
                                                       AlertName = name,
                                                       ProductType =
                                                           (ProductType) Enum.Parse(typeof (ProductType), type),
                                                       DeliveryMethod = DeliveryMethod.Online,
                                                       RemoveDuplicate = DeduplicationLevel.Similar
                                                   },
                                  SearchQuery = q
                              };

            FolderIDResponse response = _alertManager.UpdateAlert(request);
            response.folderName = name;
            return View("Create", response);
        }

        public ActionResult Get(string id)
        {
            AlertDetails response = _alertManager.GetAlert(id);
            var folderIdResponse = new FolderIDResponse
                                       {
                                           folderID = response.Properties.AlertId,
                                           folderName = response.Properties.AlertName
                                       };
            return View("Create", folderIdResponse);
        }
    }
}
