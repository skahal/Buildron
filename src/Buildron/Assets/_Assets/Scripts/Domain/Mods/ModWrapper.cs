using System;
namespace Buildron.Domain.Mods
{
	public class ModWrapper : IMod
	{
		private object m_mod;
		private Type m_modType;

		public ModWrapper(object mod)
		{
			m_mod = mod;
			m_modType = mod.GetType ();
			Name = m_modType.GetProperty ("Name").GetValue (m_mod, null) as string;
		}

		#region IMod implementation

		public void Initialize (IModContext context)
		{
			m_modType.GetMethod ("Initialize").Invoke (m_mod, new object[] { context });
		}

		public string Name { get; private set; }

		#endregion
    
	}
}
