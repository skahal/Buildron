using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buildron.Domain.Mods;
using Buildron.Infrastructure.DataProxies;

namespace Buildron.Infrastructure.PreferencesProxies
{
    public class ModPreferencesProxy : ModDataProxy, IPreferencesProxy
    {
        #region Fields
        private Preference[] m_preferences = new Preference[0];
        #endregion

        #region Constructors
        public ModPreferencesProxy(ModInfo modInfo)
            : base(modInfo)
        {
        }
        #endregion

        #region Methods   
        public void Register(params Preference[] preferences)
        {
			m_preferences = preferences;
        }

		public override TValue GetValue<TValue> (string key)
		{
			var preference = GetRegisteredPreference (key);

			if (!HasValue (key)) {
				base.SetValue(key, (TValue) preference.DefaultValue);
			}

			return base.GetValue<TValue> (key);
		}

		public override void SetValue<TValue> (string key, TValue value)
		{
			GetRegisteredPreference (key);
			base.SetValue<TValue> (key, value);
		}

		public override void RemoveValue<TValue> (string key)
		{
			GetRegisteredPreference (key);
			base.RemoveValue<TValue> (key);
		}

        public Preference[] GetRegisteredPreferences()
        {
            return m_preferences;
        }

		private Preference GetRegisteredPreference(string key)
		{
			var preference = m_preferences.FirstOrDefault (p => p.Name.Equals (key, StringComparison.OrdinalIgnoreCase));

			if(preference == null)
			{
				throw new ArgumentException ("The preference with name '{0}' is not registerd.".With(key), "key");
			}

			return preference;
		}
        #endregion
    }
}
