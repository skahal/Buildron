using System;
using Skahal.Common;
using Skahal.Infrastructure.Framework.Commons;

namespace Buildron.Domain.Versions
{
	/// <summary>
	/// Buildron's version service.
	/// </summary>
	public class VersionService : IVersionService
	{	
		#region Fields
		private IVersionClient s_versionClient;
		private IVersionRepository s_versionRepository;
		#endregion
		
		#region Events
		/// <summary>
		/// Occurs when client is registered.
		/// </summary>
		public event EventHandler<ClientRegisteredEventArgs> ClientRegistered;

		/// <summary>
		/// Occurs when update info received.
		/// </summary>
		public event EventHandler<UpdateInfoReceivedEventArgs> UpdateInfoReceived;
		#endregion
		
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Versions.VersionService"/> class.
		/// </summary>
		/// <param name="versionClient">Version client.</param>
		/// <param name="versionRepository">Version repository.</param>
		public VersionService (IVersionClient versionClient, IVersionRepository versionRepository)
		{
			s_versionClient = versionClient;
			s_versionRepository = versionRepository;
			
			s_versionClient.ClientRegistered += (sender, e) => {
				
				var version = new Version ();
				version.ClientId = e.ClientId;
				version.ClientInstance = e.ClientInstance;
				s_versionRepository.Save (version);
				
				ClientRegistered.Raise (this, e);
			};
			
			s_versionClient.UpdateInfoReceived += (sender, e) => UpdateInfoReceived.Raise (this, e);
		}
		#endregion

		#region Methods
		public void Register(ClientKind kind, SHDeviceFamily device)
		{
			var version = s_versionRepository.Find();
			
			if (version == null)
			{
				s_versionClient.RegisterClient(Guid.NewGuid().ToString(), kind, device);
			}
			else
			{
				var args = new ClientRegisteredEventArgs(version.ClientId, version.ClientInstance);
				
				ClientRegistered.Raise(typeof(VersionService), args);
			}

		}
		
		public void CheckUpdates(ClientKind kind, SHDeviceFamily device)
		{
			var version = GetVersion();
			
			if (version != null)
			{
				s_versionClient.CheckUpdates(version.ClientId, kind, device);	
			}
		
		}
		
		public Version GetVersion()
		{
			return s_versionRepository.Find();
		}
		#endregion
	}
}