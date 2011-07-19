using NHibernate;
using NHibernate.Mapping.ByCode;

namespace Core
{
	public class OrderMapping : EntityMapping<Order>
	{
		public OrderMapping()
		{
			ManyToOne(x => x.Customer, map => map.Column("CustomerId"));
			Property(x => x.EmissionDay, map => map.Type(NHibernateUtil.Date));
		
			Bag(x => x.Items, map => map.Key(km => km.Column("OrderId")));
			Bag(x => x.Items, map => map.Access(Accessor.Field));
			Bag(x => x.Items, map => map.Access(Accessor.Field));
		}
	}
}