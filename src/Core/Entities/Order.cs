using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
	public class Order : Entity
	{
		
		private IList<OrderItem> items = new List<OrderItem>();
		public virtual Customer Customer { get; set; }
		public virtual DateTime EmissionDay { get; set; }
		public virtual IEnumerable<OrderItem> Items
		{
			get { return items.ToArray(); }
			
		}

		public virtual void AddItem(OrderItem item)
		{
			items.Add(item);
		}

		public virtual void RemoveItem(OrderItem item)
		{
			items.Remove(item);
		}	
	}
}