using System;
using Buildron.Domain.RemoteControls;

namespace Buildron.Domain.RemoteControls
{
	/// <summary>
	/// Arguments for remote control command received events.
	/// </summary>
	public class RemoteControlCommandReceivedEventArgs : EventArgs
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.ModSdk.RemoteControlMessageReceivedEventArgs"/> class.
		/// </summary>
		/// <param name="command">The remote control command.</param>
		public RemoteControlCommandReceivedEventArgs(IRemoteControl remoteControl, IRemoteControlCommand command)
		{
			RemoteControl = remoteControl;
			Command = command;
		}
		#endregion

		#region Properties        
		public IRemoteControl RemoteControl { get; private set; }
		public IRemoteControlCommand Command { get; private set; }
		#endregion
	}
}