namespace Buildron.Domain
{
	/// <summary>
	/// Defines a interface to a server service.
	/// </summary>
	public interface IServerService
	{
		#region Methods
		/// <summary>
		/// Saves the server state.
		/// </summary>
		void SaveState();
	    #endregion
	}
}