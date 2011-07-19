using System;
using NUnit.Framework;
using System.Linq;
namespace Core
{
	[TestFixture]
	public class NHibernateTest
	{
		[Test]
		public void CanSaveCustomer()
		{
			using(var session = FactoryProvider.Factory.OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					try
					{
						var customer = new Customer();
						customer.CommercialName = "Franio";
						customer.TaxId = "12312";
						session.Save(customer);
						transaction.Commit();
						Assert.Greater(customer.Id, 0);
					}catch
					{
						transaction.Rollback();

					}
			}
		}
		[Test]
		public void CanSaveOrderWithItems()
		{
			int olo;
			using(var session=FactoryProvider.Factory.OpenSession())
				using(var transaction=session.BeginTransaction())
				{
					var order = new Order();
					order.EmissionDay = DateTime.Now;
					order.AddItem(new OrderItem{Product = "oo", Order = order, Price = 100});
					order.AddItem(new OrderItem{Product = "o22", Order = order, Price = 140});
					session.Save(order);
					transaction.Commit();
					olo = order.Id;
				}
			using(var session=FactoryProvider.Factory.OpenSession())
				using(var transaction=session.BeginTransaction())
				{
					var order = session.Get<Order>(olo);
					Assert.AreEqual(2, order.Items.Count());
				}
		}
	}
}
