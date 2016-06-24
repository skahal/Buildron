using System;

namespace Buildron.Domain
{
    public interface ICIServerService
    {
        /// <summary>
        /// Occurs when continuous integration server status has changed.
        /// </summary>
        event EventHandler<CIServerStatusChangedEventArgs> CIServerStatusChanged;

        /// <summary>
        /// Gets a value indicating whether this <see cref="Buildron.Domain.BuildService"/> is initialized.
        /// </summary>
        /// <value><c>true</c> if initialized; otherwise, <c>false</c>.</value>
        bool Initialized { get; }

        void Initialize(IBuildsProvider buildsProvider);

        /// <summary>
        /// Authenticates the user.
        /// </summary>
        /// <param name="user">User.</param>
        void AuthenticateUser(UserBase user);

        CIServer GetCIServer();

        void SaveCIServer(CIServer server);
    }
}