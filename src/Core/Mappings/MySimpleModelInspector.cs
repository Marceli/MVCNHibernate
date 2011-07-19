using System.Reflection;
using NHibernate.Mapping.ByCode;

namespace Core
{
	public class MySimpleModelInspector : ExplicitlyDeclaredModel
	{
		public override bool IsOneToMany(MemberInfo member)
		{
			if (IsBag(member))
			{
				return true;
			}
			return base.IsOneToMany(member);
		}
	}
}