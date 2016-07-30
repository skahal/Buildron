using System;
using Buildron.Domain.Mods;


public class EmulatorPreferencesProxy : IPreferencesProxy
{
	public EmulatorPreferencesProxy ()
	{
	}

	#region IPreferenceProxy implementation

	public void Register (params Preference[] preferences)
	{

	}

	#endregion

	#region IDataProxy implementation

	public bool HasValue (string key)
	{
		return true;
	}

	public void SetValue<TValue> (string key, TValue value)
	{

	}

	public TValue GetValue<TValue> (string key)
	{
		return default(TValue);
	}

	public void RemoveValue<TValue> (string key)
	{
		
	}

	#endregion
}


