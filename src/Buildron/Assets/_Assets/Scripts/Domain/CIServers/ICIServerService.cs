using System;
using Buildron.Domain.Builds;
using Buildron.Domain.Users;

namespace Buildron.Domain.CIServers
{
	/// <summary>
	/// Defines an interface to continuous integration server service.
	/// </summary>
    public interface ICIServerService
    {
        /// <summary>
        /// Occurs when continuous integration server status has changed.
        /// </summary>
        event EventHandler<CIServerStatusChangedEventArgs> CIServerStatusChanged;

       	/// <summary>
		/// Gets a value indicating whether this <see cref="Buildron.Domain.CIServers.ICIServerService"/> is initialized.
       	/// </summary>
       	/// <value><c>true</c> if initialized; otherwise, <c>false</c>.</value>
        bool Initialized { get; }

		/// <summary>
		/// Initialize the service.
		/// </summary>
		/// <param name="buildsProvider">Builds provider.</param>
        void Initialize(IBuildsProvider buildsProvider);

        /// <summary>
        /// Authenticates the user.
        /// </summary>
        /// <param name="user">User.</param>
        void AuthenticateUser(IAuthUser user);

		/// <summary>
		/// Gets the CI server.
		/// </summary>
		/// <returns>The CI server.</returns>
        ICIServer GetCIServer();

		/// <summary>
		/// Saves the CI server.
		/// </summary>
		/// <param name="server">Server.</param>
        void SaveCIServer(ICIServer server);
    }
}