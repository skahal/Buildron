namespace Buildron.Domain.CIServers
{
    /// <summary>
    /// Arguments for continous integration server connected events.
    /// </summary>
	public class CIServerConnectedEventArgs : CIServerEventArgsBase
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.CIServers.CIServerConnectedEventArgs"/> class.
		/// </summary>
		/// <param name="server">Server.</param>
		public CIServerConnectedEventArgs (ICIServer server)
			: base (server)
		{
		}
        #endregion
	}
}