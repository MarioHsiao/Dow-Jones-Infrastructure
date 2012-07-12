﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Web.UI.WebResourceAttribute("DowJones.Web.Mvc.UI.Components.AuthorList.AuthorList.js", "text/javascript")]

namespace DowJones.Web.Mvc.UI.Components.AuthorList
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
    using System.Collections;
    using DowJones.Web.Mvc.Extensions;
    
    [DowJones.Web.ScriptResourceAttribute(null, ResourceName="DowJones.Web.Mvc.UI.Components.AuthorList.AuthorList.js", ResourceKind=DowJones.Web.ClientResourceKind.Script, DeclaringType=typeof(DowJones.Web.Mvc.UI.Components.AuthorList.AuthorListComponent))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorViewComponentClassGenerator", "0.0.0.20608")]
    public class AuthorListComponent : DowJones.Web.Mvc.UI.ViewComponentBase<AuthorListModel>
    {
#line hidden

        public AuthorListComponent()
        {
        }
        public override string ClientPluginName
        {
            get
            {
                return "dj_AuthorList";
            }
        }
        public override void ExecuteTemplate()
        {




WriteLiteral("\r\n");





   CssClass = "dj_AuthorList"; 


  
	uint runningSum = @Model.FirstRecordIndex;
	uint i = 0;
	uint numberOfColumnsAfterOutletRelatedColumns = 0;



 if (Model != null && Model.Authors.Any())
{
	string sort = "<span class='dj_sortable-table-columnUp'></span>";
	if (Model.SortOrder == OrderDirections.Descending)
	{
		sort = "<span class='dj_sortable-table-columnDown'></span>";
	}

WriteLiteral(@"	<script type=""text/javascript"">
	    $(document).ready(function () {

	        var ids = $(""#selected_author_ids"").val();
	        if (ids == """") return;

	        var idarr = ids.split("","");
	        $(""td input:checkbox"").each(function () {
	            var $this = $(this);
	            var id = $this.attr(""authorlist-aid"");
	            var idx = $.inArray(id, idarr);
	            if (idx > -1) {
	                $this.attr(""checked"", true);
	            }
	        });

	    });
	</script>
");



WriteLiteral("\t<input type=\"hidden\" id=\"author_list_sort_by\" value=\"");


                                                  Write(EnumDescription.StringValueOf(Model.SortBy));

WriteLiteral("\" />\r\n");



WriteLiteral("\t<input type=\"hidden\" id=\"author_list_sort_order\" value=\"");


                                                     Write(EnumDescription.StringValueOf(Model.SortOrder));

WriteLiteral("\" />\r\n");



WriteLiteral("\t<input type=\"hidden\" id=\"selected_author_ids\" value=\"");


                                                 Write(Model.SelectedAuthorIds);

WriteLiteral("\" />\r\n");



WriteLiteral("\t<table class=\"dj_data_table-sorter dj_data_table dj_author-list-table\">\r\n\t\t<colg" +
"roup>\r\n\t\t\t<col class=\"dj_data_table-select-col\" />\r\n\t\t\t<col class=\"dj_data_table" +
"-col1\" />\r\n\t\t\t<col class=\"dj_data_table-col2\" />\r\n\t\t\t<col class=\"dj_data_table-c" +
"ol3\" />\r\n");


 			foreach (AuthorListColumns col in Model.DisplayedColumns)
			{

WriteLiteral("\t\t\t\t<col />\r\n");


			}

WriteLiteral("\t\t</colgroup>\r\n\t\t<thead>\r\n\t\t\t<tr>\r\n\t\t\t\t<th scope=\"col\"></th>\r\n\t\t\t\t<th scope=\"col\"" +
"></th>\r\n\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-header\" data-sort=\"contact\"" +
">\r\n\t\t\t\t\t");


Write(Model.Tokens.ContactName);

WriteLiteral("\r\n\t\t\t\t\t");


 Write(Model.SortBy == AuthorListSortColumns.ContactName ? sort : "");

WriteLiteral("\r\n\t\t\t\t</th>\r\n\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-column\">\r\n\t\t\t\t\t");


Write(Model.Tokens.Articles);

WriteLiteral("<br />(");


                             Write(Model.Tokens.Last90Days);

WriteLiteral(")\r\n\t\t\t\t</th>\r\n");


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.OutletName))
				{ 

WriteLiteral("\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-header\" data-sort=\"outlet\">\r\n\t\t\t\t\t");


Write(Model.Tokens.OutletName);

WriteLiteral("\r\n\t\t\t\t\t");


 Write(Model.SortBy == AuthorListSortColumns.OutletName ? sort : "");

WriteLiteral("\r\n\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.OutletOriginCountry))
				{ 

WriteLiteral("\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-column\">\r\n\t\t\t\t\t");


Write(Model.Tokens.OutletOriginCountry);

WriteLiteral("\r\n\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.OutletCountry))
				{ 

WriteLiteral("\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-header\" data-sort=\"outlet-country\">\r" +
"\n\t\t\t\t\t");


Write(Model.Tokens.OutletCountry);

WriteLiteral("\r\n\t\t\t\t\t");


 Write(Model.SortBy == AuthorListSortColumns.OutletCountry ? sort : "");

WriteLiteral("\r\n\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.OutletState))
				{ 

WriteLiteral("\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-header\" data-sort=\"outlet-state\">\r\n\t" +
"\t\t\t\t");


Write(Model.Tokens.OutletState);

WriteLiteral("\r\n\t\t\t\t\t");


 Write(Model.SortBy == AuthorListSortColumns.OutletState ? sort : "");

WriteLiteral("\r\n\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.OutletCity))
				{ 

WriteLiteral("\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-header\" data-sort=\"outlet-city\">\r\n\t\t" +
"\t\t\t");


Write(Model.Tokens.OutletCity);

WriteLiteral("\r\n\t\t\t\t\t");


 Write(Model.SortBy == AuthorListSortColumns.OutletCity ? sort : "");

WriteLiteral("\r\n\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.OutletType))
				{ 

WriteLiteral("\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-header\" data-sort=\"outlet-type\">\r\n\t\t" +
"\t\t\t");


Write(Model.Tokens.OutletType);

WriteLiteral("\r\n\t\t\t\t\t");


 Write(Model.SortBy == AuthorListSortColumns.OutletType ? sort : "");

WriteLiteral("\r\n\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.OutletFrequency))
				{ 

WriteLiteral("\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-header\" data-sort=\"outlet-frequency\"" +
">\r\n\t\t\t\t\t");


Write(Model.Tokens.OutletFrequency);

WriteLiteral("\r\n\t\t\t\t\t");


 Write(Model.SortBy == AuthorListSortColumns.OutletFrequency ? sort : "");

WriteLiteral("\r\n\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.OutletCirculation))
				{

WriteLiteral("\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-header\" data-sort=\"circulation\" styl" +
"e=\"text-align:right\">\r\n\t\t\t\t\t");


Write(Model.Tokens.Circulation);

WriteLiteral("\r\n\t\t\t\t\t");


 Write(Model.SortBy == AuthorListSortColumns.Circulation ? sort : "");

WriteLiteral("\r\n\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.EmploymentType))
				{

WriteLiteral("\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-header\" data-sort=\"employment-type\">" +
"\r\n\t\t\t\t\t");


Write(Model.Tokens.EmploymentType);

WriteLiteral("\r\n\t\t\t\t\t");


 Write(Model.SortBy == AuthorListSortColumns.EmploymentType ? sort : "");

WriteLiteral("\r\n\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.Country))
				{
					numberOfColumnsAfterOutletRelatedColumns++;

WriteLiteral("\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-header\" data-sort=\"country\">\r\n\t\t\t\t\t");


Write(Model.Tokens.Country);

WriteLiteral("\r\n\t\t\t\t\t");


 Write(Model.SortBy == AuthorListSortColumns.Country ? sort : "");

WriteLiteral("\r\n\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.State))
				{
					numberOfColumnsAfterOutletRelatedColumns++;

WriteLiteral("\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-header\" data-sort=\"state\">\r\n\t\t\t\t\t");


Write(Model.Tokens.State);

WriteLiteral("\r\n\t\t\t\t\t");


 Write(Model.SortBy == AuthorListSortColumns.State ? sort : "");

WriteLiteral("\r\n\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.City))
				{
					numberOfColumnsAfterOutletRelatedColumns++;

WriteLiteral("\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-header\" data-sort=\"city\">\r\n\t\t\t\t\t");


Write(Model.Tokens.City);

WriteLiteral("\r\n\t\t\t\t\t");


 Write(Model.SortBy == AuthorListSortColumns.City ? sort : "");

WriteLiteral("\r\n\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.EmailAddresses))
				{
					numberOfColumnsAfterOutletRelatedColumns++;

WriteLiteral("\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-column\">\r\n\t\t\t\t\t");


Write(Model.Tokens.EmailAddresses);

WriteLiteral("\r\n\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.UserInfo))
				{
					numberOfColumnsAfterOutletRelatedColumns++;

WriteLiteral("\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-column\">\r\n\t\t\t\t\t");


Write(Model.Tokens.UserAddedContactInfo);

WriteLiteral("\r\n\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.Phones))
				{
					numberOfColumnsAfterOutletRelatedColumns++;

WriteLiteral("\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-column\">\r\n\t\t\t\t\t");


Write(Model.Tokens.Phones);

WriteLiteral("\r\n\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.JobTitle))
				{
					numberOfColumnsAfterOutletRelatedColumns++;

WriteLiteral("\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-header\" data-sort=\"job-title\">\r\n\t\t\t\t" +
"\t");


Write(Model.Tokens.JobTitle);

WriteLiteral("\r\n\t\t\t\t\t");


 Write(Model.SortBy == AuthorListSortColumns.JobTitle ? sort : "");

WriteLiteral("\r\n\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.BeatsIndustries))
				{
					numberOfColumnsAfterOutletRelatedColumns++;

WriteLiteral("\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-column\">\r\n\t\t\t\t\t");


Write(Model.Tokens.BeatsIndustries);

WriteLiteral("\r\n\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.BeatsSubjects))
				{
					numberOfColumnsAfterOutletRelatedColumns++;

WriteLiteral("\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-column\">\r\n\t\t\t\t\t");


Write(Model.Tokens.BeatsSubjects);

WriteLiteral("\r\n\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.BeatsRegions))
				{
					numberOfColumnsAfterOutletRelatedColumns++;

WriteLiteral("\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-column\">\r\n\t\t\t\t\t");


Write(Model.Tokens.BeatsRegions);

WriteLiteral("\r\n\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.PreferedContactMethod))
				{
					numberOfColumnsAfterOutletRelatedColumns++;

WriteLiteral("\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-column\">\r\n\t\t\t\t\t");


Write(Model.Tokens.PreferedContact);

WriteLiteral("<br />");


                                   Write(Model.Tokens.Method);

WriteLiteral("\r\n\t\t\t\t</th>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.RelatedMediaContacts))
				{
					numberOfColumnsAfterOutletRelatedColumns++;

WriteLiteral("\t\t\t\t<th scope=\"col\" class=\"dj_sortable-table-column\">\r\n\t\t\t\t\t");


Write(Model.Tokens.RelatedMediaContacts);

WriteLiteral("\r\n\t\t\t\t</th>\r\n");


				}

WriteLiteral("\t\t\t</tr>\r\n\t\t</thead>\r\n\t\t<tbody class=\"dj_data_table-scroll\">\r\n");


 		foreach (var author in Model.Authors)
		{
			OutletProperties firstOutlet = null;
			if (author.HasOutlets)
			{
				firstOutlet = author.Outlets.FirstOrDefault();
			}

			string oddClass = ++i % 2 == 0 ? "" : " class='odd'";

WriteLiteral("\t\t\t<tr");


  Write(oddClass);

WriteLiteral(">\r\n\t\t\t\t<td class=\"dj_data_table-select-col\">\r\n\t\t\t\t\t<input name=\"dj_author-select\"" +
" type=\"checkbox\" authorlist-aid=\'");


                                                               Write(author.AuthorId);

WriteLiteral("\' authorlist-nnid=\'");


                                                                                                  Write(author.AuthorNNID);

WriteLiteral("\' />\r\n\t\t\t\t</td>\r\n\t\t\t\t<td class=\"dj_num-col\">");


                       Write(runningSum++);

WriteLiteral(".</td>\r\n\t\t\t\t<td class=\"dj_contact-name-column\">\r\n\t\t\t\t\t");


 Write(Model.AnyOutletRelatedColumn && author.ExpandableOutlet ? "<div class='dj_collapsable-icon collapsed'></div>" : "");

WriteLiteral("\r\n\t\t\t\t\t<a href=\"javascript:void(0);\" class=\"author-name-selector\">");


                                                           Write(author.AuthorName);

WriteLiteral("</a>\r\n\t\t\t\t</td>\r\n\t\t\t\t<td>\r\n");


 					if (author.HasArticles)
					{

WriteLiteral("\t\t\t\t\t\t<a href=\"javascript:void(0);\" class=\"author-articles-selector\">");


                                                                Write(Model.Tokens.ViewArticles);

WriteLiteral("</a>\r\n");


					}
					else
					{
						
 Write(Model.Tokens.NoArticles);

                              
					}

WriteLiteral("\t\t\t\t</td>\r\n");


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.OutletName))
				{

WriteLiteral("\t\t\t\t<td>\r\n");


 					if (author.HasOutlets)
					{

WriteLiteral("\t\t\t\t\t\t<a href=\"javascript:void(0);\" authorlist-outlet-id=\"");


                                                     Write(firstOutlet.OutletId);

WriteLiteral("\" class=\"author-outlet-selector\">");


                                                                                                           Write(firstOutlet.OutletName);

WriteLiteral("</a>\r\n");


					}
					else
					{

WriteLiteral("\t\t\t\t\t\t<span></span>\r\n");


					}

WriteLiteral("\t\t\t\t</td>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.OutletOriginCountry))
				{

WriteLiteral("\t\t\t\t<td>\r\n");


 					if (author.HasOutlets)
					{
						
 Write(firstOutlet.OutletOriginCountry);

                                      
					}
					else
					{

WriteLiteral("\t\t\t\t\t\t<span></span>\r\n");


					}

WriteLiteral("\t\t\t\t</td>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.OutletCountry))
				{

WriteLiteral("\t\t\t\t<td>\r\n");


 					if (author.HasOutlets)
					{
						
 Write(firstOutlet.OutletCountry);

                                
					}
					else
					{

WriteLiteral("\t\t\t\t\t\t<span></span>\r\n");


					}

WriteLiteral("\t\t\t\t</td>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.OutletState))
				{

WriteLiteral("\t\t\t\t<td>\r\n");


 					if (author.HasOutlets)
					{
						
 Write(firstOutlet.OutletState);

                              
					}
					else
					{

WriteLiteral("\t\t\t\t\t\t<span></span>\r\n");


					}

WriteLiteral("\t\t\t\t</td>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.OutletCity))
				{

WriteLiteral("\t\t\t\t<td>\r\n");


 					if (author.HasOutlets)
					{
						
 Write(firstOutlet.OutletCity);

                             
					}
					else
					{

WriteLiteral("\t\t\t\t\t\t<span></span>\r\n");


					}

WriteLiteral("\t\t\t\t</td>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.OutletType))
				{

WriteLiteral("\t\t\t\t<td>\r\n");


 					if (author.HasOutlets)
					{
						
 Write(firstOutlet.Type.Name);

                            
					}
					else
					{

WriteLiteral("\t\t\t\t\t\t<span></span>\r\n");


					}

WriteLiteral("\t\t\t\t</td>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.OutletFrequency))
				{

WriteLiteral("\t\t\t\t<td>\r\n");


 					if (author.HasOutlets)
					{
						
 Write(firstOutlet.Frequency.Name);

                                 
					}
					else
					{

WriteLiteral("\t\t\t\t\t\t<span></span>\r\n");


					}

WriteLiteral("\t\t\t\t</td>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.OutletCirculation))
				{

WriteLiteral("\t\t\t\t<td align=\"right\">\r\n");


 					if (author.HasOutlets && firstOutlet.Circulation > 0)
					{
						
 Write(firstOutlet.Circulation.ToString("0,0", System.Globalization.CultureInfo.InvariantCulture));

                                                                                                 
					}
					else
					{

WriteLiteral("\t\t\t\t\t\t<span></span>\r\n");


					}

WriteLiteral("\t\t\t\t</td>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.EmploymentType))
				{

WriteLiteral("\t\t\t\t<td>\r\n");


 					if (author.HasOutlets)
					{ 
						
 Write(firstOutlet.EmploymentType);

                                 
					}
					else
					{

WriteLiteral("\t\t\t\t\t\t<span></span>\r\n");


					}

WriteLiteral("\t\t\t\t</td>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.Country))
				{

WriteLiteral("\t\t\t\t<td>");


   Write(author.Country);

WriteLiteral("</td>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.State))
				{

WriteLiteral("\t\t\t\t<td>");


   Write(author.State);

WriteLiteral("</td>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.City))
				{

WriteLiteral("\t\t\t\t<td>");


   Write(author.City);

WriteLiteral("</td>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.EmailAddresses))
				{

WriteLiteral("\t\t\t\t<td>");


   Write(author.EmailAddresses);

WriteLiteral("</td>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.UserInfo))
				{

WriteLiteral("\t\t\t\t<td>");


    Write(new MvcHtmlString(author.CreateTextFromUserAddedContactInfo()));

WriteLiteral("</td>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.Phones))
				{

WriteLiteral("\t\t\t\t<td>");


   Write(author.Phones);

WriteLiteral("</td>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.JobTitle))
				{

WriteLiteral("\t\t\t\t<td>");


   Write(author.JobTitle);

WriteLiteral("</td>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.BeatsIndustries))
				{

WriteLiteral("\t\t\t\t<td>");


   Write(author.BeatsIndustries);

WriteLiteral("</td>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.BeatsSubjects))
				{

WriteLiteral("\t\t\t\t<td>");


   Write(author.BeatsSubjects);

WriteLiteral("</td>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.BeatsRegions))
				{

WriteLiteral("\t\t\t\t<td>");


   Write(author.BeatsRegions);

WriteLiteral("</td>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.PreferedContactMethod))
				{

WriteLiteral("\t\t\t\t<td>\r\n");


 					if (author.ContactMethod == null || author.ContactMethod.Type == ContactMethodTypes.None)
					{ 

WriteLiteral("\t\t\t\t\t\t<span></span>\r\n");


					}
					else
					{ 

WriteLiteral("\t\t\t\t\t\t<b>");


     Write(EnumDescription.StringValueOf(author.ContactMethod.Type) + ":");

WriteLiteral("</b>\r\n");



WriteLiteral("\t\t\t\t\t\t<br />\r\n");


						
 Write(author.ContactMethod.Contact);

                                   
					}

WriteLiteral("\t\t\t\t</td>\r\n");


				}


 				if (Model.DisplayedColumns.Contains(AuthorListColumns.RelatedMediaContacts))
				{

WriteLiteral("\t\t\t\t<td>");


   Write(author.RelatedMediaContacts);

WriteLiteral("</td>\r\n");


				}

WriteLiteral("\t\t\t</tr>\r\n");


				if (Model.AnyOutletRelatedColumn && author.ExpandableOutlet)
				{
					oddClass = i % 2 == 0 ? "" : " odd";
					foreach (var outlet in author.Outlets.Skip(1))
					{

WriteLiteral("\t\t\t\t\t\t<tr class=\"outlet-other");


                         Write(oddClass);

WriteLiteral(" hide\">\r\n\t\t\t\t\t\t\t<td colspan=\"4\"></td>\r\n");


 							if (Model.DisplayedColumns.Contains(AuthorListColumns.OutletName))
							{

WriteLiteral("\t\t\t\t\t\t\t<td>\r\n\t\t\t\t\t\t\t\t<a href=\"javascript:void(0);\" authorlist-outlet-id=\"");


                                                       Write(outlet.OutletId);

WriteLiteral("\" class=\"author-outlet-selector\">\r\n\t\t\t\t\t\t\t\t\t");


    Write(outlet.OutletName);

WriteLiteral("\r\n\t\t\t\t\t\t\t\t</a>\r\n\t\t\t\t\t\t\t</td>\r\n");


							}


 							if (Model.DisplayedColumns.Contains(AuthorListColumns.OutletOriginCountry))
							{

WriteLiteral("\t\t\t\t\t\t\t<td>");


      Write(outlet.OutletOriginCountry);

WriteLiteral("</td>\r\n");


							}


 							if (Model.DisplayedColumns.Contains(AuthorListColumns.OutletCountry))
							{

WriteLiteral("\t\t\t\t\t\t\t<td>");


      Write(outlet.OutletCountry);

WriteLiteral("</td>\r\n");


							}


 							if (Model.DisplayedColumns.Contains(AuthorListColumns.OutletState))
							{

WriteLiteral("\t\t\t\t\t\t\t<td>");


      Write(outlet.OutletState);

WriteLiteral("</td>\r\n");


							}


 							if (Model.DisplayedColumns.Contains(AuthorListColumns.OutletCity))
							{

WriteLiteral("\t\t\t\t\t\t\t<td>");


      Write(outlet.OutletCity);

WriteLiteral("</td>\r\n");


							}


 							if (Model.DisplayedColumns.Contains(AuthorListColumns.OutletType))
							{

WriteLiteral("\t\t\t\t\t\t\t<td>");


      Write(outlet.Type.Name);

WriteLiteral("</td>\r\n");


							}


 							if (Model.DisplayedColumns.Contains(AuthorListColumns.OutletFrequency))
							{

WriteLiteral("\t\t\t\t\t\t\t<td>");


      Write(outlet.Frequency.Name);

WriteLiteral("</td>\r\n");


							}


 							if (Model.DisplayedColumns.Contains(AuthorListColumns.OutletCirculation))
							{

WriteLiteral("\t\t\t\t\t\t\t<td align=\"right\">\r\n\t\t\t\t\t\t\t\t");


    Write(@outlet.Circulation == 0 ? "" : outlet.Circulation.ToString("0,0", System.Globalization.CultureInfo.InvariantCulture));

WriteLiteral("\r\n\t\t\t\t\t\t\t</td>\r\n");


							}


 							if (Model.DisplayedColumns.Contains(AuthorListColumns.EmploymentType))
							{

WriteLiteral("\t\t\t\t\t\t\t<td>");


      Write(outlet.EmploymentType);

WriteLiteral("</td>\r\n");


							}


 							if (numberOfColumnsAfterOutletRelatedColumns > 0)
							{ 

WriteLiteral("\t\t\t\t\t\t\t<td colspan=\"");


               Write(numberOfColumnsAfterOutletRelatedColumns);

WriteLiteral("\"></td>\r\n");


							}

WriteLiteral("\t\t\t\t\t\t</tr>\r\n");


					}
				}
			}

WriteLiteral("\t\t</tbody>\r\n\t</table>\r\n");



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


}
else
{

WriteLiteral("\t<span class=\"dj_noResults\">");


                       Write(Model.Tokens.NoResults);

WriteLiteral("</span>\r\n");


}

        }
    }
}
