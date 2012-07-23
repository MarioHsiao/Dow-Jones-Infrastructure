using System;
using DowJones.DTO.Web.LOB;

namespace DowJones.DTO.Web
{
	public abstract class AbstractFormState
	{
		public const string CONTEXT_KEY_CURRENT_FORM_STATE_OBJECT = "CURRENT_FORM_STATE_OBJECT";

		public abstract string EmptyDtoFormStateString { get; }
		public abstract IRequestDTO Accept(Type type, bool persist);

		protected abstract void Load();
		protected abstract void Load(string formState);
		protected abstract void Save();

		public abstract void Remove(IRequestDTO requestDto);
		public abstract void Add(IRequestDTO dto);
		public abstract string GetSessionData();
	    public abstract string GetFormState();
		public abstract string GetState(object domainObject);
		public abstract string GetState(object domainObject, bool escapeForJavascript);
	}
}