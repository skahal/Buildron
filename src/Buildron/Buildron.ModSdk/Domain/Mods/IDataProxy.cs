using System;

namespace Buildron.Domain.Mods
{
	public interface IDataProxy
	{
		bool HasValue (string key);
		void SetValue<TValue>(string key, TValue value);
		TValue GetValue<TValue> (string key);
		void RemoveValue<TValue> (string key);
	}
}