﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.OutletList.OutletList.js", "text/javascript")]

namespace DowJones.Web.Mvc.UI.Components.OutletList
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.Mvc;
    using DowJones.Web.Mvc.UI.Components.Models;
    using DowJones.Web.Mvc.Extensions;
    
    // Last Generated Timestamp: 05/29/2012 12:25 PM
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.OutletList.OutletList.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.OutletList.OutletList))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "0.0.0.30158")]
    public class OutletList : DowJones.Web.Mvc.UI.ViewComponentBase<DowJones.Web.Mvc.UI.Components.Models.OutletListModel>
    {
#line hidden

        public OutletList()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_OutletList";
            }
        }
        public override void ExecuteTemplate()
        {






   CssClass = "dj_OutletList"; 


 if (Model != null)
{
	Model.InitializeDomDataCollections();
	string sort = "<span class='dj_sortable-table-columnUp'></span>";
	if (Model.SortOrder == OrderDirections.Descending)
	{
		sort = "<span class='dj_sortable-table-columnDown'></span>";
	}

WriteLiteral(@"	<script type=""text/javascript"">
		$(document).ready(function () {

			var ids = $(""#selected_outlet_ids"").val();
			if (ids == """") return;

			var idarr = ids.split("","");
			$(""td input:checkbox"").each(function () {
				var $this = $(this);
				var id = $this.attr(""outletlist-aid"");
				var idx = $.inArray(id, idarr);
				if (idx > -1) {
					$this.attr(""checked"", true);
				}
			});

			if (idarr.length > 1) {
				if ($("".dj_list-select-count"").hasClass(""hide"") == true) {
					$("".dj_list-select-count"").removeClass(""hide"");
				}

				$("".dj_list-select-count span.count"").html(idarr.length);
			}

		});
	</script>
");


	

WriteLiteral("\t<input type=\"hidden\" id=\"outlet_list_sort_by\" value=\"");


                                                  Write(EnumDescription.StringValueOf(Model.SortBy));

WriteLiteral("\" />\r\n");



WriteLiteral("\t<input type=\"hidden\" id=\"outlet_list_sort_order\" value=\"");


                                                     Write(EnumDescription.StringValueOf(Model.SortOrder));

WriteLiteral("\" />\r\n");



WriteLiteral("\t<input type=\"hidden\" id=\"selected_outlet_ids\" value=\"");


                                                 Write(Model.SelectedOutletIds);

WriteLiteral("\" />\r\n");



WriteLiteral(@"	<div class=""dj_data_table-container"">
	<table class=""dj_data_table-sorter dj_data_table dj_author-list-table"">
		<thead>
			<tr>
				<th colspan=""2"" class=""dj_col-checkbox""></th>
				<th scope=""col"" class=""dj_sortable-table-header"" data-sort=""outlet"">
					");


Write(Model.Tokens.OutletName);

WriteLiteral("\r\n\t\t\t\t\t");


 Write(Model.SortBy == OutletListSortColumns.OutletName ? sort : "");

WriteLiteral("\r\n\t\t\t\t</th>\r\n\t\t\t\t<th class=\"dj_sortable-table-column dj_table-column-center\">");


                                                           Write(Model.Tokens.MediaContacts);

WriteLiteral("</th>\r\n\t\t\t\t<th class=\"dj_col-string dj_sortable-table-column\">");


                                                  Write(Model.Tokens.Articles);

WriteLiteral("</th>\r\n\r\n");


 				foreach (ThOutletItem th in Model.ThCollection) 
				{
					string s = String.Empty;
					if (th.Sortable)
					{
						s = String.Format(" data-sort='{0}'", th.SortableAttribut);
					}

WriteLiteral("\t\t\t\t\t<th class=\"");


           Write(th.ThClass);

WriteLiteral("\"");


                       Write(s);

WriteLiteral(">\r\n");


 						if(th.IsTextSplitable)
						{
							new MvcHtmlString(th.Text);
						}
						else
						{
							
  Write(th.Text);

               
						}

WriteLiteral("\t\t\t\t\t\t");


 Write(th.SortedSpan);

WriteLiteral("\r\n\t\t\t\t\t</th>\r\n");


				}

WriteLiteral("\t\t\t</tr>\r\n\t\t</thead>\r\n\t\t<tbody class=\"dj_data_table-scroll\">\r\n");


 		foreach(TrItem row in Model.TrCollection)
		{

WriteLiteral("\t\t\t<tr class=\"");


         Write(row.TrClass);

WriteLiteral("\">\r\n");


 				for (int c = 0; c < row.TdItems.Length; ++c)
				{
					TdItem td = row.TdItems[c];

WriteLiteral("\t\t\t\t\t<td class=\"");


           Write(td.TdClass);

WriteLiteral("\"");


                       Write(td.TdAttributes);

WriteLiteral(">\r\n");


 						if (td.IsAncor)
						{ 

WriteLiteral("\t\t\t\t\t\t\t<a href=\"");


           Write(td.AncorHref);

WriteLiteral("\" class=\"");


                                 Write(td.AncorClass);

WriteLiteral("\" ");


                                                 Write(td.AncorAttributes);

WriteLiteral(">\r\n");


 								if (td.IsHtmlText)
								{
									
     Write(new MvcHtmlString(td.Text));

                                      
								}
								else
								{
									
    Write(td.Text);

                 
								}

WriteLiteral("\t\t\t\t\t\t\t</a>\r\n");


						}
						else
						{
							if (td.IsHtmlText)
							{
								
    Write(new MvcHtmlString(td.Text));

                                     
							}
							else
							{
								
   Write(td.Text);

                
							}
						}

WriteLiteral("\t\t\t\t\t</td>\r\n");


				}

WriteLiteral("\t\t\t</tr>\r\n");


		}

WriteLiteral("\t\t</tbody>\r\n\t</table>\r\n\t</div>\r\n");



WriteLiteral("\t<!-- Save As Menu -->\r\n");



WriteLiteral("\t<div class=\"menu actionsMenu\" style=\"display:none;\">\r\n\t\t<div class=\"menuitems\">\r" +
"\n");


  		
			foreach (var menu in Model.Actions)
			{

WriteLiteral("\t\t\t\t<div class=\"menuitem\">\r\n\t\t\t\t\t<div class=\"label\" data-action=\"");


                                Write(menu.Value);

WriteLiteral("\">");


                                             Write(menu.Text);

WriteLiteral("</div>\r\n\t\t\t\t</div>\r\n");


			}
		

WriteLiteral("\t\t</div>\r\n\t</div>\r\n");



WriteLiteral("\t<!-- Save As Menu -->\r\n");



WriteLiteral("\t<!-- Modal Clear Selection -->\n");



WriteLiteral("\t<div id=\"dj_clear_selection\" style=\"display:none;\">\n\t\t<!-- Modal title tag: -->\n" +
"\t\t<h3 class=\"dj_modal-title\">");


                        Write(Model.Tokens.UnselectCheckboxesConfirmTitle);

WriteLiteral("</h3>\n\t\t<!-- Modal content tag: -->\n\t\t<div class=\"dj_modal-content\">\n\t\t\t<div clas" +
"s=\"dj_modal-content-wrap\" style=\"padding-top: 20px; padding-left: 20px; padding-" +
"right: 20px;\">\n\t\t\t\t<h3 style=\"height:54px;\">");


                        Write(Model.Tokens.UnselectCheckboxesConfirmMessage);

WriteLiteral(@"?</h3>
					<div class=""dj_modal-footer"">
					<input class=""dj_btn dj_btn-green export dj_confirm-unselect-checkboxes"" value=""Yes"" type=""submit"" />
					<span class=""dj_btn dj_btn-gray dj_modal-close"" onclick=""$().overlay.hide('#'+$(this).closest('.dj_modal').attr('id'));"">Cancel</span>
				</div>
			</div>
		</div>
	</div>
");



WriteLiteral("\t<!-- Modal Clear Selection -->\r\n");


}
else
{

WriteLiteral("\t <span class=\"dj_noResults\">");


                        Write(Model.Tokens.NoResults);

WriteLiteral("</span>\r\n");


}


        }
    }
}
