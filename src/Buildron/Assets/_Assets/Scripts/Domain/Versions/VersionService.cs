using System;
using Skahal.Common;
using Skahal.Infrastructure.Framework.Commons;

namespace Buildron.Domain.Versions
{
	public static class VersionService
	{	
		#region Fields
		private static IVersionClient s_versionClient;
		private static IVersionRepository s_versionRepository;
		private static System.EventHandler<ClientRegisteredEventArgs> s_clientRegistered;
		private static System.EventHandler<UpdateInfoReceivedEventArgs> s_updateInfoReceived;
		#endregion
		
		#region Events
		public static event System.EventHandler<ClientRegisteredEventArgs> ClientRegistered {
			add { s_clientRegistered += value; }
			remove { s_clientRegistered -= value; }	
		}
		
		public static event System.EventHandler<UpdateInfoReceivedEventArgs> UpdateInfoReceived {
			add { s_updateInfoReceived += value; }
			remove { s_updateInfoReceived -= value; }
		}
		#endregion
		
		#region Methods
		public static void Initialize ()
		{
			s_clientRegistered = null;
			s_updateInfoReceived = null;
			
			s_versionClient = DependencyService.Create<IVersionClient> ();
			s_versionRepository = DependencyService.Create<IVersionRepository> ();
			
			s_versionClient.ClientRegistered += delegate(object sender, ClientRegisteredEventArgs e) {
				
				var version = new Version ();
				version.ClientId = e.ClientId;
				version.ClientInstance = e.ClientInstance;
				s_versionRepository.Save (version);
				
				s_clientRegistered.Raise (typeof(VersionService), e);
			};
			
			s_versionClient.UpdateInfoReceived += delegate(object sender, UpdateInfoReceivedEventArgs e) {
				s_updateInfoReceived.Raise (typeof(VersionService), e);
			};
		}
		
		public static void Register(ClientKind kind, SHDeviceFamily device)
		{
			var version = s_versionRepository.Find();
			
			if (version == null)
			{
				s_versionClient.RegisterClient(Guid.NewGuid().ToString(), kind, device);
			}
			else
			{
				var args = new ClientRegisteredEventArgs(version.ClientId, version.ClientInstance);
				
				s_clientRegistered.Raise(typeof(VersionService), args);
			}

		}
		
		public static void CheckUpdates(ClientKind kind, SHDeviceFamily device)
		{
			var version = GetVersion();
			
			if (version != null)
			{
				s_versionClient.CheckUpdates(version.ClientId, kind, device);	
			}
		
		}
		
		public static Version GetVersion()
		{
			return s_versionRepository.Find();
		}
		#endregion
	}
}