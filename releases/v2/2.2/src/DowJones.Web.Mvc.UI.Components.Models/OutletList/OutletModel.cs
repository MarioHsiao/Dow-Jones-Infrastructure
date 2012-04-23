using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DowJones.Web.Mvc.UI.Components.Models
{
	/// <summary>
	/// Class representing a model for outlet list.
	/// </summary>
	public class OutletModel
	{
		// About;
		public int OutletId { get; set; }
		public string OutletName { get; set; }
		public bool HasMediaContacts { get; set; }
		public int Circulation { get; set; }
		public List<string> Type { get; set; }
		public string WebSite { get; set; }
		// Contact information;
		public string Address { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public string Country { get; set; }
		public List<string> Emails { get; set; }
		public List<string> Phones { get; set; }
		public List<string> Faxes { get; set; }
		// Editorial focus;
		public List<string> Industries { get; set; }
		public List<string> Subjects { get; set; }
		public List<string> Regions { get; set; }
		// Additional metadata;
		public List<string> Languages { get; set; }
		public string Frequency { get; set; }
		public string Coverage { get; set; }
		public List<string> Audiences { get; set; }
		public string Publisher { get; set; }
		// User Information;
		public ProprietaryInfo UserAddedInformation { get; set; }

		private string GetTextFromCollection(List<string> namesCol)
		{
			StringBuilder sb = new StringBuilder();
			foreach (string name in namesCol)
			{
				if (!string.IsNullOrEmpty(name))
				{
					sb.AppendFormat("{0}|", name);
				}
			}
			return sb.ToString();
		}

		public string CreateTextFromUserAddedContactInfo()
		{
			if (this.UserAddedInformation == null)
			{
				return String.Empty;
			}

			StringBuilder sb = new StringBuilder();
			OutletListTokens t = new OutletListTokens();
			int count = 0;
			const string div = "<div>";
			const string hiddenDiv = "<div class='dj_hidden-cell-item hide'>";
			if (String.IsNullOrEmpty(this.UserAddedInformation.City) == false)
			{
				count++;
				sb.AppendFormat("<div>{0}: {1}</div>", t.UACity, this.UserAddedInformation.City);
			}

			if (String.IsNullOrEmpty(this.UserAddedInformation.Country) == false)
			{
				count++;
				sb.AppendFormat("<div>{0}: {1}</div>", t.UACountry, this.UserAddedInformation.Country);
			}

			if (String.IsNullOrEmpty(this.UserAddedInformation.Phone) == false)
			{
				count++;
				sb.AppendFormat(
					"{0}{1}: {2}</div>",
					count > 2 ? hiddenDiv : div,
					t.UAPhone,
					this.UserAddedInformation.Phone);
			}

			if (String.IsNullOrEmpty(this.UserAddedInformation.Mobile) == false)
			{
				count++;
				sb.AppendFormat(
					"{0}{1}: {2}</div>",
					count > 2 ? hiddenDiv : div,
					t.UAMobile,
					this.UserAddedInformation.Mobile);
			}

			if (String.IsNullOrEmpty(this.UserAddedInformation.Email1) == false
				|| String.IsNullOrEmpty(this.UserAddedInformation.Email2) == false
				|| String.IsNullOrEmpty(this.UserAddedInformation.Email3) == false)
			{
				if (String.IsNullOrEmpty(this.UserAddedInformation.Email1) == false)
				{
					count++;
					sb.AppendFormat(
						"{0}{1}: {2}</div>",
						count > 2 ? hiddenDiv : div,
						t.UAEmail,
						this.UserAddedInformation.Email1);
				}

				if (String.IsNullOrEmpty(this.UserAddedInformation.Email2) == false)
				{
					count++;
					sb.AppendFormat(
						"{0}{1}: {2}</div>",
						count > 2 ? hiddenDiv : div,
						t.UAEmail,
						this.UserAddedInformation.Email2);
				}

				if (String.IsNullOrEmpty(this.UserAddedInformation.Email3) == false)
				{
					count++;
					sb.AppendFormat(
						"{0}{1}: {2}</div>",
						count > 2 ? hiddenDiv : div,
						t.UAEmail,
						this.UserAddedInformation.Email3);
				}
			}

			if (count > 2)
			{
				sb.AppendFormat(
					"<div><a href='javascript:void(0);' class='dj_show-hide-cell-items' more='true'>{0}</a></div>",
					t.ShowCellItems);
			}

			string retval = sb.ToString();
			return retval;
		}
	}

	// Hold proprietary info
	public class ProprietaryInfo
	{
		public string City { get; set; }
		public string Country { get; set; }
		public string Phone { get; set; }
		public string Mobile { get; set; }
		public string Fax { get; set; }
		public string Email1 { get; set; }
		public string Email2 { get; set; }
		public string Email3 { get; set; }
	}
}