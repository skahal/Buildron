using Buildron.Domain;
using UnityEngine;
#region Usings
#endregion

namespace Buildron.Domain
{
	/// <summary>
	/// Remote control service.
	/// </summary>
	public static class RemoteControlService
	{
		#region Fields
		private static RemoteControl s_remoteControl;
		#endregion
		
		#region Constructors
		static RemoteControlService ()
		{
			BuildService.UserAuthenticationFailed += delegate {
				s_remoteControl = null;
			};
		}
		#endregion
		
		#region Properties
		public static bool HasRemoteControlConnectedSomeDay {
			get {
				return PlayerPrefs.GetInt ("_HasRemoteControlConnectedSomeDay_", 0) == 1;
			}
			
			set {
				PlayerPrefs.SetInt ("_HasRemoteControlConnectedSomeDay_", value ? 1 : 0);
			}
		}
		
		public static bool HasRemoteControlConnected
		{
			get {
				return s_remoteControl != null;
			}
		}
		#endregion
		
		#region Methods
		public static void ConnectRemoteControl (RemoteControl rcToConnect)
		{
			s_remoteControl = rcToConnect;
		
			BuildService.AuthenticateUser (rcToConnect);
			HasRemoteControlConnectedSomeDay = true;
		}
		
		public static void DisconnectRemoteControl ()
		{
			s_remoteControl = null;
		}
		
		public static RemoteControl GetConnectedRemoteControl ()
		{
			return s_remoteControl;
		}
		#endregion
	}
}