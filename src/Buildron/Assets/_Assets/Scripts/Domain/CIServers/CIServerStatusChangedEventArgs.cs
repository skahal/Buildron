namespace Buildron.Domain.CIServers
{
    /// <summary>
    /// Arguments for continous integration server status changed events.
    /// </summary>
	public class CIServerStatusChangedEventArgs : CIServerEventArgsBase
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.CIServers.CIServerStatusChangedEventArgs"/> class.
		/// </summary>
		/// <param name="server">Server.</param>
		public CIServerStatusChangedEventArgs (ICIServer server)
			: base (server)
		{
		}
        #endregion
	}
}