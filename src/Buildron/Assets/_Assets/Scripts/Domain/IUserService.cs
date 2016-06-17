using System;
using System.Collections.Generic;
using UnityEngine;

namespace Buildron.Domain
{
    public interface IUserService
    {
        IList<User> Users { get; }

        event EventHandler<UserFoundEventArgs> UserFound;
        event EventHandler<UserRemovedEventArgs> UserRemoved;
        event EventHandler<UserTriggeredBuildEventArgs> UserTriggeredBuild;
        event EventHandler<UserUpdatedEventArgs> UserUpdated;
        
        /// <summary>
        /// Occurs when an user authentication is completed.
        /// </summary>
        event EventHandler<UserAuthenticationCompletedEventArgs> UserAuthenticationCompleted;

		/// <summary>
		/// Listens the builds provider.
		/// </summary>
		/// <param name="buildsProvider">Builds provider.</param>
		void ListenBuildsProvider(IBuildsProvider buildsProvider);

        void GetUserPhoto(User user, Action<Texture2D> photoReceived);
    }
}