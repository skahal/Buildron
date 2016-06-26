using System;

namespace Buildron.Domain.RemoteControls
{
	/// <summary>
	/// Define an interface to remote control service.
	/// </summary>
    public interface IRemoteControlService
    {
		/// <summary>
		/// Occurs when remote control changed.
		/// </summary>
		event EventHandler<RemoteControlChangedEventArgs> RemoteControlChanged;

		/// <summary>
		/// Gets a value indicating whether has remote control connected.
		/// </summary>
        bool HasRemoteControlConnected { get; }

		/// <summary>
		/// Gets a value indicating whetherhas remote control connected some day.
		/// </summary>
	    bool HasRemoteControlConnectedSomeDay { get; }
	
		/// <summary>
		/// Connects the remote control.
		/// </summary>
		/// <param name="rcToConnect">RC to connect.</param>
        void ConnectRemoteControl(RemoteControl rcToConnect);

		/// <summary>
		/// Disconnects the remote control.
		/// </summary>
        void DisconnectRemoteControl();

		/// <summary>
		/// Gets the connected remote control.
		/// </summary>
	    RemoteControl GetConnectedRemoteControl();
    }
}