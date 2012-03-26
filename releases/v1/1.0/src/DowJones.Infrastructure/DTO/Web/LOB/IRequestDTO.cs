
namespace DowJones.Utilities.DTO.Web.LOB
{
	public enum DtoPersistance
	{
		Session,
		Request,
		Breadtrail,
		Persisted,
	}
	
	/// <summary>
	/// Summary description for IRequestLob.
	/// </summary>
	public interface IRequestDTO
	{
		/// <summary>
		/// Determines whether this instance is valid.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if this instance is valid; otherwise, <c>false</c>.
		/// </returns>
		bool IsValid();
		
		/// <summary>
		/// Gets the key.
		/// </summary>
		/// <value>The key.</value>
		string Key { get; }
		
		/// <summary>
		/// Gets the root key.
		/// </summary>
		/// <value>The root key.</value>
		DtoPersistance RootKey { get; }
		
		
		/// <summary>
		/// Loads this instance.
		/// </summary>
		void Load();
		
	}
}
