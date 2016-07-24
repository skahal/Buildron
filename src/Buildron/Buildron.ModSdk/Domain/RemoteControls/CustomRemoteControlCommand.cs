using System;

namespace Buildron.Domain.RemoteControls
{
	public class CustomRemoteControlCommand : IRemoteControlCommand
	{
		public CustomRemoteControlCommand (string name)
		{
			Name = name;
		}

		public string Name { get; private set; }
	}
}

