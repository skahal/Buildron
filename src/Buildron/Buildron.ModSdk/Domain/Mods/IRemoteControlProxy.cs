using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buildron.Domain.RemoteControls;

namespace Buildron.Domain.Mods
{
    public interface IRemoteControlProxy
    {
        IRemoteControl Current { get; }

        bool ReceiveCommand(IRemoteControlCommand command);
    }
}
