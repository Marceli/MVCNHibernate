namespace Core
{
	public class OrderItem : Entity
	{
		public virtual Order Order { get; set; }
		public virtual string Product { get; set; }
		public virtual decimal Price { get; set; }
	}
}