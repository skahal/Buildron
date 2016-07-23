using System;
using System.Linq;

namespace Buildron.Infrastructure
{
	public static class TypeHelper
	{
		private static Type[] s_allTypes;

		static TypeHelper()
		{
			s_allTypes = AppDomain.CurrentDomain.GetAssemblies ().SelectMany (a => a.GetTypes ()).ToArray ();
		}

		public static Type[] GetAllTypes()
		{
			return s_allTypes;
		}

		public static Type[] GetImplementationsOf<TInterface>()
		{
			var type = typeof(TInterface);

			return s_allTypes.Where (t => !t.IsAbstract && type.IsAssignableFrom (t)).ToArray ();
		}

		public static Type GetType(string typeName)
		{
			return s_allTypes.SingleOrDefault (t => t.Name.Equals (typeName, StringComparison.OrdinalIgnoreCase));
		}
	}
}

