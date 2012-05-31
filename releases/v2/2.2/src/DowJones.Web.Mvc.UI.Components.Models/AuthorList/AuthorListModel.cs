using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using System;

namespace DowJones.Web.Mvc.UI.Components.Models
{
	public class AuthorListModel : ViewComponentModel
	{
		/// <summary>
		/// Gets and sets author list.
		/// </summary>
		public IEnumerable<AuthorModel> Authors { get; set; }

		/// <summary>
		/// Gets and sets first author index.
		/// Used in table to show a serial number.
		/// </summary>
		public uint FirstRecordIndex { get; set; }

		/// <summary>
		/// Gets and sets tokens to be used in AuthorList component.
		/// </summary>
		public AuthorListTokens Tokens { get; set; }

		/// <summary>
		/// Gets and sets column by which the author list will be sorted.
		/// </summary>
		public AuthorListSortColumns SortBy { get; set; }

		/// <summary>
		/// Gets and sets order direction for sorting the author list.
		/// </summary>
		public OrderDirections SortOrder { get; set; }

		[ClientProperty("pageSize")]
		public int PageSize { get; set; }

		[ClientProperty("totalResultCount")]
		public int TotalResultCount { get; set; }

		public bool ShowDeleteAction { get; set; }
		public bool ShowDistributeListAction { get; set; }

		public List<AuthorListColumns> DisplayedColumns { get; set; }

		public IEnumerable<SelectListItem> Actions
		{
			get
			{
				IEnumerable<SelectListItem> retVal = GetActionMenuItems();
				if (this.ShowDistributeListAction == false)
				{
					retVal = from a in retVal
							 where a.Value.Equals("distribute-list") == false
							 select a;
				}

				if (this.ShowDeleteAction == false)
				{
					retVal = from a in retVal
							 where a.Value.Equals("delete") == false
							 select a;
				}

				return retVal;
			}
		}

		public bool AnyOutletRelatedColumn
		{
			get
			{
				bool retval = this.DisplayedColumns.Contains(AuthorListColumns.OutletName)
					|| this.DisplayedColumns.Contains(AuthorListColumns.OutletEmploymentType)
					|| this.DisplayedColumns.Contains(AuthorListColumns.OutletJobTitle)
					|| this.DisplayedColumns.Contains(AuthorListColumns.OutletType)
					|| this.DisplayedColumns.Contains(AuthorListColumns.OutletCirculation)
					|| this.DisplayedColumns.Contains(AuthorListColumns.OutletFrequency)
					|| this.DisplayedColumns.Contains(AuthorListColumns.OutletState)
					|| this.DisplayedColumns.Contains(AuthorListColumns.OutletCountry)
					|| this.DisplayedColumns.Contains(AuthorListColumns.OutletEmailAddress)
					|| this.DisplayedColumns.Contains(AuthorListColumns.OutletPhone)
					|| this.DisplayedColumns.Contains(AuthorListColumns.OutletFax);

				return retval;
			}
		}

		[ClientData("selectedAuthorIds")]
		public string SelectedAuthorIds { get; set; }

		/// <summary>
		/// Create an instanse of AuthorListModel class.
		/// </summary>
		public AuthorListModel()
		{
			this.Authors = Enumerable.Empty<AuthorModel>();
			this.Tokens = new AuthorListTokens();
			this.ShowDeleteAction = false;
			this.ShowDistributeListAction = false;
			this.SortBy = AuthorListSortColumns.ContactName;
			this.SortOrder = OrderDirections.Ascending;
			this.DisplayedColumns = new List<AuthorListColumns>();
		}

		public void InitializeDomDataCollections()
		{
			ColumnHeaderOrdering();
			ParcelingData();
		}

		public List<ThItem> ThCollection { get; set; }
		public List<TrItem> TrCollection { get; set; }

		private ColumnOrder columnOrder = new ColumnOrder();
		private int columns = -1;
		private void ColumnHeaderOrdering()
		{
			// init TH collections;
			this.ThCollection = new List<ThItem>();

			int i = 4;
			foreach (AuthorListColumns col in this.DisplayedColumns)
			{
				ThItem th = null;
				switch (col)
				{
					// contact info [9]
					case AuthorListColumns.EmailAddress:
						th = new ThItem
						{
							Column = col,
							Text = Tokens.EmailAddress,
							Sortable = false,
						};
						columnOrder.EmailAddress = ++i;
						break;
					case AuthorListColumns.Phone:
						th = new ThItem
						{
							Column = col,
							Text = Tokens.Phone,
							Sortable = false,
						};
						columnOrder.Phone = ++i;
						break;
					case AuthorListColumns.Fax:
						th = new ThItem
						{
							Column = col,
							Text = Tokens.Fax,
							Sortable = false,
						};
						columnOrder.Fax = ++i;
						break;
					case AuthorListColumns.Address:
						th = new ThItem
						{
							Column = col,
							Text = Tokens.Address,
							Sortable = false,
						};
						columnOrder.Address = ++i;
						break;
					case AuthorListColumns.City:
						th = new ThItem
						{
							Column = col,
							Text = Tokens.City,
							SortableColumn = AuthorListSortColumns.City
						};
						columnOrder.City = ++i;
						break;
					case AuthorListColumns.State:
						th = new ThItem
						{
							Column = col,
							Text = Tokens.State,
							SortableColumn = AuthorListSortColumns.State
						};
						columnOrder.State = ++i;
						break;
					case AuthorListColumns.Country:
						th = new ThItem
						{
							Column = col,
							Text = Tokens.Country,
							SortableColumn = AuthorListSortColumns.Country
						};
						columnOrder.Country = ++i;
						break;
					case AuthorListColumns.Zip:
						th = new ThItem
						{
							Column = col,
							Text = Tokens.Zip,
							SortableColumn = AuthorListSortColumns.Zip
						};
						columnOrder.Zip = ++i;
						break;
					case AuthorListColumns.Language:
						th = new ThItem
						{
							Column = col,
							Text = Tokens.Language,
							SortableColumn = AuthorListSortColumns.Language
						};
						columnOrder.Language = ++i;
						break;
					// outlet info [8+3];
					case AuthorListColumns.OutletName:
						th = new ThItem
						{
							Column = col,
							Text = Tokens.OutletName,
							SortableColumn = AuthorListSortColumns.OutletName
						};
						columnOrder.OutletName = ++i;
						break;
					case AuthorListColumns.OutletEmploymentType:
						th = new ThItem
						{
							Column = col,
							Text = Tokens.OutletEmploymentType,
							SortableColumn = AuthorListSortColumns.OutletEmploymentType
						};
						columnOrder.OutletEmploymentType = ++i;
						break;
					case AuthorListColumns.OutletJobTitle:
						th = new ThItem
						{
							Column = col,
							Text = Tokens.OutletJobTitle,
							SortableColumn = AuthorListSortColumns.OutletJobTitle
						};
						columnOrder.OutletJobTitle = ++i;
						break;
					case AuthorListColumns.OutletType:
						th = new ThItem
						{
							Column = col,
							Text = Tokens.OutletType
						};
						columnOrder.OutletType = ++i;
						break;
					case AuthorListColumns.OutletCirculation:
						th = new ThItem
						{
							Column = col,
							ThClass = "dj_col-integer",
							Text = Tokens.OutletCirculation,
							SortableColumn = AuthorListSortColumns.OutletCirculation
						};
						columnOrder.OutletCirculation = ++i;
						break;
					case AuthorListColumns.OutletFrequency:
						th = new ThItem
						{
							Column = col,
							Text = Tokens.OutletFrequency,
							Sortable = false
						};
						columnOrder.OutletFrequency = ++i;
						break;
					case AuthorListColumns.OutletState:
						th = new ThItem
						{
							Column = col,
							Text = Tokens.OutletState,
							SortableColumn = AuthorListSortColumns.OutletState
						};
						columnOrder.OutletState = ++i;
						break;
					case AuthorListColumns.OutletCountry:
						th = new ThItem
						{
							Column = col,
							Text = Tokens.OutletCountry,
							SortableColumn = AuthorListSortColumns.OutletCountry
						};
						columnOrder.OutletCountry = ++i;
						break;
					case AuthorListColumns.OutletEmailAddress:
						th = new ThItem
						{
							Column = col,
							Text = Tokens.OutletEmail,
							Sortable = false,
						};
						columnOrder.OutletEmail = ++i;
						break;
					case AuthorListColumns.OutletPhone:
						th = new ThItem
						{
							Column = col,
							Text = Tokens.OutletPhone,
							Sortable = false,
						};
						columnOrder.OutletPhone = ++i;
						break;
					case AuthorListColumns.OutletFax:
						th = new ThItem
						{
							Column = col,
							Text = Tokens.OutletFax,
							Sortable = false,
						};
						columnOrder.OutletFax = ++i;
						break;
					// beats [2]
					case AuthorListColumns.BeatsSubjects:
						th = new ThItem
						{
							Column = col,
							Text = Tokens.BeatsSubjects,
							Sortable = false,
						};
						columnOrder.BeatsSubjects = ++i;
						break;
					case AuthorListColumns.BeatsIndustries:
						th = new ThItem
						{
							Column = col,
							Text = Tokens.BeatsIndustries,
							Sortable = false,
						};
						columnOrder.BeatsIndustries = ++i;
						break;
					// user info [1];
					case AuthorListColumns.UserInfo:
						th = new ThItem
						{
							Column = col,
							Text = Tokens.UserAddedContactInfo,
							Sortable = false,
						};
						columnOrder.UserInfo = ++i;
						break;
					case AuthorListColumns.None:
					default: // do nothing!
						break;
				}

				if (th != null)
				{
					this.ThCollection.Add(th);
				}
			}

			this.columns = ++i;

			// prepare sorting class
			string sortSpan = "<span class='dj_sortable-table-columnUp'></span>";
			if (this.SortOrder == OrderDirections.Descending)
			{
				sortSpan = "<span class='dj_sortable-table-columnDown'></span>";
			}

			var sortedColumn = from c in this.ThCollection
							   where c.SortableColumn == this.SortBy
							   select c;

			if (sortedColumn.Any())
			{
				((ThItem)sortedColumn.First()).SortedSpan = sortSpan;
			}
		}

		private void ParcelingData()
		{
			this.TrCollection = new List<TrItem>();
			uint runningSum = this.FirstRecordIndex;
			uint oddNumber = 0;
			foreach (AuthorModel author in this.Authors)
			{
				bool isOdd = ++oddNumber % 2 != 0;
				TdItem[] cols = new TdItem[this.columns];
				// check box;
				cols[0] = new TdItem
				{
					IsHtmlText = true,
					Text = String.Format(
						"<input name='dj_author-select' type='checkbox' authorlist-aid='{0}' authorlist-nnid='{1}' />",
						author.AuthorId,
						author.AuthorNNID),
					TdClass = "dj_col-checkbox"
				};
				// row numbers;
				cols[1] = new TdItem
				{
					Text = String.Format("{0}.", runningSum++),
					TdClass = "dj_col-row-id"
				};
				// row expander;
				cols[2] = new TdItem
				{
					IsHtmlText = true,
					TdClass = "dj_col-collapser",
					Text = "<div></div>"
				};

				if (this.AnyOutletRelatedColumn && author.ExpandableOutlet)
				{
					cols[2].Text = "<div class='dj_collapsable-icon collapsed'></div>";
				}

				// author name;
				cols[3] = new TdItem
				{
					Text = author.AuthorName,
					IsAncor = true,
					AncorHref = "javascript:void(0);",
					AncorClass = "author-name-selector"
				};
				// articles;
				if (author.HasArticles)
				{
					cols[4] = new TdItem
					{
						Text = this.Tokens.ViewArticles,
						IsAncor = true,
						AncorHref = "javascript:void(0);",
						AncorClass = "author-articles-selector"
					};
				}
				else
				{
					cols[4] = new TdItem
					{
						Text = this.Tokens.NoArticles
					};
				}
				// e-mail address;
				if (this.DisplayedColumns.Contains(AuthorListColumns.EmailAddress))
				{
					cols[columnOrder.EmailAddress] = new TdItem
					{
						Text = SeparateStringListWithDiv(author.EmailAddresses)
					};
				}
				// phone;
				if (this.DisplayedColumns.Contains(AuthorListColumns.Phone))
				{
					cols[columnOrder.Phone] = new TdItem
					{
						Text = SeparateStringListWithDiv(author.Phones)
					};
				}
				// fax;
				if (this.DisplayedColumns.Contains(AuthorListColumns.Fax))
				{
					cols[columnOrder.Fax] = new TdItem
					{
						Text = SeparateStringListWithDiv(author.Fax)
					};
				}
				// address;
				if (this.DisplayedColumns.Contains(AuthorListColumns.Address))
				{
					cols[columnOrder.Address] = new TdItem
					{
						Text = author.Address
					};
				}
				// city;
				if (this.DisplayedColumns.Contains(AuthorListColumns.City))
				{
					cols[columnOrder.City] = new TdItem
					{
						Text = author.City
					};
				}
				// state;
				if (this.DisplayedColumns.Contains(AuthorListColumns.State))
				{
					cols[columnOrder.State] = new TdItem
					{
						Text = author.State
					};
				}
				// country;
				if (this.DisplayedColumns.Contains(AuthorListColumns.Country))
				{
					cols[columnOrder.Country] = new TdItem
					{
						Text = author.Country
					};
				}
				// zip;
				if (this.DisplayedColumns.Contains(AuthorListColumns.Zip))
				{
					cols[columnOrder.Zip] = new TdItem
					{
						Text = author.Zip
					};
				}
				// language;
				if (this.DisplayedColumns.Contains(AuthorListColumns.Language))
				{
					cols[columnOrder.Language] = new TdItem
					{
						Text = SeparateStringListWithDiv(author.Language)
					};
				}
				// ***********************************
				// OUTLETS;
				// ***********************************
				OutletProperties firstOutlet = null;
				if (author.HasOutlets)
				{
					firstOutlet = author.Outlets.FirstOrDefault();
				}
				// outlet name;
				if (this.DisplayedColumns.Contains(AuthorListColumns.OutletName))
				{
					cols[columnOrder.OutletName] = new TdItem();
					if (author.HasOutlets)
					{
						cols[columnOrder.OutletName].Text = firstOutlet.Name;
						cols[columnOrder.OutletName].IsAncor = true;
						cols[columnOrder.OutletName].AncorClass = "author-outlet-selector";
						cols[columnOrder.OutletName].AncorHref = "javascript:void(0);";
						cols[columnOrder.OutletName].AncorAttributes = String.Format("authorlist-outlet-id='{0}'", firstOutlet.Id);
					}
					else
					{
						// only under outlet name column, no one else!!!
						cols[columnOrder.OutletName].Text = this.Tokens.NoCurrentOutlet;
					}
				}
				// outlet employnment type;
				if (this.DisplayedColumns.Contains(AuthorListColumns.OutletEmploymentType))
				{
					cols[columnOrder.OutletEmploymentType] = new TdItem();
					if (author.HasOutlets)
					{
						cols[columnOrder.OutletEmploymentType].Text = firstOutlet.EmploymentType;
					}
				}
				// outlet job title;
				if (this.DisplayedColumns.Contains(AuthorListColumns.OutletJobTitle))
				{
					cols[columnOrder.OutletJobTitle] = new TdItem();
					if (author.HasOutlets)
					{
						cols[columnOrder.OutletJobTitle].Text = firstOutlet.JobTitle;
					}
				}
				// outlet job title;
				if (this.DisplayedColumns.Contains(AuthorListColumns.OutletJobTitle))
				{
					cols[columnOrder.OutletJobTitle] = new TdItem();
					if (author.HasOutlets)
					{
						cols[columnOrder.OutletJobTitle].Text = firstOutlet.JobTitle;
					}
				}
				// outlet type;
				if (this.DisplayedColumns.Contains(AuthorListColumns.OutletType))
				{
					cols[columnOrder.OutletType] = new TdItem();
					if (author.HasOutlets)
					{
						cols[columnOrder.OutletType].Text = SeparateStringListWithDiv(firstOutlet.Type);
					}
				}
				// outlet circulation;
				if (this.DisplayedColumns.Contains(AuthorListColumns.OutletCirculation))
				{
					cols[columnOrder.OutletCirculation] = new TdItem();
					if (author.HasOutlets)
					{
						cols[columnOrder.OutletCirculation].TdClass = "dj_col-integer";
						if (firstOutlet.Circulation > 0)
						{
							cols[columnOrder.OutletCirculation].Text = firstOutlet.Circulation.ToString(
								"0,0",
								System.Globalization.CultureInfo.InvariantCulture);
						}
					};
				}
				// outlet frequency;
				if (this.DisplayedColumns.Contains(AuthorListColumns.OutletFrequency))
				{
					cols[columnOrder.OutletFrequency] = new TdItem();
					if (author.HasOutlets)
					{
						cols[columnOrder.OutletFrequency].Text = firstOutlet.Frequency;
					};
				}
				// outlet state;
				if (this.DisplayedColumns.Contains(AuthorListColumns.OutletState))
				{
					cols[columnOrder.OutletState] = new TdItem();
					if (author.HasOutlets)
					{
						cols[columnOrder.OutletState].Text = firstOutlet.State;
					}
				}
				// outlet country;
				if (this.DisplayedColumns.Contains(AuthorListColumns.OutletCountry))
				{
					cols[columnOrder.OutletCountry] = new TdItem();
					if (author.HasOutlets)
					{
						cols[columnOrder.OutletCountry].Text = firstOutlet.Country;
					}
				}
				// outlet e-mail address;
				if (this.DisplayedColumns.Contains(AuthorListColumns.OutletEmailAddress))
				{
					cols[columnOrder.OutletEmail] = new TdItem();
					if (author.HasOutlets)
					{
						cols[columnOrder.OutletEmail].Text =
							SeparateStringListWithDiv(firstOutlet.EmailAddresses);
					};
				}
				// outlet phone;
				if (this.DisplayedColumns.Contains(AuthorListColumns.OutletPhone))
				{
					cols[columnOrder.OutletPhone] = new TdItem();
					if (author.HasOutlets)
					{
						cols[columnOrder.OutletPhone].Text = SeparateStringListWithDiv(firstOutlet.Phones);
					};
				}
				// outlet fax;
				if (this.DisplayedColumns.Contains(AuthorListColumns.OutletFax))
				{
					cols[columnOrder.OutletFax] = new TdItem();
					if (author.HasOutlets)
					{
						cols[columnOrder.OutletFax].Text = SeparateStringListWithDiv(firstOutlet.Faxes);
					};
				}
				// ***********************************
				// BEATS;
				// ***********************************
				// subject;
				if (this.DisplayedColumns.Contains(AuthorListColumns.BeatsSubjects))
				{
					cols[columnOrder.BeatsSubjects] = new TdItem
					{
						IsHtmlText = true,
						Text = ParcelListOfStrings(author.BeatsSubjects)
					};
				}
				// industries;
				if (this.DisplayedColumns.Contains(AuthorListColumns.BeatsIndustries))
				{
					cols[columnOrder.BeatsIndustries] = new TdItem
					{
						IsHtmlText = true,
						Text = ParcelListOfStrings(author.BeatsIndustries)
					};
				}
				// ***********************************
				// USER INFO;
				// ***********************************
				if (this.DisplayedColumns.Contains(AuthorListColumns.UserInfo))
				{
					cols[columnOrder.UserInfo] = new TdItem
					{
						Text = author.CreateTextFromUserAddedContactInfo()
					};
				}

				TrItem tr = new TrItem(cols, isOdd);
				this.TrCollection.Add(tr);
				// ************************************
				// additional outlets info
				// ************************************
				if (this.AnyOutletRelatedColumn && author.ExpandableOutlet)
				{
					// add second, third, ... , n-th outlet info rows;
					foreach (OutletProperties outlet in author.Outlets.Skip(1))
					{
						TdItem[] outletRows = new TdItem[this.columns];
						for (int x = 0; x < this.columns; ++x)
						{
							outletRows[x] = new TdItem();
						}

						// outlet name;
						if (this.DisplayedColumns.Contains(AuthorListColumns.OutletName))
						{
							outletRows[columnOrder.OutletName] = new TdItem();
							outletRows[columnOrder.OutletName].Text = outlet.Name;
							outletRows[columnOrder.OutletName].IsAncor = true;
							outletRows[columnOrder.OutletName].AncorClass = "author-outlet-selector";
							outletRows[columnOrder.OutletName].AncorHref = "javascript:void(0);";
							outletRows[columnOrder.OutletName].AncorAttributes =
								String.Format("authorlist-outlet-id='{0}'", outlet.Id);
						}
						// outlet employnment type;
						if (this.DisplayedColumns.Contains(AuthorListColumns.OutletEmploymentType))
						{
							outletRows[columnOrder.OutletEmploymentType] = new TdItem();
							outletRows[columnOrder.OutletEmploymentType].Text = outlet.EmploymentType;
						}
						// outlet job title;
						if (this.DisplayedColumns.Contains(AuthorListColumns.OutletJobTitle))
						{
							outletRows[columnOrder.OutletJobTitle] = new TdItem();
							outletRows[columnOrder.OutletJobTitle].Text = outlet.JobTitle;
						}
						// outlet job title;
						if (this.DisplayedColumns.Contains(AuthorListColumns.OutletJobTitle))
						{
							outletRows[columnOrder.OutletJobTitle] = new TdItem();
							outletRows[columnOrder.OutletJobTitle].Text = outlet.JobTitle;
						}
						// outlet type;
						if (this.DisplayedColumns.Contains(AuthorListColumns.OutletType))
						{
							outletRows[columnOrder.OutletType] = new TdItem();
							outletRows[columnOrder.OutletType].Text 
								= SeparateStringListWithDiv(outlet.Type);
						}
						// outlet circulation;
						if (this.DisplayedColumns.Contains(AuthorListColumns.OutletCirculation))
						{
							outletRows[columnOrder.OutletCirculation] = new TdItem();
							outletRows[columnOrder.OutletCirculation].TdClass = "dj_col-integer";
							if (outlet.Circulation > 0)
							{
								outletRows[columnOrder.OutletCirculation].Text = outlet.Circulation.ToString(
									"0,0",
									System.Globalization.CultureInfo.InvariantCulture);
							}
						}
						// outlet frequency;
						if (this.DisplayedColumns.Contains(AuthorListColumns.OutletFrequency))
						{
							outletRows[columnOrder.OutletFrequency] = new TdItem();
							outletRows[columnOrder.OutletFrequency].Text = outlet.Frequency;
						}
						// outlet state;
						if (this.DisplayedColumns.Contains(AuthorListColumns.OutletState))
						{
							outletRows[columnOrder.OutletState] = new TdItem();
							outletRows[columnOrder.OutletState].Text = outlet.State;
						}
						// outlet country;
						if (this.DisplayedColumns.Contains(AuthorListColumns.OutletCountry))
						{
							outletRows[columnOrder.OutletCountry] = new TdItem();
							outletRows[columnOrder.OutletCountry].Text = outlet.Country;
						}
						// outlet e-mail address;
						if (this.DisplayedColumns.Contains(AuthorListColumns.OutletEmailAddress))
						{
							outletRows[columnOrder.OutletEmail] = new TdItem();
							outletRows[columnOrder.OutletEmail].Text =
								SeparateStringListWithDiv(outlet.EmailAddresses);
						}
						// outlet phone;
						if (this.DisplayedColumns.Contains(AuthorListColumns.OutletPhone))
						{
							outletRows[columnOrder.OutletPhone] = new TdItem();
							outletRows[columnOrder.OutletPhone].Text =
								SeparateStringListWithDiv(outlet.Phones);
						}
						// outlet fax;
						if (this.DisplayedColumns.Contains(AuthorListColumns.OutletFax))
						{
							outletRows[columnOrder.OutletFax] = new TdItem();
							outletRows[columnOrder.OutletFax].Text =
								SeparateStringListWithDiv(outlet.Faxes);
						}

						tr = new TrItem(outletRows, isOdd, true);
						this.TrCollection.Add(tr);
					}
				}
			}
		}

		private string ParcelListOfStrings(List<string> collection)
		{
			string retval = String.Empty;

			if (collection == null || collection.Any() == false)
			{
				return retval;
			}

			retval = String.Join("", from s in collection.Take(2)
									 select String.Format("<div>{0}</div>", s));

			if (collection.Count > 2)
			{
				retval += String.Join("", from s in collection.Skip(2)
										  select String.Format("<div class='dj_hidden-cell-item hide'>{0}</div>", s));
				retval += String.Format(
					"<div><a href='javascript:void(0);' class='dj_show-hide-cell-items' more='true'>{0}</a></div>",
					this.Tokens.ShowCellItems);
			}

			return retval;
		}

		private string SeparateStringListWithDiv(List<string> collection)
		{
			string retval = String.Empty;

			if (collection == null || collection.Any() == false)
			{
				return retval;
			}

			retval = String.Join("", from s in collection
									 select String.Format("<div>{0}</div>", s));

			return retval;
		}

		private IEnumerable<SelectListItem> GetActionMenuItems()
		{
			// Define the action menu;
			return new[] {
				new SelectListItem { Text = this.Tokens.AddToContactList, Value = "contact-list" },
				new SelectListItem { Text = this.Tokens.CreateActivity, Value = "create-activity" },
				new SelectListItem { Text = this.Tokens.CreateAlert, Value = "create-alert" },
				new SelectListItem { Text = this.Tokens.CreateBriefingBook, Value = "create-briefing-book" },
				new SelectListItem { Text = this.Tokens.Print, Value = "print" },
				new SelectListItem { Text = this.Tokens.Export, Value = "export" },
				new SelectListItem { Text = this.Tokens.ExportAll, Value = "export-all" },
				new SelectListItem { Text = this.Tokens.DistributeList, Value = "distribute-list" },
				new SelectListItem { Text = this.Tokens.Delete, Value = "delete" },
				new SelectListItem { Text = this.Tokens.Email, Value = "email" }
			};
		}
	}

	/// <summary>
	/// Author's sortable columns enumerator.
	/// </summary>
	public enum AuthorListSortColumns
	{
		None = 0,
		[Description("name")]
		ContactName = 1,
		[Description("city")]
		City = 2,
		[Description("state")]
		State = 3,
		[Description("country")]
		Country = 4,
		[Description("zip")]
		Zip = 5,
		[Description("language")]
		Language = 6,
		[Description("outlet-name")]
		OutletName = 7,
		[Description("outlet-employment-type")]
		OutletEmploymentType = 8,
		[Description("outlet-job-title")]
		OutletJobTitle = 9,
		[Description("outlet-circulation")]
		OutletCirculation = 10,
		[Description("outlet-state")]
		OutletState = 11,
		[Description("outlet-country")]
		OutletCountry = 12,
	}

	public enum AuthorListColumns
	{
		None = 0,
		// contact info [9];
		EmailAddress,
		Phone,
		Fax,
		Address,
		City,
		State,
		Country,
		Zip,
		Language,
		// outlet info [8+3];
		OutletName,
		OutletEmploymentType,
		OutletJobTitle,
		OutletType,
		OutletCirculation,
		OutletFrequency,
		OutletState,
		OutletCountry,
		OutletEmailAddress,
		OutletPhone,
		OutletFax,
		// beats [2];
		BeatsSubjects,
		BeatsIndustries,
		// user added info [1];
		UserInfo
	}

	/// <summary>
	/// Sort order enumerator.
	/// </summary>
	public enum OrderDirections
	{
		None = 0,

		/// <summary>
		/// Order ascending;
		/// </summary>
		[Description("asc")]
		Ascending = 1,

		/// <summary>
		/// Order descending;
		/// </summary>
		[Description("desc")]
		Descending = 3
	}

	public struct ColumnOrder
	{
		// contact info [9];
		public int EmailAddress;
		public int Phone;
		public int Fax;
		public int Address;
		public int City;
		public int State;
		public int Country;
		public int Zip;
		public int Language;
		// outlet info [8+3];
		public int OutletName;
		public int OutletEmploymentType;
		public int OutletJobTitle;
		public int OutletType;
		public int OutletCirculation;
		public int OutletFrequency;
		public int OutletState;
		public int OutletCountry;
		public int OutletEmail;
		public int OutletPhone;
		public int OutletFax;
		// beats [2];
		public int BeatsSubjects;
		public int BeatsIndustries;
		// user added info [1];
		public int UserInfo;
	}

	public class ThItem
	{
		const string COLUMN_TH_STYLE_SORTABLE = "dj_sortable-table-header";
		const string COLUMN_TH_STYLE_NONSORTABLE = "dj_sortable-table-column";
		const string COLUMN_STRING = "dj_col-string";
		const string COLUMN_INTEGER = "dj_col-integer";

		public AuthorListColumns Column { get; set; }

		private string thClass = COLUMN_STRING;
		public string ThClass
		{
			get
			{
				return String.Format(
					"{0} {1}", 
					this.thClass,
					this.Sortable ? COLUMN_TH_STYLE_SORTABLE : COLUMN_TH_STYLE_NONSORTABLE);
			}
			set { this.thClass = value; }
		}
		public string Text { get; set; }
		public bool IsTextSplitable { get; set; }
		public bool Sortable { get; set; }
		public AuthorListSortColumns SortableColumn { get; set; }
		public string SortableAttribut 
		{
			get
			{
				string retval = String.Empty;
				if (this.Sortable && this.SortableColumn != AuthorListSortColumns.None)
				{
					retval = EnumDescription.StringValueOf(this.SortableColumn);
				}

				return retval;
			}
		}
		public string SortedSpan { get; set; }

		public ThItem()
		{
			this.IsTextSplitable = false;
			this.Sortable = true;
			this.SortedSpan = String.Empty;
		}
	}

	public class TrItem
	{
		public TdItem[] TdItems { get; set; }
		public bool IsHidden { get; set; }
		public bool IsOddRow { get; set; }
		public string TrClass
		{
			get 
			{
				string retval = String.Empty;
				if (this.IsHidden)
				{
					retval = "outlet-other";
					if (this.IsOddRow)
					{
						retval += " odd";
					}

					retval += " hide";
				}
				else
				{
					if (this.IsOddRow)
					{
						retval = "odd";
					}
				}

				return retval;
			}
		}

		public TrItem(TdItem[] tdItems, bool isOddRow, bool isHidden)
		{
			this.TdItems = tdItems;
			this.IsOddRow = isOddRow;
			this.IsHidden = isHidden;
		}

		public TrItem(TdItem[] tdItems, bool isOddRow)
			: this(tdItems, isOddRow, false)
		{
		}
	}

	public class TdItem
	{
		const string COLUMN_STRING = "dj_col-string";
		const string COLUMN_INTEGER = "dj_col-integer";

		public string TdClass { get; set; }

		private string tdAttributes = String.Empty;
		public string TdAttributes
		{
			get
			{
				string attr = String.Empty;
				if (String.IsNullOrEmpty(this.tdAttributes.Trim()) == false)
				{
					if (this.tdAttributes.StartsWith(" ") == false)
					{
						attr = String.Format(" {0}", this.tdAttributes);
					}
				}

				return attr;
			}
			set { this.tdAttributes = value; }
		}
		public string Text { get; set; }
		public bool IsHtmlText { get; set; }
		public bool IsAncor { get; set; }
		public string AncorHref { get; set; }
		public string AncorClass { get; set; }
		public string AncorAttributes { get; set; }

		public TdItem()
		{
			this.Text = String.Empty;
			this.IsHtmlText = false;
			this.IsAncor = false;
			this.TdClass = COLUMN_STRING;
		}
	}
}