using System;

namespace Buildron.Domain.Servers
{
	/// <summary>
	/// Defines a interface to a server service.
	/// </summary>
	public interface IServerService
	{
		#region Events
		/// <summary>
		/// Occurs when server state is updated.
		/// </summary>
		event EventHandler<ServerStateUpdatedEventArgs> StateUpdated;
		#endregion

		#region Methods
		/// <summary>
		/// Saves the state.
		/// </summary>
		/// <param name="state">The state to save.></param>
		void SaveState(ServerState state);

		/// <summary>
		/// Gets the state.
		/// </summary>
		/// <returns>The state.</returns>
		ServerState GetState();
	    #endregion
	}
}