using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace DowJones.Web.Mvc.UI.Components.Models
{
	public class OutletListModel : ViewComponentModel
	{
		/// <summary>
		/// Gets and sets outlet list.
		/// </summary>
		public IEnumerable<OutletModel> Outlets { get; set; }

		/// <summary>
		/// Gets and sets first author index.
		/// Used in table to show a serial number.
		/// </summary>
		public uint FirstRecordIndex { get; set; }

		/// <summary>
		/// Gets and sets tokens to be used in OutletList component.
		/// </summary>
		public OutletListTokens Tokens { get; set; }

		/// <summary>
		/// Gets and sets column by which the author list will be sorted.
		/// </summary>
		public OutletListSortColumns SortBy { get; set; }

		/// <summary>
		/// Gets and sets order direction for sorting the outlet list.
		/// </summary>
		public OrderDirections SortOrder { get; set; }
		// by rss @ 20120119 begin
		[ClientProperty("pageSize")]
		public int PageSize { get; set; }
		// by rss @ 20120119 end

		public bool ShowDeleteAction { get; set; }

		public List<OutletListSortColumns> DisplayedColumns { get; set; }

		public IEnumerable<SelectListItem> Actions
		{
			get
			{
				IEnumerable<SelectListItem> all = GetActionMenuItems();
				if (this.ShowDeleteAction)
				{
					return all;
				}
				else
				{
					var woDelete = from a in all
								   where a.Value.Equals("delete") == false
								   select a;
					return woDelete;
				}
			}
		}

		[ClientData("selectedOutletIds")]
		public string SelectedOutletIds { get; set; }

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
			this.DisplayedColumns = new List<OutletListSortColumns>();
		}

		private IEnumerable<SelectListItem> GetActionMenuItems()
		{
			return new[] {
				new SelectListItem { Text = this.Tokens.AddToContactList, Value = "contact-list" },
				new SelectListItem { Text = this.Tokens.Print, Value = "print" },
				new SelectListItem { Text = this.Tokens.Export, Value = "export" },
				new SelectListItem { Text = this.Tokens.ExportAll, Value = "export-all" },
				new SelectListItem { Text = this.Tokens.Delete, Value = "delete" },
				new SelectListItem { Text = this.Tokens.Email, Value = "email" },
				new SelectListItem { Text = this.Tokens.UnselectAll, Value = "unselect-all" }
			};
		}
	}

	/// <summary>
	/// Outlet's sortable columns enumerator.
	/// </summary>
	public enum OutletListSortColumns
	{
		None = 0,

		/// <summary>
		/// Sort by outlet name;
		/// </summary>
		[Description("outlet")]
		OutletName = 1,

				/// <summary>
		/// Sort by circulation;
		/// </summary>
		[Description("circulation")]
		Circulation = 2,

		/// <summary>
		/// Sort by country;
		/// </summary>
		[Description("country")]
		Country = 3,

		 /// <summary>
		/// Sort by media format;
		/// </summary>
		[Description("mediaformat")]
		MediaFormat = 4,

		/// <summary>
		/// Sort by outlet type;
		/// </summary>
		[Description("outlettype")]
		OutletType = 5,

		/// <summary>
		/// Sort by last modified date;
		/// </summary>
		[Description("datemodified")]
		DateModified = 6,


		//newly added
		/// <summary>
		/// Sort by language;
		/// </summary>
		[Description("language")]
		Language = 7,

		/// <summary>
		/// Sort by frequency;
		/// </summary>
		[Description("frequency")]
		Frequency = 8,

		/// <summary>
		/// Sort by coverage;
		/// </summary>
		[Description("coverage")]
		Coverage = 9,

		/// <summary>
		/// Sort by publisher;
		/// </summary>
		[Description("publisher")]
		Publisher = 10,

		/// <summary>
		/// Sort by regions;
		/// </summary>
		[Description("regions")]
		Regions = 11,

		/// <summary>
		/// Sort by subjects;
		/// </summary>
		[Description("subjects")]
		Subjects = 12,

		/// <summary>
		/// Sort by industries;
		/// </summary>
		[Description("industries")]
		Industries = 13,

		/// <summary>
		/// Sort by user added info;
		/// </summary>
		[Description("useraddedinfo")]
		UserAddedInfo = 14,

		/// <summary>
		/// Sort by state;
		/// </summary>
		[Description("state")]
		State = 15,

		/// <summary>
		/// Sort by city;
		/// </summary>
		[Description("city")]
		City = 16,

		[Description("origin-country")]
		OriginCountry = 17
	}
}