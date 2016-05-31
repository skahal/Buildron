#region Usings
using System;
#endregion

namespace Buildron.Domain.Versions
{
	#region Enums
	public enum ClientKind 
	{
		Buildron,
		RemoteControl
	}
	#endregion
	
	public interface IVersionClient
	{
		#region Events
		event EventHandler<ClientRegisteredEventArgs> ClientRegistered;
		event EventHandler<UpdateInfoReceivedEventArgs> UpdateInfoReceived;
		#endregion
		
		#region Properties
		void RegisterClient(string clientId, ClientKind kind, SHDeviceFamily device);
		void CheckUpdates(string clientId, ClientKind kind, SHDeviceFamily device);
		#endregion
	}
}