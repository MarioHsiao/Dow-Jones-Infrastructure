using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Web.Mvc.UI.Components.OutletList
{
	/// <summary>
	/// Class representing a model for outlet list.
	/// </summary>
	public class OutletModel
	{
		/// <summary>
		/// Gets and sets outlet ID.
		/// </summary>
		public int OutletId { get; set; }

		/// <summary>
		/// Gets and sets outlet name.
		/// </summary>
		public string OutletName { get; set; }

		/// <summary>
		/// Gets and sets outlet circulation.
		/// </summary>
		public int Circulation { get; set; }

		/// <summary>
		/// Gets and sets outlet country.
		/// </summary>
		public string OriginCountry { get; set; }

		/// <summary>
		/// Gets and sets outlet country.
		/// </summary>
		public string Country { get; set; }

		/// <summary>
		/// Gets and sets outlet state.
		/// </summary>
		public string State { get; set; }

		/// <summary>
		/// Gets and sets outlet city.
		/// </summary>
		public string City { get; set; }

		/// <summary>
		/// Gets and sets when last an outlet has been modified
		/// </summary>
		public DateTime DateModified { get; set; }

		/// <summary>
		/// Gets and sets outlet type
		/// </summary>
		public OutletType OutletType { get; set; }

		/// <summary>
		/// Gets and sets media format
		/// </summary>
		public MediaFormat MediaFormat { get; set; }

		//new properties

		/// <summary>
		/// Gets and sets languages
		/// </summary>
		public List<Language> Languages { get; set; }

		/// <summary>
		/// Gets and sets Frequency
		/// </summary>
		public Frequency Frequency { get; set; }

		/// <summary>
		/// Gets and sets Coverage
		/// </summary>
		public string Coverage { get; set; }

		/// <summary>
		/// Gets and sets Publisher
		/// </summary>
		public string Publisher { get; set; }

		/// <summary>
		/// Gets or sets the region editorial focus.
		/// </summary>
		/// <value>The region editorial focus.</value>
		public List<Region> RegionEditorialFocus { get; set; }

		/// <summary>
		/// Gets or sets the industry editorial focus.
		/// </summary>
		/// <value>The industry editorial focus.</value>
		public List<Industry> IndustryEditorialFocus { get; set; }

		/// <summary>
		/// Gets or sets the subject editorial focus.
		/// </summary>
		/// <value>The subject editorial focus.</value>
		public List<Subject> SubjectEditorialFocus { get; set; }

		/// <summary>
		/// Gets or sets the user added info.
		/// </summary>
		/// <value>The subject editorial focus.</value>
		public ProprietaryInfo UserAddedInformation { get; set; }

		public string GetLanguagesText()
		{
			List<string> languages = this.Languages.Select(x => x.Name).ToList();
			return GetTextFromCollection(languages);
		}

		public string GetIndustriesText()
		{
			List<string> industries = this.IndustryEditorialFocus.Select(x=>x.Name).ToList();
			return GetTextFromCollection(industries);
		}

		public string GetSubjectsText()
		{
			List<string> subjects = this.SubjectEditorialFocus.Select(x => x.Name).ToList();
			return GetTextFromCollection(subjects);
		}

		public string GetRegionsText()
		{
			List<string> regions = this.RegionEditorialFocus.Select(x => x.Name).ToList();
			return GetTextFromCollection(regions);
		}

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

		public string GetUserAddedInfoText()
		{
			StringBuilder sb = new StringBuilder();
			OutletListTokens olt = new OutletListTokens();

			if (!string.IsNullOrEmpty(this.UserAddedInformation.City))
			{
				sb.AppendFormat("{0}: {1}<br />", olt.UACity, this.UserAddedInformation.City);
			}
			if (!string.IsNullOrEmpty(this.UserAddedInformation.Country))
			{
				sb.AppendFormat("{0}: {1}<br />", olt.UACountry, this.UserAddedInformation.Country);
			}
			if (!string.IsNullOrEmpty(this.UserAddedInformation.Phone))
			{
				sb.AppendFormat("{0}: {1}<br />", olt.UAPhone, this.UserAddedInformation.Phone);
			}
			if (!string.IsNullOrEmpty(this.UserAddedInformation.Mobile))
			{
				sb.AppendFormat("{0}: {1}<br />", olt.UAMobile, this.UserAddedInformation.Mobile);
			}
			if (!string.IsNullOrEmpty(this.UserAddedInformation.Email1) ||
				!string.IsNullOrEmpty(this.UserAddedInformation.Email2) ||
				!string.IsNullOrEmpty(this.UserAddedInformation.Email3))
			{
				StringBuilder esb = new StringBuilder();
				if (!string.IsNullOrEmpty(this.UserAddedInformation.Email1))
					esb.AppendFormat("{0}; ", this.UserAddedInformation.Email1);
				if (!string.IsNullOrEmpty(this.UserAddedInformation.Email2))
					esb.AppendFormat("{0}; ", this.UserAddedInformation.Email2);
				if (!string.IsNullOrEmpty(this.UserAddedInformation.Email3))
					esb.AppendFormat("{0}; ", this.UserAddedInformation.Email3);

				sb.AppendFormat("{0}: {1}", olt.UAEmail, esb.ToString());
			}

			return sb.ToString();
		}
	}

	// Hold proprietary info
	public class ProprietaryInfo
	{

		/// <summary>
		/// Gets or sets the city.
		/// </summary>
		/// <value>The city.</value>
		public string City { get; set; }

		/// <summary>
		/// Gets or sets the country.
		/// </summary>
		/// <value>The country.</value>
		public string Country { get; set; }

		/// <summary>
		/// Gets or sets the phone.
		/// </summary>
		/// <value>The phone.</value>
		public string Phone { get; set; }

		/// <summary>
		/// Gets or sets the mobile.
		/// </summary>
		/// <value>The mobile.</value>
		public string Mobile { get; set; }

		/// <summary>
		/// Gets or sets the fax.
		/// </summary>
		/// <value>The fax.</value>
		public string Fax { get; set; }

		/// <summary>
		/// Gets or sets the email1.
		/// </summary>
		/// <value>The email1.</value>
		public string Email1 { get; set; }

		/// <summary>
		/// Gets or sets the email2.
		/// </summary>
		/// <value>The email2.</value>
		public string Email2 { get; set; }

		/// <summary>
		/// Gets or sets the email3.
		/// </summary>
		/// <value>The email3.</value>
		public string Email3 { get; set; }

	}

	/// Holds subjects
	public class Subject : Beat
	{
		public Subject(){}
		public Subject(int id, string name, string description)
		{
			this.Id = id;
			this.Name = name;
			Description = description;
		} 
		public string Description { get; set; }
	}

	/// Holds idustries
	public class Industry : Beat
	{
		public Industry(){}
		public Industry(int id, string name, string description)
		{
			this.Id = id;
			this.Name = name;
			Description = description;
		} 
		public string Description { get; set; }
	}

	/// <summary>
	/// Holds regions
	/// </summary>
	public class Region : Beat
	{
		public Region(){}
		public Region(int id, string name, string code, string description)
		{
			this.Id = id;
			this.Name = name;
			this.Code = code;
			Description = description;
		}

		public string Code { get; set; }
		public string Description { get; set; }
	}

	/// <summary>
	/// Represents region, industry or subject
	/// </summary>
	public class Beat
	{
		/// <summary>
		/// Gets or sets the id.
		/// </summary>
		/// <value>The id.</value>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }


		/// <summary>
		/// Gets or sets the parent beat id.
		/// </summary>
		/// <value>The parent id.</value>
		public int? ParentId { get; set; }
	}

	/// <summary>
	/// Represents how often the outlet is published
	/// </summary>
	public class Frequency
	{
		/// <summary>
		/// Gets or sets the id.
		/// </summary>
		/// <value>The id.</value>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		public Frequency() { }

		public Frequency(int id, string name)
		{
			this.Id = id;
			this.Name = name;
		}
	}

	public class Language
	{
		/// <summary>
		/// Gets and sets language Id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets and sets language name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the code.
		/// </summary>
		/// <value>The code.</value>
		public string Code { get; set; }  

		/// <summary>
		/// Create an instanse of OutletProperties class.
		/// </summary>
		public Language()
		{
		}

		/// <summary>
		/// Create an instance of Language class and populate properties.
		/// </summary>
		/// <param name="outletId">Language ID.</param>
		/// <param name="outletName">Language name.</param>
		public Language(int id, string name, string code)
		{
			this.Id = id;
			this.Name = name;
			this.Code = code;
		}
	}

	/// <summary>
	/// Class represents an Outlet type.
	/// </summary>
	public class OutletType
	{
		/// <summary>
		/// Gets and sets outlet type ID.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets and sets outlet type name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Create an instanse of OutletProperties class.
		/// </summary>
		public OutletType()
		{
		}

		/// <summary>
		/// Create an instance of Outlet type class and populate properties.
		/// </summary>
		/// <param name="outletId">Outlet type ID.</param>
		/// <param name="outletName">Outlet type name.</param>
		public OutletType(int id, string name)
		{
			this.Id = id;
			this.Name = name;
		}
	}

	/// <summary>
	/// Class represents a Media Format.
	/// </summary>
	public class MediaFormat
	{
		/// <summary>
		/// Gets and sets media format ID.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets and sets media format name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Create an instanse of OutletProperties class.
		/// </summary>
		public MediaFormat()
		{
		}

		/// <summary>
		/// Create an instance of Media Format class and populate properties.
		/// </summary>
		/// <param name="outletId">Media format ID.</param>
		/// <param name="outletName">Media format name.</param>
		public MediaFormat(int id, string name)
		{
			this.Id = id;
			this.Name = name;
		}
	}

}