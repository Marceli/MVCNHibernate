using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Iesi.Collections.Generic;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Mapping;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;

namespace Core
{
	public class FactoryProvider
	{
		private static IAuxiliaryDatabaseObject CreateHighLowScript(IModelInspector inspector, IEnumerable<Type> entities)
		{
			var script = new StringBuilder(3072);
			script.AppendLine("DELETE FROM NextHighVaues;");
			script.AppendLine("ALTER TABLE NextHighVaues ADD EntityName VARCHAR(128) NOT NULL;");
			script.AppendLine("CREATE NONCLUSTERED INDEX IdxNextHighVauesEntity ON NextHighVaues (EntityName ASC);");
			script.AppendLine("GO");
			foreach (var entity in entities.Where(inspector.IsRootEntity))
			{
				script.AppendLine(String.Format("INSERT INTO [NextHighVaues] (EntityName, NextHigh) VALUES ('{0}',1);", entity.Name.ToLowerInvariant()));
			}
			return new SimpleAuxiliaryDatabaseObject(script.ToString(), null, new HashedSet<string> { typeof(MsSql2005Dialect).FullName, typeof(MsSql2008Dialect).FullName });
		}

		public static ISessionFactory Factory { get; private set; }

		static FactoryProvider()
		{
			var modelInspector = new MySimpleModelInspector();
			Assert.IsNotNull(new Entity());
			var mapper = new ModelMapper(modelInspector);
			mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());
			mapper.BeforeMapClass += (mi, type, map) =>
			                         map.Id(idmap => idmap.Generator(Generators.HighLow,
			                                                         gmap => gmap.Params(new
			                                                                             	{
			                                                                             		table = "NextHighVaues",
			                                                                             		column = "NextHigh",
			                                                                             		max_lo = 100,
			                                                                             		where = String.Format("EntityName = '{0}'", type.Name.ToLowerInvariant())
			                                                                             	})));
			mapper.BeforeMapClass += (mi, t, map) => map.Table(t.Name.ToLowerInvariant());
			mapper.BeforeMapJoinedSubclass += (mi, t, map) => map.Table(t.Name.ToLowerInvariant());
			mapper.BeforeMapUnionSubclass += (mi, t, map) => map.Table(t.Name.ToLowerInvariant());

			mapper.BeforeMapProperty += (mi, propertyPath, map) =>
			                            	{
			                            		if (typeof(decimal).Equals(propertyPath.LocalMember.GetPropertyOrFieldType()))
			                            		{
			                            			map.Type(NHibernateUtil.Currency);
			                            		}
			                            	};
			mapper.BeforeMapBag += (mi, propPath, map) =>
			                       	{
			                       		map.Cascade(Cascade.All.Include(Cascade.DeleteOrphans));
			                       		map.BatchSize(10);
			                       	};
			mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());
			HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
			var configuration = new Configuration();
			configuration.DataBaseIntegration(c =>
			                                  	{
			                                  		c.Dialect<MsSql2008Dialect>();
			                                  		c.ConnectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=IntroNH;Integrated Security=True;Pooling=False";
			                                  		c.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
			                                  		c.SchemaAction = SchemaAutoAction.Create;
			                                  	});
			configuration.AddMapping(domainMapping);
			configuration.AddAuxiliaryDatabaseObject(CreateHighLowScript(modelInspector, Assembly.GetExecutingAssembly().GetExportedTypes()));

			Factory=configuration.BuildSessionFactory();
		}
	}
}
