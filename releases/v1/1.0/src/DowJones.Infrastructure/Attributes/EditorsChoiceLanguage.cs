using System;

namespace DowJones.Utilities.Attributes
{
	/// <summary>
	/// Summary description for IsEditorsChoiceLanguage.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple=false)]
	public class EditorsChoiceLanguage : Attribute
	{
		
		private bool _isEditorsChoiceLanguage = false;

		/// <summary>
		/// Gets or sets a value indicating whether this instance is editors choice language.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is editors choice language; otherwise, <c>false</c>.
		/// </value>
		public bool IsEditorsChoiceLanguage
		{
			get { return _isEditorsChoiceLanguage; }
			set { _isEditorsChoiceLanguage = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EditorsChoiceLanguage"/> class.
		/// </summary>
		public EditorsChoiceLanguage()
		{
			_isEditorsChoiceLanguage = true;
		}

	}
}
