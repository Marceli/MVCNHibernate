namespace Core
{
	public class CustomerMapping : EntityMapping<Customer>
	{
		public CustomerMapping()
		{
			Property(x => x.CommercialName);
			NaturalId(map => map.Property(x => x.TaxId));
			Bag(x => x.Orders, map => map.Key(km => km.Column("CustomerId")));
		}
	}
}