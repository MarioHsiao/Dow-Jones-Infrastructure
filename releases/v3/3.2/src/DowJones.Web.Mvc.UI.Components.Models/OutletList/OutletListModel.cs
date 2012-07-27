using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using System;

namespace DowJones.Web.Mvc.UI.Components.Models
{
	public class OutletListModel : ViewComponentModel
	{
		public IEnumerable<OutletModel> Outlets { get; set; }
		public OutletListTokens Tokens { get; set; }
		public uint FirstRecordIndex { get; set; }
		public OutletListSortColumns SortBy { get; set; }
		public OrderDirections SortOrder { get; set; }
		[ClientProperty("pageSize")]
		public int PageSize { get; set; }
		public bool ShowDeleteAction { get; set; }
		public bool ShowDistributeListAction { get; set; }
		public List<OutletListColumns> DisplayedColumns { get; set; }

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

		[ClientData("selectedOutletIds")]
		public string SelectedOutletIds { get; set; }

		public void InitializeDomDataCollections()
		{
			ColumnHeaderOrdering();
			ParcelingData();
		}

		public List<ThOutletItem> ThCollection { get; set; }
		public List<TrItem> TrCollection { get; set; }

		private OutletColumnOrder columnOrder = new OutletColumnOrder();
		private int columns = -1;
		private void ColumnHeaderOrdering()
		{
			// init TH collections;
			this.ThCollection = new List<ThOutletItem>();

			int i = 4;
			foreach (OutletListColumns col in this.DisplayedColumns)
			{
				ThOutletItem th = null;

				switch (col)
				{
					case OutletListColumns.Circulation:
						th = new ThOutletItem
						{
							Column = col,
							Text = Tokens.Circulation,
							SortableColumn = OutletListSortColumns.Circulation
						};
						columnOrder.Circulation = ++i;
						break;
					case OutletListColumns.Type:
						th = new ThOutletItem
						{
							Column = col,
							Text = Tokens.Type
						};
						columnOrder.Type = ++i;
						break;
					case OutletListColumns.Website:
						th = new ThOutletItem
						{
							Column = col,
							Text = Tokens.Website,
							Sortable = false
						};
						columnOrder.Website = ++i;
						break;
					case OutletListColumns.Address:
						th = new ThOutletItem
						{
							Column = col,
							Text = Tokens.Address,
							Sortable = false
						};
						columnOrder.Address = ++i;
						break;
					case OutletListColumns.City:
						th = new ThOutletItem
						{
							Column = col,
							Text = Tokens.City,
							SortableColumn = OutletListSortColumns.City
						};
						columnOrder.City = ++i;
						break;
					case OutletListColumns.State:
						th = new ThOutletItem
						{
							Column = col,
							Text = Tokens.State,
							SortableColumn = OutletListSortColumns.State
						};
						columnOrder.State = ++i;
						break;
					case OutletListColumns.Zip:
						th = new ThOutletItem
						{
							Column = col,
							Text = Tokens.Zip,
							SortableColumn = OutletListSortColumns.Zip
						};
						columnOrder.Zip = ++i;
						break;
					case OutletListColumns.Country:
						th = new ThOutletItem
						{
							Column = col,
							Text = Tokens.Country,
							SortableColumn = OutletListSortColumns.Country
						};
						columnOrder.Country = ++i;
						break;
					case OutletListColumns.Email:
						th = new ThOutletItem
						{
							Column = col,
							Text = Tokens.EmailAddress,
							Sortable = false
						};
						columnOrder.Email = ++i;
						break;
					case OutletListColumns.Phone:
						th = new ThOutletItem
						{
							Column = col,
							Text = Tokens.Phone,
							Sortable = false
						};
						columnOrder.Phone = ++i;
						break;
					case OutletListColumns.Fax:
						th = new ThOutletItem
						{
							Column = col,
							Text = Tokens.Fax,
							Sortable = false
						};
						columnOrder.Fax = ++i;
						break;
					case OutletListColumns.Industries:
						th = new ThOutletItem
						{
							Column = col,
							Text = Tokens.Industries,
							Sortable = false
						};
						columnOrder.Industries = ++i;
						break;
					case OutletListColumns.Subjects:
						th = new ThOutletItem
						{
							Column = col,
							Text = Tokens.Subjects,
							Sortable = false
						};
						columnOrder.Subjects = ++i;
						break;
					case OutletListColumns.Regions:
						th = new ThOutletItem
						{
							Column = col,
							Text = Tokens.Regions,
							Sortable = false
						};
						columnOrder.Regions = ++i;
						break;
					case OutletListColumns.Language:
						th = new ThOutletItem
						{
							Column = col,
							Text = Tokens.Language,
							SortableColumn = OutletListSortColumns.Language
						};
						columnOrder.Language = ++i;
						break;
					case OutletListColumns.Frequency:
						th = new ThOutletItem
						{
							Column = col,
							Text = Tokens.Frequency,
							SortableColumn = OutletListSortColumns.Frequency
						};
						columnOrder.Frequency = ++i;
						break;
					case OutletListColumns.Coverage:
						th = new ThOutletItem
						{
							Column = col,
							Text = Tokens.Coverage,
							SortableColumn = OutletListSortColumns.Coverage
						};
						columnOrder.Coverage = ++i;
						break;
					case OutletListColumns.Audience:
						th = new ThOutletItem
						{
							Column = col,
							Text = Tokens.Audience,
							Sortable = false
						};
						columnOrder.Audience = ++i;
						break;
					case OutletListColumns.Publisher:
						th = new ThOutletItem
						{
							Column = col,
							Text = Tokens.Publisher,
							SortableColumn = OutletListSortColumns.Publisher
						};
						columnOrder.Publisher = ++i;
						break;
					case OutletListColumns.UserInfo:
						th = new ThOutletItem
						{
							Column = col,
							Text = Tokens.UserAddedInfo,
							Sortable = false
						};
						columnOrder.UserInfo = ++i;
						break;
					case OutletListColumns.None:
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
				((ThOutletItem)sortedColumn.First()).SortedSpan = sortSpan;
			}
		}

		private void ParcelingData()
		{
			this.TrCollection = new List<TrItem>();
			uint runningSum = this.FirstRecordIndex;
			uint oddNumber = 0;
			foreach (OutletModel outlet in this.Outlets)
			{
				bool isOdd = ++oddNumber % 2 != 0;
				TdItem[] cols = new TdItem[this.columns];
				// check box;
				cols[0] = new TdItem
				{
					IsHtmlText = true,
					Text = String.Format(
						"<input name='dj_outlet-select' type='checkbox' outletlist-aid='{0}' />",
						outlet.OutletId),
					TdClass = "dj_col-checkbox"
				};
				// row numbers;
				cols[1] = new TdItem
				{
					Text = String.Format("{0}.", runningSum++),
					TdClass = "dj_col-row-id"
				};
				// outlet name;
				cols[2] = new TdItem
				{
					Text = outlet.OutletName,
					TdClass = "dj_col-string ellipsis",
					TdAttributes = "nowrap='nowrap'",
					IsAncor = true,
					AncorHref = "javascript:void(0);",
					AncorClass = "outlet-selector"
				};
				// media contacts
				string withContacts = String.Format(
					"<span class='dj_icon dj_list-icon dj_list-icon-has-contacts' title='{0}'></span>",
					this.Tokens.ViewMediaContacts);
				if (outlet.HasMediaContacts == false)
				{
					withContacts = String.Format(
						"<span class='dj_icon dj_list-icon dj_list-icon-no-contacts' title='{0}'></span>",
						this.Tokens.SendMailToMediaLab);
				}
				cols[3] = new TdItem
				{
					Text = withContacts,
					IsHtmlText = true
				};
				// articles;
				cols[4] = new TdItem
				{
					Text = this.Tokens.ViewArticles,
					IsAncor = true,
					AncorHref = "javascript:void(0);",
					AncorClass = "outlet-articles-selector"
				};
				// circulation;
				if (this.DisplayedColumns.Contains(OutletListColumns.Circulation))
				{
					cols[columnOrder.Circulation] = new TdItem
					{
						TdClass = "dj_col-integer",
					};
					if (outlet.Circulation > 0)
					{
						cols[columnOrder.Circulation].Text = outlet.Circulation.ToString(
							"0,0",
							System.Globalization.CultureInfo.InvariantCulture);
					}
				}
				// type;
				if (this.DisplayedColumns.Contains(OutletListColumns.Type))
				{
					cols[columnOrder.Type] = new TdItem
					{
						Text = SeparateStringListWithDiv(outlet.Type),
						TdClass = "dj_col-string ellipsis",
						TdAttributes = "nowrap='nowrap'"
					};
				}
				// website;
				if (this.DisplayedColumns.Contains(OutletListColumns.Website))
				{
					cols[columnOrder.Website] = new TdItem
					{
						IsAncor = true,
						AncorHref = outlet.WebSite,
						AncorAttributes = "target='_blank'",
						Text = outlet.WebSite
					};
				}
				// address;
				if (this.DisplayedColumns.Contains(OutletListColumns.Address))
				{
					cols[columnOrder.Address] = new TdItem
					{
						Text = outlet.Address
					};
				}
				// city;
				if (this.DisplayedColumns.Contains(OutletListColumns.City))
				{
					cols[columnOrder.City] = new TdItem
					{
						Text = outlet.City
					};
				}
				// state;
				if (this.DisplayedColumns.Contains(OutletListColumns.State))
				{
					cols[columnOrder.State] = new TdItem
					{
						Text = outlet.State
					};
				}
				// zip;
				if (this.DisplayedColumns.Contains(OutletListColumns.Zip))
				{
					cols[columnOrder.Zip] = new TdItem
					{
						Text = outlet.Zip
					};
				}
				// country;
				if (this.DisplayedColumns.Contains(OutletListColumns.Country))
				{
					cols[columnOrder.Country] = new TdItem
					{
						Text = outlet.Country
					};
				}
				// e-mail;
				if (this.DisplayedColumns.Contains(OutletListColumns.Email))
				{
					cols[columnOrder.Email] = new TdItem
					{
						Text = SeparateStringListWithDiv(outlet.Emails),
						TdAttributes = "nowrap='nowrap'",
						IsHtmlText = true
					};
				}
				// phone;
				if (this.DisplayedColumns.Contains(OutletListColumns.Phone))
				{
					cols[columnOrder.Phone] = new TdItem
					{
						Text = SeparateStringListWithDiv(outlet.Phones),
						TdAttributes = "nowrap='nowrap'",
						IsHtmlText = true
					};
				}
				// fax;
				if (this.DisplayedColumns.Contains(OutletListColumns.Fax))
				{
					cols[columnOrder.Fax] = new TdItem
					{
						Text = SeparateStringListWithDiv(outlet.Faxes),
						TdAttributes = "nowrap='nowrap'",
						IsHtmlText = true
					};
				}
				// industries;
				if (this.DisplayedColumns.Contains(OutletListColumns.Industries))
				{
					cols[columnOrder.Industries] = new TdItem
					{
						IsHtmlText = true,
						Text = ParcelListOfStrings(outlet.Industries),
						TdAttributes = "nowrap='nowrap'"
					};
				}
				// subjects;
				if (this.DisplayedColumns.Contains(OutletListColumns.Subjects))
				{
					cols[columnOrder.Subjects] = new TdItem
					{
						IsHtmlText = true,
						Text = ParcelListOfStrings(outlet.Subjects),
						TdAttributes = "nowrap='nowrap'"
					};
				}
				// regions;
				if (this.DisplayedColumns.Contains(OutletListColumns.Regions))
				{
					cols[columnOrder.Regions] = new TdItem
					{
						IsHtmlText = true,
						Text = ParcelListOfStrings(outlet.Regions),
						TdAttributes = "nowrap='nowrap'"
					};
				}
				// language;
				if (this.DisplayedColumns.Contains(OutletListColumns.Language))
				{
					cols[columnOrder.Language] = new TdItem
					{
						Text = SeparateStringListWithDiv(outlet.Languages)
					};
				}
				// frequence;
				if (this.DisplayedColumns.Contains(OutletListColumns.Frequency))
				{
					cols[columnOrder.Frequency] = new TdItem
					{
						Text = outlet.Frequency
					};
				}
				// coverage;
				if (this.DisplayedColumns.Contains(OutletListColumns.Coverage))
				{
					cols[columnOrder.Coverage] = new TdItem
					{
						Text = outlet.Coverage
					};
				}
				// audience;
				if (this.DisplayedColumns.Contains(OutletListColumns.Audience))
				{
					cols[columnOrder.Audience] = new TdItem
					{
						Text = String.Join("<br />", outlet.Audiences),
						TdClass = "dj_col-string ellipsis",
						TdAttributes = "nowrap='nowrap'"
					};
				}
				// publisher;
				if (this.DisplayedColumns.Contains(OutletListColumns.Publisher))
				{
					cols[columnOrder.Publisher] = new TdItem
					{
						Text = outlet.Publisher
					};
				}
				// user added information;
				if (this.DisplayedColumns.Contains(OutletListColumns.UserInfo))
				{
					cols[columnOrder.UserInfo] = new TdItem
					{
						Text = outlet.CreateTextFromUserAddedContactInfo()
					};
				}

				TrItem tr = new TrItem(cols, isOdd);
				this.TrCollection.Add(tr);
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
									 select String.Format("<div class='ellipsis' title='{0}'>{0}</div>", s));

			if (collection.Count > 2)
			{
				retval += String.Join("", from s in collection.Skip(2)
										  select String.Format("<div class='dj_hidden-cell-item ellipsis hide' title='{0}'>{0}</div>", s));
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
									 select String.Format("<div class='ellipsis' title='{0}'>{0}</div>", s));

			return retval;
		}

		/// <summary>
		/// Create an instanse of OutletListModel class.
		/// </summary>
		public OutletListModel()
		{
			this.Outlets = Enumerable.Empty<OutletModel>();
			this.Tokens = new OutletListTokens();
			this.ShowDeleteAction = false;
			this.SortBy = OutletListSortColumns.OutletName;
			this.SortOrder = OrderDirections.Ascending;
			this.DisplayedColumns = new List<OutletListColumns>();
		}

		private IEnumerable<SelectListItem> GetActionMenuItems()
		{
			return new[] {
				new SelectListItem { Text = this.Tokens.AddToContactList, Value = "contact-list" },
				new SelectListItem { Text = this.Tokens.Print, Value = "print" },
				new SelectListItem { Text = this.Tokens.Export, Value = "export" },
				new SelectListItem { Text = this.Tokens.ExportAll, Value = "export-all" },
				new SelectListItem { Text = this.Tokens.DistributeList, Value = "distribute-list" },
				new SelectListItem { Text = this.Tokens.Delete, Value = "delete" },
				new SelectListItem { Text = this.Tokens.Email, Value = "email" }
			};
		}
	}

	public enum OutletListColumns
	{
		None = 0,
		// About;
		Circulation,
		Type,
		Website,
		// Contact Information;
		Address,
		City,
		State,
		Zip,
		Country,
		Email,
		Phone,
		Fax,
		// Editorial focus;
		Industries,
		Subjects,
		Regions,
		// Aditional metadata;
		Language,
		Frequency,
		Coverage,
		Audience,
		Publisher,
		// user added info;
		UserInfo
	}

	public struct OutletColumnOrder
	{
		// About;
		public int Circulation;
		public int Type;
		public int Website;
		// Contact Information;
		public int Address;
		public int City;
		public int State;
		public int Zip;
		public int Country;
		public int Email;
		public int Phone;
		public int Fax;
		// Editorial focus;
		public int Industries;
		public int Subjects;
		public int Regions;
		// Aditional metadata;
		public int Language;
		public int Frequency;
		public int Coverage;
		public int Audience;
		public int Publisher;
		// user added info;
		public int UserInfo;

	}


	/// <summary>
	/// Outlet's sortable columns enumerator.
	/// </summary>
	public enum OutletListSortColumns
	{
		None = 0,
		[Description("outlet")]
		OutletName,
		[Description("circulation")]
		Circulation,
		[Description("city")]
		City,
		[Description("state")]
		State,
		[Description("zip")]
		Zip,
		[Description("country")]
		Country,
		[Description("language")]
		Language,
		[Description("frequency")]
		Frequency,
		[Description("coverage")]
		Coverage,
		[Description("publisher")]
		Publisher
	}

	public class ThOutletItem
	{
		const string COLUMN_TH_STYLE_SORTABLE = "dj_sortable-table-header";
		const string COLUMN_TH_STYLE_NONSORTABLE = "dj_sortable-table-column";
		const string COLUMN_STRING = "dj_col-string";
		const string COLUMN_INTEGER = "dj_col-integer";

		public OutletListColumns Column { get; set; }

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
		public OutletListSortColumns SortableColumn { get; set; }
		public string SortableAttribut
		{
			get
			{
				string retval = String.Empty;
				if (this.Sortable && this.SortableColumn != OutletListSortColumns.None)
				{
					retval = EnumDescription.StringValueOf(this.SortableColumn);
				}

				return retval;
			}
		}
		public string SortedSpan { get; set; }

		public ThOutletItem()
		{
			this.IsTextSplitable = false;
			this.Sortable = true;
			this.SortedSpan = String.Empty;
		}
	}
}