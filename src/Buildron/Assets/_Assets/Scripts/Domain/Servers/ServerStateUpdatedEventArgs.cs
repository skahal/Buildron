using System;

namespace Buildron.Domain.Servers
{
    /// <summary>
    /// Arguments for user removed events.
    /// </summary>
    public class ServerStateUpdatedEventArgs : EventArgs
	{
        #region Constructors
        /// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Servers.ServerStateUpdatedEventArgs"/> class.
        /// </summary>
        /// <param name="state">the state updated.</param>
		public ServerStateUpdatedEventArgs(ServerState state)
		{
			State = state;
		}
        #endregion

		#region Properties
		/// <summary>
		/// Gets the state.
		/// </summary>
		/// <value>The state.</value>
		public ServerState State { get; private set; }
		#endregion
	}
}