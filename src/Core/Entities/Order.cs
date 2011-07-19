using System;
using System.Collections.Generic;
using System.Linq;
using Iesi.Collections.Generic;

namespace Core
{
	public class Order : Entity
	{
		
		private ISet<OrderItem> items = new HashedSet<OrderItem>();
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