using System;

namespace Buildron.Domain.Versions
{
	/// <summary>
	/// Client registered event arguments.
	/// </summary>
	public class ClientRegisteredEventArgs : EventArgs
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Versions.ClientRegisteredEventArgs"/> class.
		/// </summary>
		/// <param name="clientId">Client identifier.</param>
		/// <param name="clientInstance">Client instance.</param>
		public ClientRegisteredEventArgs (string clientId, int clientInstance)
		{
			ClientId = clientId;
			ClientInstance = clientInstance;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the client identifier.
		/// </summary>
		/// <value>The client identifier.</value>
		public string ClientId { get; private set; }

		/// <summary>
		/// Gets the client instance.
		/// </summary>
		/// <value>The client instance.</value>
		public int ClientInstance { get; private set; }
		#endregion
	}
}