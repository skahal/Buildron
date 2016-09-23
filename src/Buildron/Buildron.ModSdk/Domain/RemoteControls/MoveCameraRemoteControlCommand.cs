using UnityEngine;

namespace Buildron.Domain.RemoteControls
{
	/// <summary>
	/// Move camera remote control command.
	/// </summary>
	public class MoveCameraRemoteControlCommand : IRemoteControlCommand
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Buildron.Domain.RemoteControls.MoveCameraRemoteControlCommand"/> class.
		/// </summary>
		/// <param name="direction">The move direction.</param>
		public MoveCameraRemoteControlCommand (Vector3 direction)
		{
			Direction = direction;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the direction.
		/// </summary>
		/// <value>The direction.</value>
		public Vector3 Direction { get; private set; }
		#endregion
	}
}

