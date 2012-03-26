// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyndicationData.cs" company="Dow Jones">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the GetSyndicationCanvasRequestDelegate type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DowJones.Tools.Ajax;
using DowJones.Tools.Ajax.HeadlineList;
using DowJones.Web.Mvc.UI.Canvas.Ajax.Core;

namespace DowJones.Web.Mvc.UI.Canvas.Ajax.CanvasModules.Basic
{
    public class GetSyndicationModuleRequestDelegate : BaseCanvasAjaxRequestDelegate
    {
        public string feedUri = string.Empty;
    }

    public class GetSyndicationModuleResponseDelegate : AbstractAjaxResponseDelegate
    {
        public HeadlineListDataResult data;
        public string title;
    }
}
