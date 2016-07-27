using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Skahal.Logging;

namespace Buildron.Infrastructure
{
	public static class TypeHelper
	{
		private static Type[] s_allTypes;

		static TypeHelper()
		{
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = new List<Type>();

            foreach (var a in assemblies)
            {
                try
                {
                    types.AddRange(a.GetTypes());
                }
                catch(ReflectionTypeLoadException ex)
                {                    
                    ex.Log("Error getting types from assembly: {0}".With(a.FullName));
                    throw;
                }
                catch (Exception ex)
                {
                    SHLog.Error("Error getting types from assembly: {0}. Exception {1}: {2}", a.FullName, ex.GetType().Name, ex.Message);
                    throw;
                }
            }

            s_allTypes = types.ToArray();
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

