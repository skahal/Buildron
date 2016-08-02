using System;
using Buildron.Domain.Mods;
using UnityEngine;
using Skahal.Common;
using System.Linq;
using Buildron.Domain.RemoteControls;

namespace Buildron.Infrastructure.RemoteControlProxies
{
    /// <summary>
    /// Mod remote control proxy.
    /// </summary>
    public class ModRemoteControlProxy : IRemoteControlProxy
    {
        private IRemoteControlService m_remoteControlService;

        public ModRemoteControlProxy(IRemoteControlService remoteControlService)
        {
            m_remoteControlService = remoteControlService;
        }

        public IRemoteControl Current
        {
            get
            {
                return m_remoteControlService.GetConnectedRemoteControl();
            }
        }

        public bool ReceiveCommand(IRemoteControlCommand command)
        {
            return m_remoteControlService.ReceiveCommand(command);
        }
    }
}
