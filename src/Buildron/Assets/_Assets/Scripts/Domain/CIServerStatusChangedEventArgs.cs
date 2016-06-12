using System;

namespace Buildron.Domain
{
    /// <summary>
    /// Arguments for continous integration server status changed events.
    /// </summary>
	public class CIServerStatusChangedEventArgs : CIServerEventArgsBase
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.CIServerStatusChangedEventArgs"/> class.
		/// </summary>
		/// <param name="server">Server.</param>
		public CIServerStatusChangedEventArgs (CIServer server)
			: base (server)
		{
		}
        #endregion
	}
}