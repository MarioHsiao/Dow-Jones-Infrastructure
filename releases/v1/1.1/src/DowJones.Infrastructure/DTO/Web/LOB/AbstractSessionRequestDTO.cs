namespace DowJones.Utilities.DTO.Web.LOB
{
	public abstract class AbstractSessionRequestDTO : AbstractRequestDTO
	{
		public override DtoPersistance RootKey
		{
			get { return DtoPersistance.Session; }
		}

		public override string Key
		{
			get { return (DTOKeyMapper.GetDtoId(GetType())); }
		}
	}
}