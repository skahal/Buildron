using System;
using System.Collections.Generic;
using System.Linq;
using Skahal.Common;
using Skahal.Logging;
using UnityEngine;

namespace Buildron.Domain
{
    /// <summary>
    /// Domain service to user from continuous integration server.
    /// </summary>
    public class UserService : IUserService
    {
        #region Events
        /// <summary>
        /// Occurs when an user is found.
        /// </summary>
        public event EventHandler<UserFoundEventArgs> UserFound;

        /// <summary>
        /// Occurs when an user is updated.
        /// </summary>
        public event EventHandler<UserUpdatedEventArgs> UserUpdated;

        /// <summary>
        /// Occurs when an user triggered a build.
        /// </summary>
        public event EventHandler<UserTriggeredBuildEventArgs> UserTriggeredBuild;

        /// <summary>
        /// Occurs when an user is removed.
        /// </summary>
        public event EventHandler<UserRemovedEventArgs> UserRemoved;

        /// <summary>
        /// Occurs when an user authentication is completed.
        /// </summary>
        public event EventHandler<UserAuthenticationCompletedEventArgs> UserAuthenticationCompleted;
        #endregion

        #region Fields
        private IUserAvatarProvider[] m_humanUserAvatarProviders;
        private IUserAvatarProvider[] m_nonHumanUserAvatarProviders;
        private ISHLogStrategy m_log;
        #endregion

        #region Constructors               
        /// <summary>
        /// Inicia uma nova inst�ncia da classe <see cref="UserService"/>.
        /// </summary>
        /// <param name="humanUserAvatarProviders">The human user avatar providers.</param>
        /// <param name="nonHumanAvatarProviders">The non human avatar providers.</param>
        /// <param name="log">The log strategy.</param>
        public UserService(
            IUserAvatarProvider[] humanUserAvatarProviders, 
            IUserAvatarProvider[] nonHumanAvatarProviders,
            ISHLogStrategy log)
        {
            Users = new List<User>();
            m_humanUserAvatarProviders = humanUserAvatarProviders;
            m_nonHumanUserAvatarProviders = nonHumanAvatarProviders;
            m_log = log;
        }
        #endregion

        #region Properties
	    /// <summary>
        /// Gets the users.
        /// </summary>
        public IList<User> Users { get; private set; }
        #endregion

        #region Methods
		/// <summary>
		/// Listens the builds provider.
		/// </summary>
		/// <param name="buildsProvider">Builds provider.</param>
		public void ListenBuildsProvider(IBuildsProvider buildsProvider) 
		{
			var serviceSender = typeof(UserService);
			var usersInLastBuildsUpdate = new List<User>();
		
			buildsProvider.BuildUpdated += (sender, e) =>
			{
				var user = e.Build.TriggeredBy;

				if (user != null)
				{
                    var previousUser = Users.FirstOrDefault(f => f == user);

                    if (previousUser == null)
                    {
                        // New user found.
                        Users.Add(user);
                        UserFound.Raise(serviceSender, new UserFoundEventArgs(user));
                        RaiseUserTriggeredBuildEvents(serviceSender, user, user.Builds);                       
					}
					else
					{
                        var triggeredBuilds = user.Builds.Except(previousUser.Builds);
                        RaiseUserTriggeredBuildEvents(serviceSender, user, triggeredBuilds);

                        Users.Remove(previousUser);
                        Users.Add(user);
                    }

					UserUpdated.Raise(serviceSender, new UserUpdatedEventArgs(user));
					usersInLastBuildsUpdate.Add(user);
				}
			};

			buildsProvider.BuildsRefreshed += delegate
			{
				var removedUsers = Users.Except(usersInLastBuildsUpdate).ToArray();

				m_log.Warning("UserService.BuildsRefreshed: there is {0} users and {1} were refreshed. {2} will be removed", Users.Count, usersInLastBuildsUpdate.Count(), removedUsers.Length);

				foreach (var user in removedUsers)
				{
					Users.Remove(user);
					UserRemoved.Raise(typeof(BuildService), new UserRemovedEventArgs(user));
				}

				usersInLastBuildsUpdate.Clear();
			};

			buildsProvider.UserAuthenticationSuccessful += delegate {
				// TODO: change buildsProvider.UserAuthenticationSuccessful to pass user.
				UserAuthenticationCompleted.Raise(this, new UserAuthenticationCompletedEventArgs(null, true));                
			};

			buildsProvider.UserAuthenticationFailed += delegate {
				UserAuthenticationCompleted.Raise(this, new UserAuthenticationCompletedEventArgs(null, false));
			};
		}

        /// <summary>
        /// Gets the user photo.
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="photoReceived">Photo received callback.</param>
        public void GetUserPhoto(User user, Action<Texture2D> photoReceived)
        {
            if (user != null)
            {
                if (user.Kind == UserKind.Human)
                {
                    GetUserPhoto(user, photoReceived, m_humanUserAvatarProviders);
                }
                else {
                    GetUserPhoto(user, photoReceived, m_nonHumanUserAvatarProviders);
                }
            }
        }

        private void GetUserPhoto(User user, Action<Texture2D> photoReceived, IUserAvatarProvider[] providersChain, int providerStartIndex = 0)
        {
            if (providerStartIndex < providersChain.Length)
            {
                providersChain[providerStartIndex].GetUserPhoto(user, (photo) =>
                {
                    if (photo == null)
                    {
                        GetUserPhoto(user, photoReceived, providersChain, ++providerStartIndex);
                    }
                    else
                    {
                        photoReceived(photo);
                    }
                });
            }
            else
            {
                photoReceived(null);
            }
        }

        private void RaiseUserTriggeredBuildEvents(Type serviceSender, User user, IEnumerable<Build> triggeredBuilds)
        {
            foreach (var build in triggeredBuilds)
            {
                UserTriggeredBuild.Raise(serviceSender, new UserTriggeredBuildEventArgs(user, build));
            }
        }
        #endregion
    }
}