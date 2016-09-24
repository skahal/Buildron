using System;
using Buildron.Domain.Mods;
using Buildron.Domain.RemoteControls;

public class SimulatorRemoteControlProxy : IRemoteControlProxy
{
	public IRemoteControl Current
	{
		get
		{
			return new SimulatorRemoteControl();
		}
	}

	public bool ReceiveCommand(IRemoteControlCommand command)
	{
		SimulatorModContext.Instance.Log.Debug("RemoteControl.ReceiveCommand: {0}", command.GetType());

		return true;
	}
}

