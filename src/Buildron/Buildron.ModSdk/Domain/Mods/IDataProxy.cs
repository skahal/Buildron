using System;

namespace Buildron.Domain.Mods
{
	public interface IDataProxy
	{
		void SaveValue<TValue>(string key, TValue value);
		TValue GetValue<TValue> (string key);
		void RemoveValue<TValue> (string key);
	}
}