using NHibernate.Mapping.ByCode.Conformist;

namespace Core
{
	public class EntityMapping<T> : ClassMapping<T> where T : Entity
	{
		public EntityMapping()
		{
			Id(x => x.Id);
		}
	}
}