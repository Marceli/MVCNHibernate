using System.Collections.Generic;

namespace Core
{
	public class Customer : Entity
	{
		public virtual string CommercialName { get; set; }
		public virtual string TaxId { get; set; }
		public virtual IEnumerable<Order> Orders { get; set; }
	}
}