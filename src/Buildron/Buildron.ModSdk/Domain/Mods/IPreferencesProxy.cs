namespace Buildron.Domain.Mods
{
	/// <summary>
	/// Defines an interface to a preferences proxy.
	/// </summary>
	public interface IPreferencesProxy : IDataProxy
    {
		/// <summary>
		/// Register the specified preferences to be available for the users.
		/// </summary>
		/// <remarks>
		/// This method should be called on mod initialization to register mod's preferences.
		/// </remarks>
		/// <param name="preferences">The preferences.</param>
        void Register(params Preference[] preferences);
    }
}