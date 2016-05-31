
using System;

namespace Buildron.Domain.Versions
{
	public class ClientRegisteredEventArgs : EventArgs
	{
		public ClientRegisteredEventArgs (string clientId, int clientInstance)
		{
			ClientId = clientId;
			ClientInstance = clientInstance;
		}
		
		public string ClientId { get; private set; }
		public int ClientInstance { get; private set; }
	}
}