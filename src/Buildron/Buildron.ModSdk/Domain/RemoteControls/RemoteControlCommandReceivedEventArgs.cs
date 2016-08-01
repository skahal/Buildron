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
		/// Initializes a new instance of the <see cref="RemoteControlCommandReceivedEventArgs"/> class.
		/// </summary>
		/// <param name="remoteControl">The remote control.</param>
		/// <param name="command">The remote control command.</param>
		public RemoteControlCommandReceivedEventArgs(IRemoteControl remoteControl, IRemoteControlCommand command)
		{
			RemoteControl = remoteControl;
			Command = command;
		}
		#endregion

		#region Properties        
		/// <summary>
		/// Gets the remote control.
		/// </summary>
		/// <value>The remote control.</value>
		public IRemoteControl RemoteControl { get; private set; }

		/// <summary>
		/// Gets the command.
		/// </summary>
		/// <value>The command.</value>
		public IRemoteControlCommand Command { get; private set; }

		/// <summary>
		/// Gets or sets a value that indicate whether the command should be canceled.
		/// </summary>
		public bool Cancel { get; set; }
		#endregion
	}
}