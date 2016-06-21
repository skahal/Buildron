using System;

namespace Buildron.Domain.Versions
{
	/// <summary>
	/// Defines an version service interface.
	/// </summary>
	public interface IVersionService
	{
		/// <summary>
		/// Occurs when client is registered.
		/// </summary>
		event EventHandler<ClientRegisteredEventArgs> ClientRegistered;

		/// <summary>
		/// Occurs when update info received.
		/// </summary>
		event EventHandler<UpdateInfoReceivedEventArgs> UpdateInfoReceived;

		/// <summary>
		/// Register the specified Buildron's client.
		/// </summary>
		/// <param name="buildron">Buildron.</param>
		/// <param name="family">Family.</param>
		void Register (ClientKind buildron, SHDeviceFamily family);

		/// <summary>
		/// Checks available updates to specified Buildron's client.
		/// </summary>
		/// <param name="buildron">Buildron.</param>
		/// <param name="family">Family.</param>
		void CheckUpdates (ClientKind buildron, SHDeviceFamily family);

		/// <summary>
		/// Gets the version.
		/// </summary>
		/// <returns>The version.</returns>
		Version GetVersion ();
	}
}