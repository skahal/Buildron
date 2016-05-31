#region Usings
using UnityEngine;
using System.Collections;
using System;
#endregion

namespace Buildron.Domain
{
	/// <summary>
	/// Represents the current Buildron server state.
	/// </summary>
	[Serializable]
	public sealed class ServerState
	{
		#region Constructors
		static ServerState ()
		{
			Instance = new ServerState ();
		}
		
		private ServerState ()
		{
			BuildFilter = new BuildFilter ();
		}
		#endregion
		
		#region Properties
		public static ServerState Instance { get; private set; }
		public BuildFilter BuildFilter { get; internal set; }
		public bool IsSorting { get; set; }
		public bool HasHistory { get; set; }
		public bool IsShowingHistory { get; set; }
		public float CameraPositionZ { get; set; }
		#endregion
	}
}