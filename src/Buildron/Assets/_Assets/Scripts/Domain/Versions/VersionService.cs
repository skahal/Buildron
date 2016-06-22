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
		private readonly IVersionClient m_versionClient;
		private readonly IVersionRepository m_versionRepository;
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
			m_versionClient = versionClient;
			m_versionRepository = versionRepository;
			
			m_versionClient.ClientRegistered += (sender, e) => {
				
				var version = new Version ();
				version.ClientId = e.ClientId;
				version.ClientInstance = e.ClientInstance;
				m_versionRepository.Save (version);
				
				ClientRegistered.Raise (this, e);
			};

            m_versionClient.UpdateInfoReceived += (sender, e) =>
            {
                UpdateInfoReceived.Raise(this, e);
            };
		}
		#endregion

		#region Methods
		/// <summary>
		/// Register the specified Buildron's client.
		/// </summary>
		/// <param name="buildron">Buildron.</param>
		/// <param name="family">Family.</param>
		/// <param name="kind">Kind.</param>
		/// <param name="device">Device.</param>
		public void Register(ClientKind kind, SHDeviceFamily device)
		{
			var version = m_versionRepository.Find();
			
			if (version == null)
			{
				m_versionClient.RegisterClient(Guid.NewGuid().ToString(), kind, device);
			}
			else
			{
				var args = new ClientRegisteredEventArgs(version.ClientId, version.ClientInstance);
				
				ClientRegistered.Raise(typeof(VersionService), args);
			}

		}

		/// <summary>
		/// Checks available updates to specified Buildron's client.
		/// </summary>
		/// <param name="buildron">Buildron.</param>
		/// <param name="family">Family.</param>
		/// <param name="kind">Kind.</param>
		/// <param name="device">Device.</param>
		public void CheckUpdates(ClientKind kind, SHDeviceFamily device)
		{
			var version = GetVersion();
			
			if (version != null)
			{
				m_versionClient.CheckUpdates(version.ClientId, kind, device);	
			}
		
		}

		/// <summary>
		/// Gets the version.
		/// </summary>
		/// <returns>The version.</returns>
		public Version GetVersion()
		{
			return m_versionRepository.Find();
		}
		#endregion
	}
}