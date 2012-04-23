using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DowJones.Web.Mvc.UI.Components.Models
{
	/// <summary>
	/// Class representing an model for author list.
	/// </summary>
	public class AuthorModel
	{
		public int AuthorId { get; set; }
		public int AuthorNNID { get; set; }
		public string AuthorName { get; set; }
		public List<string> EmailAddresses { get; set; }
		public List<string> Phones { get; set; }
		public List<string> Fax { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
		public string Zip { get; set; }
		public List<string> Language { get; set; }
		public IEnumerable<OutletProperties> Outlets { get; set; }
		public List<string> BeatsSubjects { get; set; }
		public List<string> BeatsIndustries { get; set; }
		public ProprietaryInfo UserInfo { get; set; }

		/// <summary>
		/// Gets whether the author have at least one outlet.
		/// </summary>
		public bool HasOutlets
		{
			get
			{
				if (this.Outlets == null || this.Outlets.Any() == false)
				{
					return false;
				}

				return true;
			}
		}

		/// <summary>
		/// Gets whether the author have more than one outlet.
		/// </summary>
		public bool ExpandableOutlet
		{
			get
			{
				if (this.Outlets.Any() == false)
					return false;
				else if (this.Outlets.Count() > 1)
					return true;

				return false;
			}
		}

		/// <summary>
		/// Gets and sets whether an author has articles in last tree months.
		/// </summary>
		public bool HasArticles { get; set; }

		public AuthorModel()
		{
			this.Outlets = Enumerable.Empty<OutletProperties>();
		}

		public string CreateTextFromUserAddedContactInfo()
		{
			if (this.UserInfo == null)
			{
				return String.Empty;
			}

			StringBuilder sb = new StringBuilder();
			AuthorListTokens t = new AuthorListTokens();
			int count = 0;
			const string div = "<div>";
			const string hiddenDiv = "<div class='dj_hidden-cell-item hide'>";
			if (String.IsNullOrEmpty(this.UserInfo.City) == false)
			{
				count++;
				sb.AppendFormat("<div>{0}: {1}</div>", t.UACity, this.UserInfo.City);
			}

			if (String.IsNullOrEmpty(this.UserInfo.Country) == false)
			{
				count++;
				sb.AppendFormat("<div>{0}: {1}</div>", t.UACountry, this.UserInfo.Country);
			}

			if (String.IsNullOrEmpty(this.UserInfo.Phone) == false)
			{
				count++;
				sb.AppendFormat(
					"{0}{1}: {2}</div>", 
					count > 2 ? hiddenDiv : div,
					t.UAPhone,
					this.UserInfo.Phone);
			}

			if (String.IsNullOrEmpty(this.UserInfo.Mobile) == false)
			{
				count++;
				sb.AppendFormat(
					"{0}{1}: {2}</div>", 
					count > 2 ? hiddenDiv : div, 
					t.UAMobile,
					this.UserInfo.Mobile);
			}

			if (String.IsNullOrEmpty(this.UserInfo.Email1) == false
				|| String.IsNullOrEmpty(this.UserInfo.Email2) == false
				|| String.IsNullOrEmpty(this.UserInfo.Email3) == false)
			{
				if (String.IsNullOrEmpty(this.UserInfo.Email1) == false)
				{
					count++;
					sb.AppendFormat(
						"{0}{1}: {2}</div>",
						count > 2 ? hiddenDiv : div,
						t.UAEmail,
						this.UserInfo.Email1);
				}

				if (String.IsNullOrEmpty(this.UserInfo.Email2) == false)
				{
					count++;
					sb.AppendFormat(
						"{0}{1}: {2}</div>",
						count > 2 ? hiddenDiv : div,
						t.UAEmail,
						this.UserInfo.Email2);
				}

				if (String.IsNullOrEmpty(this.UserInfo.Email3) == false)
				{
					count++;
					sb.AppendFormat(
						"{0}{1}: {2}</div>",
						count > 2 ? hiddenDiv : div,
						t.UAEmail,
						this.UserInfo.Email3);
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

	/// <summary>
	/// Class represents an outlet.
	/// Used in DowJones.Web.Mvc.UI.Components.AuthorList.AuthorModel class.
	/// </summary>
	public class OutletProperties
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string EmploymentType { get; set; }
		public string JobTitle { get; set; }
		public List<string> Type { get; set; }
		public int Circulation { get; set; }
		public string Frequency { get; set; }
		public string State { get; set; }
		public string Country { get; set; }
		public List<string> EmailAddresses { get; set; }
		public List<string> Phones { get; set; }
		public List<string> Faxes { get; set; }

		public OutletProperties()
		{
		}

		public OutletProperties(
			int id,
			string name,
			string employmentType,
			string jobTitle,
			List<string> type,
			int circulation,
			string frequency,
			string state,
			string country,
			List<string> emails,
			List<string> phones,
			List<string> faxes)
		{
			this.Id = id;
			this.Name = name;
			this.EmploymentType = employmentType;
			this.JobTitle = jobTitle;
			this.Type = type;
			this.Circulation = circulation;
			this.Frequency = frequency;
			this.State = state;
			this.Country = country;
			this.EmailAddresses = emails;
			this.Phones = phones;
			this.Faxes = faxes;
		}
	}
}