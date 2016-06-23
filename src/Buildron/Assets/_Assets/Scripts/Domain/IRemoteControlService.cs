using System;

namespace Buildron.Domain
{
    public interface IRemoteControlService
    {
        bool HasRemoteControlConnected { get; }
        bool HasRemoteControlConnectedSomeDay { get; set; }

        event EventHandler<RemoteControlChangedEventArgs> RemoteControlChanged;

        void ConnectRemoteControl(RemoteControl rcToConnect);
        void DisconnectRemoteControl();
        RemoteControl GetConnectedRemoteControl();
    }
}