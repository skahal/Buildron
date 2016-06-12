using System;

namespace Buildron.Domain
{
    /// <summary>
    /// Base class to arguments for CI Server events.
    /// </summary>
    public abstract class CIServerEventArgsBase: EventArgs
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.CIServerdEventArgsBase"/> class.
		/// </summary>
		/// <param name="build">The Continuous Integration server.</param>
		protected CIServerEventArgsBase(CIServer server)
		{
			Server = server;
		}
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the server.
        /// </summary>
		public CIServer Server { get; private set; }
		#endregion
	}
}