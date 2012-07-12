﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
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
    using AuthorList;
    using DowJones.Web.Mvc.Extensions;
    
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.OutletList.OutletList.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.OutletList.OutletListComponent))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "0.0.0.20608")]
    public class OutletListComponent : DowJones.Web.Mvc.UI.ViewComponentBase<OutletListModel>
    {
#line hidden

        public OutletListComponent()
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


  
	uint runningSum = @Model.FirstRecordIndex;
	uint i = 0;




 if (Model != null)
{
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



WriteLiteral("\t<table class=\"dj_data_table-sorter dj_data_table dj_author-list-table\">\r\n\t\t<colg" +
"roup>\r\n\t\t\t<col class=\"dj_data_table-select-col\" />\r\n\t\t\t<col class=\"dj_data_table" +
"-col1\" />\r\n\t\t\t<col class=\"dj_data_table-col2\" />\r\n");


 			foreach (AuthorListColumns col in Model.DisplayedColumns)
			{

WriteLiteral("\t\t\t\t<col />\r\n");


			}

WriteLiteral("\t\t</colgroup>\r\n\t\t<thead>\r\n\t\t\t<tr>\r\n\t\t\t\t<th scope=\"col\"></th>\r\n\t\t\t\t<th scope=\"col\"" +
"></th>\r\n\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-header\" data-sort=\"outlet\">" +
"\r\n\t\t\t\t\t");


Write(Model.Tokens.OutletName);

WriteLiteral("\r\n\t\t\t\t\t");


 Write(Model.SortBy == OutletListSortColumns.OutletName ? sort : "");

WriteLiteral("\r\n\t\t\t\t</th>\r\n");


 				if (Model.DisplayedColumns.Contains(OutletListSortColumns.Circulation))
				{ 

WriteLiteral("\t\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-header\" data-sort=\"circulation\" sty" +
"le=\"text-align:right\">\r\n\t\t\t\t\t\t");


 Write(Model.Tokens.Circulation);

WriteLiteral("\r\n\t\t\t\t\t\t");


  Write(Model.SortBy == OutletListSortColumns.Circulation ? sort : "");

WriteLiteral("\r\n\t\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(OutletListSortColumns.OriginCountry))
				{ 

WriteLiteral("\t\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-column\">\r\n\t\t\t\t\t\t");


 Write(Model.Tokens.OriginCountry);

WriteLiteral("\r\n\t\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(OutletListSortColumns.Country))
				{ 

WriteLiteral("\t\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-header\" data-sort=\"country\">\r\n\t\t\t\t\t" +
"\t");


 Write(Model.Tokens.Country);

WriteLiteral("\r\n\t\t\t\t\t\t");


  Write(Model.SortBy == OutletListSortColumns.Country ? sort : "");

WriteLiteral("\r\n\t\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(OutletListSortColumns.State))
				{ 

WriteLiteral("\t\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-header\" data-sort=\"state\">\r\n\t\t\t\t\t\t");


 Write(Model.Tokens.State);

WriteLiteral("\r\n\t\t\t\t\t\t");


  Write(Model.SortBy == OutletListSortColumns.State ? sort : "");

WriteLiteral("\r\n\t\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(OutletListSortColumns.City))
				{ 

WriteLiteral("\t\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-header\" data-sort=\"city\">\r\n\t\t\t\t\t\t");


 Write(Model.Tokens.City);

WriteLiteral("\r\n\t\t\t\t\t\t");


  Write(Model.SortBy == OutletListSortColumns.City ? sort : "");

WriteLiteral("\r\n\t\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(OutletListSortColumns.MediaFormat))
				{ 

WriteLiteral("\t\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-header\" data-sort=\"mediaformat\">\r\n\t" +
"\t\t\t\t\t");


 Write(Model.Tokens.MediaFormat);

WriteLiteral("\r\n\t\t\t\t\t\t");


  Write(Model.SortBy == OutletListSortColumns.MediaFormat ? sort : "");

WriteLiteral("\r\n\t\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(OutletListSortColumns.OutletType))
				{ 

WriteLiteral("\t\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-header\" data-sort=\"outlettype\">\r\n\t\t" +
"\t\t\t\t");


 Write(Model.Tokens.OutletType);

WriteLiteral("\r\n\t\t\t\t\t\t");


  Write(Model.SortBy == OutletListSortColumns.OutletType ? sort : "");

WriteLiteral("\r\n\t\t\t\t\t</th>\r\n");


				}
				

                   


 				if (Model.DisplayedColumns.Contains(OutletListSortColumns.Language))
				{ 

WriteLiteral("\t\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-column\" data-sort=\"language\">\r\n\t\t\t\t" +
"\t\t");


 Write(Model.Tokens.Language);

WriteLiteral("\r\n\t\t\t\t\t\t");


  Write(Model.SortBy == OutletListSortColumns.Language ? sort : "");

WriteLiteral("\r\n\t\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(OutletListSortColumns.Frequency))
				{ 

WriteLiteral("\t\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-header\" data-sort=\"frequency\">\r\n\t\t\t" +
"\t\t\t");


 Write(Model.Tokens.Frequency);

WriteLiteral("\r\n\t\t\t\t\t\t");


  Write(Model.SortBy == OutletListSortColumns.Frequency ? sort : "");

WriteLiteral("\r\n\t\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(OutletListSortColumns.Coverage))
				{ 

WriteLiteral("\t\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-header\" data-sort=\"coverage\">\r\n\t\t\t\t" +
"\t\t");


 Write(Model.Tokens.Coverage);

WriteLiteral("\r\n\t\t\t\t\t\t");


  Write(Model.SortBy == OutletListSortColumns.Coverage ? sort : "");

WriteLiteral("\r\n\t\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(OutletListSortColumns.Industries))
				{ 

WriteLiteral("\t\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-column\" data-sort=\"industries\">\r\n\t\t" +
"\t\t\t\t");


 Write(Model.Tokens.Industries);

WriteLiteral("\r\n\t\t\t\t\t\t");


  Write(Model.SortBy == OutletListSortColumns.Industries ? sort : "");

WriteLiteral("\r\n\t\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(OutletListSortColumns.Subjects))
				{ 

WriteLiteral("\t\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-column\" data-sort=\"subjects\">\r\n\t\t\t\t" +
"\t\t");


 Write(Model.Tokens.Subjects);

WriteLiteral("\r\n\t\t\t\t\t\t");


  Write(Model.SortBy == OutletListSortColumns.Subjects ? sort : "");

WriteLiteral("\r\n\t\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(OutletListSortColumns.Regions))
				{ 

WriteLiteral("\t\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-column\" data-sort=\"regions\">\r\n\t\t\t\t\t" +
"\t");


 Write(Model.Tokens.Regions);

WriteLiteral("\r\n\t\t\t\t\t\t");


  Write(Model.SortBy == OutletListSortColumns.Regions ? sort : "");

WriteLiteral("\r\n\t\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(OutletListSortColumns.Publisher))
				{ 

WriteLiteral("\t\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-header\" data-sort=\"publisher\">\r\n\t\t\t" +
"\t\t\t");


 Write(Model.Tokens.Publisher);

WriteLiteral("\r\n\t\t\t\t\t\t");


  Write(Model.SortBy == OutletListSortColumns.Publisher ? sort : "");

WriteLiteral("\r\n\t\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(OutletListSortColumns.UserAddedInfo))
				{ 

WriteLiteral("\t\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-column\" data-sort=\"useraddedinfo\">\r" +
"\n\t\t\t\t\t\t");


 Write(Model.Tokens.UserAddedInfo);

WriteLiteral("\r\n\t\t\t\t\t\t");


  Write(Model.SortBy == OutletListSortColumns.UserAddedInfo ? sort : "");

WriteLiteral("\r\n\t\t\t\t\t</th>\r\n");


				}

WriteLiteral("\t\t\t</tr>\r\n\t\t</thead>\r\n\t\t<tbody class=\"dj_data_table-scroll\">\r\n");


 		foreach (var outlet in Model.Outlets)
		{
			string oddClass = ++i % 2 == 0 ? "" : " class='odd'";

WriteLiteral("\t\t\t\t<tr");


   Write(oddClass);

WriteLiteral(">\r\n\t\t\t\t\t<td class=\"dj_data_table-select-col\">\r\n\t\t\t\t\t\t<input name=\"dj_outlet-selec" +
"t\" type=\"checkbox\" outletlist-aid=\'");


                                                                Write(outlet.OutletId);

WriteLiteral("\'/>\r\n\t\t\t\t\t</td>\r\n\t\t\t\t\t<td class=\"dj_num-col\">");


                        Write(runningSum++);

WriteLiteral(".</td>\r\n\t\t\t\t\t<td class=\"dj_sortable-sorted\"><a href=\"javascript:void(0);\" class=\"" +
"outlet-selector\">");


                                                                                     Write(outlet.OutletName);

WriteLiteral("</a></td>\r\n");


 					if (Model.DisplayedColumns.Contains(OutletListSortColumns.Circulation))
					{ 

WriteLiteral("\t\t\t\t\t\t\t\t\t<td align=\"right\">\r\n");


 											if (outlet.Circulation > 0)
							{
												
       Write(outlet.Circulation.ToString("0,0", System.Globalization.CultureInfo.InvariantCulture));

                                                                                                  
							}
							else
							{

WriteLiteral("\t\t\t\t\t\t\t\t\t\t\t\t<span></span>\r\n");


							}

WriteLiteral("\t\t\t\t\t\t\t\t\t</td>\r\n");


					}


 					if (Model.DisplayedColumns.Contains(OutletListSortColumns.OriginCountry))
					{ 

WriteLiteral("\t\t\t\t\t\t<td>");


     Write(outlet.OriginCountry);

WriteLiteral("</td>\r\n");


					}


 					if (Model.DisplayedColumns.Contains(OutletListSortColumns.Country))
					{ 

WriteLiteral("\t\t\t\t\t\t<td>");


     Write(outlet.Country);

WriteLiteral("</td>\r\n");


					}


 					if (Model.DisplayedColumns.Contains(OutletListSortColumns.State))
					{ 

WriteLiteral("\t\t\t\t\t\t<td>");


     Write(outlet.State);

WriteLiteral("</td>\r\n");


					}


 					if (Model.DisplayedColumns.Contains(OutletListSortColumns.City))
					{ 

WriteLiteral("\t\t\t\t\t\t<td>");


     Write(outlet.City);

WriteLiteral("</td>\r\n");


					}


 					if (Model.DisplayedColumns.Contains(OutletListSortColumns.MediaFormat))
					{ 

WriteLiteral("\t\t\t\t\t\t<td>");


     Write(outlet.MediaFormat.Name);

WriteLiteral("</td>\r\n");


					}


 					if (Model.DisplayedColumns.Contains(OutletListSortColumns.OutletType))
					{ 

WriteLiteral("\t\t\t\t\t\t<td>");


     Write(outlet.OutletType.Name);

WriteLiteral("</td>\r\n");


					}


 					if (Model.DisplayedColumns.Contains(OutletListSortColumns.Language))
					{
						if (outlet.Languages == null || outlet.Languages.Count == 0)
						{

WriteLiteral("\t\t\t\t\t\t\t<td></td> \r\n");


						}
						else
						{  

WriteLiteral("\t\t\t\t\t\t\t<td>");


      Write(outlet.GetLanguagesText().Replace("|", "<br />"));

WriteLiteral("</td>\r\n");


						}
					}


 					if (Model.DisplayedColumns.Contains(OutletListSortColumns.Frequency))
					{
						if (outlet.Frequency == null)
						{

WriteLiteral("\t\t\t\t\t\t\t<td></td>\r\n");


						}
						else
						{ 

WriteLiteral("\t\t\t\t\t\t\t<td>");


      Write(outlet.Frequency.Name);

WriteLiteral("</td>\r\n");


						}
					}


 					if (Model.DisplayedColumns.Contains(OutletListSortColumns.Coverage))
					{ 

WriteLiteral("\t\t\t\t\t\t<td>");


     Write(outlet.Coverage);

WriteLiteral("</td>\r\n");


					}


 					if (Model.DisplayedColumns.Contains(OutletListSortColumns.Industries))
					{
						if (outlet.IndustryEditorialFocus == null || outlet.IndustryEditorialFocus.Count == 0)
						{

WriteLiteral("\t\t\t\t\t\t\t<td></td> \r\n");


						}
						else
						{ 

WriteLiteral("\t\t\t\t\t\t\t<td>");


      Write(outlet.GetIndustriesText().Replace("|", "<br />"));

WriteLiteral("</td>\r\n");


						}
					}


 					if (Model.DisplayedColumns.Contains(OutletListSortColumns.Subjects))
					{
						if (outlet.SubjectEditorialFocus == null || outlet.SubjectEditorialFocus.Count == 0)
						{

WriteLiteral("\t\t\t\t\t\t\t<td></td> \r\n");


						}
						else
						{ 

WriteLiteral("\t\t\t\t\t\t\t<td>");


      Write(outlet.GetSubjectsText().Replace("|", "<br />"));

WriteLiteral("</td>\r\n");


						}
					}


 					if (Model.DisplayedColumns.Contains(OutletListSortColumns.Regions))
					{
						if (outlet.RegionEditorialFocus == null || outlet.RegionEditorialFocus.Count == 0)
						{

WriteLiteral("\t\t\t\t\t\t\t<td></td> \r\n");


						}
						else
						{ 

WriteLiteral("\t\t\t\t\t\t\t<td>");


      Write(outlet.GetRegionsText().Replace("|", "<br />"));

WriteLiteral("</td>\r\n");


						}
					}


 					if (Model.DisplayedColumns.Contains(OutletListSortColumns.Publisher))
					{ 

WriteLiteral("\t\t\t\t\t\t<td>");


     Write(outlet.Publisher);

WriteLiteral("</td>\r\n");


					}


 					if (Model.DisplayedColumns.Contains(OutletListSortColumns.UserAddedInfo))
					{
						if (outlet.UserAddedInformation == null)
						{

WriteLiteral("\t\t\t\t\t\t\t<td></td> \r\n");


						}
						else
						{ 

WriteLiteral("\t\t\t\t\t\t\t<td>");


       Write(new MvcHtmlString(outlet.GetUserAddedInfoText()));

WriteLiteral("\r\n\t\t\t\t\t\t\t</td>\r\n");


						}
					}

WriteLiteral("\t\t\t </tr>\r\n");


  }

WriteLiteral("\t\t</tbody>\r\n\t</table>\r\n");



WriteLiteral("\t\t<!-- Save As Menu -->\r\n");



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
