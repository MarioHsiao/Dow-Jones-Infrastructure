
namespace DowJones.DTO.Web.LOB
{
	public abstract class AbstractRequestDTO : IRequestDTO
	{
		/// <summary>
		/// Gets the root key.
		/// </summary>
		/// <value>The root key.</value>
		public virtual DtoPersistance RootKey
		{
			get { return DtoPersistance.Request; }
		}

		/// <summary>
		/// Loads this instance.
		/// </summary>
		public virtual void Load()
		{
		}

		/// <summary>
		/// Determines whether this instance is valid.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if this instance is valid; otherwise, <c>false</c>.
		/// </returns>
		public virtual bool IsValid()
		{
			return true;
		}

		/// <summary>
		/// Gets the key.
		/// </summary>
		/// <value>The key.</value>
		public virtual string Key
		{
			get { return (DTOKeyMapper.GetDtoId(GetType())); }
		}

	    /// <summary>
		/// Determines whether the specified strings is valid.
		/// </summary>
		/// <param name="strings">The strings.</param>
		/// <returns>
		/// 	<c>true</c> if the specified strings is valid; otherwise, <c>false</c>.
		/// </returns>
		public bool IsValid(string[] strings)
	    {
	        return strings != null && strings.Length >= 0;
	    }
	}
}