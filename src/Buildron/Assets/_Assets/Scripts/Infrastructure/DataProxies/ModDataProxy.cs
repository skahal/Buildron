using System;
using Buildron.Domain.Mods;
using UnityEngine;
using Skahal.Serialization;

namespace Buildron.Infrastructure.DataProxies
{
	public class ModDataProxy : IDataProxy
	{
		private ModInfo m_modInfo;

		public ModDataProxy (ModInfo modInfo)
		{
			m_modInfo = modInfo;
		}

		#region IDataProxy implementation

		public void SaveValue<TValue> (string key, TValue value)
		{
			var serialized = Serialize(value);
			PlayerPrefs.SetString (GetModKey (key), serialized);
		}

		public TValue GetValue<TValue> (string key)
		{
			var modKey = GetModKey (key);

			if (PlayerPrefs.HasKey (modKey)) {
				var value = PlayerPrefs.GetString(modKey);

				return (TValue)Deserialize<TValue> (value);
			}

			return default(TValue);
		}

		public void RemoveValue<TValue>(string key)
		{
			PlayerPrefs.DeleteKey (key);
		}

		string GetModKey (string key)
		{
			return "{0}_{1}".With (m_modInfo.Name, key);
		}

		string Serialize (object value)
		{
			var valueType = value.GetType ();

			if (valueType == typeof(Vector3)) {
				var v = (Vector3)value;
				return SHSerializer.SerializeVector3 (v);
			}

			return SHSerializer.SerializeToString (value);
		}

		object Deserialize<TValue> (string value)
		{
			var valueType = typeof(TValue);

			if (valueType == typeof(Vector3)) {
				return SHSerializer.DeserializeVector3 (value);
			}

			return SHSerializer.DeserializeFromString<TValue> (value);
		}
		#endregion
	}
}

