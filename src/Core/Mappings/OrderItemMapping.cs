namespace Core
{
	public class OrderItemMapping : EntityMapping<OrderItem>
	{
		public OrderItemMapping()
		{
			ManyToOne(x => x.Order, map => map.Column("OrderId"));
			Property(x => x.Product);
			Property(x => x.Price);
		}
	}
}