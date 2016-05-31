#region Usings
using System;
using System.Collections.Generic;
using Buildron.Domain;
#endregion

namespace Buildron.Domain
{
	public enum AuthenticationRequirement {
		Always,
		Optional,
		Never
	}
	
	public interface IBuildsProvider
	{
		#region Events
		event EventHandler<BuildUpdatedEventArgs> BuildUpdated;
		event EventHandler BuildsRefreshed;
		event EventHandler ServerUp;
		event EventHandler ServerDown;
		event EventHandler UserAuthenticationSuccessful;
		event EventHandler UserAuthenticationFailed;
		#endregion
		
		#region Properties
		string Name { get; }
		AuthenticationRequirement AuthenticationRequirement { get; }
		string AuthenticationTip { get; }
		#endregion
	
		#region Methods
		void RefreshAllBuilds();
		void RunBuild(User user, Build build);
		void StopBuild(User user, Build build);
		void AuthenticateUser(User user);
		#endregion
	}
}

