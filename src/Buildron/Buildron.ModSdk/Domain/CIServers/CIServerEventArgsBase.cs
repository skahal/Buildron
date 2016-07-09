using System;

namespace Buildron.Domain.CIServers
{
    /// <summary>
    /// Base class to arguments for CI Server events.
    /// </summary>
    public abstract class CIServerEventArgsBase: EventArgs
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.CIServers.CIServerdEventArgsBase"/> class.
		/// </summary>
		/// <param name="build">The Continuous Integration server.</param>
		protected CIServerEventArgsBase(ICIServer server)
		{
			Server = server;
		}
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the server.
        /// </summary>
		public ICIServer Server { get; private set; }
		#endregion
	}
}