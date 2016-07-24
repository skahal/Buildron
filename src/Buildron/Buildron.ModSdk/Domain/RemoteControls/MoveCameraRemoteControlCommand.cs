using System;
using UnityEngine;

namespace Buildron.Domain.RemoteControls
{
	public class MoveCameraRemoteControlCommand : IRemoteControlCommand
	{
		public MoveCameraRemoteControlCommand (Vector3 direction)
		{
			Direction = direction;
		}

		public Vector3 Direction { get; private set; }
	}
}

