using System;

namespace Buildron.Domain.Versions
{
	#region Enums
	/// <summary>
	/// Client kind.
	/// </summary>
	public enum ClientKind 
	{
		/// <summary>
		/// Buildron client.
		/// </summary>
		Buildron,

		/// <summary>
		/// RC client.
		/// </summary>
		RemoteControl
	}
	#endregion

	/// <summary>
	/// Defines an interface for version client.
	/// </summary>
	public interface IVersionClient
	{
		#region Events
		/// <summary>
		/// Occurs when client is registered.
		/// </summary>
		event EventHandler<ClientRegisteredEventArgs> ClientRegistered;

		/// <summary>
		/// Occurs when update info is received.
		/// </summary>
		event EventHandler<UpdateInfoReceivedEventArgs> UpdateInfoReceived;
		#endregion
		
		#region Methods
		/// <summary>
		/// Registers the client.
		/// </summary>
		/// <param name="clientId">Client identifier.</param>
		/// <param name="kind">Kind.</param>
		/// <param name="device">Device.</param>
		void RegisterClient(string clientId, ClientKind kind, SHDeviceFamily device);

		/// <summary>
		/// Checks the updates.
		/// </summary>
		/// <param name="clientId">Client identifier.</param>
		/// <param name="kind">Kind.</param>
		/// <param name="device">Device.</param>
		void CheckUpdates(string clientId, ClientKind kind, SHDeviceFamily device);
		#endregion
	}
}