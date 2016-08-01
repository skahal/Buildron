namespace Buildron.Domain.Mods
{
	/// <summary>
	/// Defines an interface to a data proxy.
	/// </summary>
	public interface IDataProxy
	{
		/// <summary>
		/// Verifiy if there is a value with the specified key.
		/// </summary>
		/// <returns>True if there is a value for the key.</returns>
		/// <param name="key">The value's key.</param>
		bool HasValue (string key);

		/// <summary>
		/// Set a value for the key.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="key">The value's key.</param>
		/// <param name="value">The value.</param>
		/// <typeparam name="TValue">The 1st type parameter.</typeparam>
		void SetValue<TValue>(string key, TValue value);

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="key">The value's key.</param>
		/// <typeparam name="TValue">The 1st type parameter.</typeparam>
		TValue GetValue<TValue> (string key);

		/// <summary>
		/// Removes the value.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="key">The value's key.</param>
		/// <typeparam name="TValue">The 1st type parameter.</typeparam>
		void RemoveValue<TValue> (string key);
	}
}