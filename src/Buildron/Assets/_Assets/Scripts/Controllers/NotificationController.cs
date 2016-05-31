#region Usings
using UnityEngine;
using System.Collections;
using Buildron.Domain.Notifications;
using Buildron.Domain.Versions;
using Buildron.Domain;
using Skahal.Logging;
using UnityEngine.UI;


#endregion

/// <summary>
/// Notification controller for messages from Builcron back-end
/// </summary>
[AddComponentMenu("Buildron/Controllers/NotificationController")]
public class NotificationController : MonoBehaviour 
{
	#region Editor properties
	public float MinutesCheckNotificationsInterval = 10;
	public float MinutesUntilHideNotification = 2;
	public Text NotificationLabel;
	#endregion
	
	#region Methods
	private void Awake ()
	{
		NotificationService.NotificationReceived += delegate(object sender, NotificationReceivedEventArgs e) {		
			SHLog.Debug ("NotificationController: notification received '{0}'.", e.Notification.Text);
			if (!RemoteControlService.HasRemoteControlConnected) {
				
							
				// Someday a RC has connect to this Buildron instance, so stop to boring about RC ;)
				if (e.Notification.Name.Equals ("DOWNLOAD_RC") && RemoteControlService.HasRemoteControlConnectedSomeDay) {
					return;
				}
	
				NotificationLabel.text = e.Notification.Text;
				NotificationLabel.enabled = true;
			
				StartCoroutine (DelayHideNotification ());
			}
		};
		
		Messenger.Register (gameObject, "OnRemoteControlConnected");
		
		StartCoroutine (CheckNotification ());
	}
	
	private IEnumerator CheckNotification ()
	{
		while (true) {
			SHLog.Debug ("NotificationController: next check in {0} minutes", MinutesCheckNotificationsInterval);
			yield return new WaitForSeconds(60f * MinutesCheckNotificationsInterval);
			
			SHLog.Debug ("NotificationController: checking notifications...");
			NotificationService.CheckNotifications (ClientKind.Buildron, SHDevice.Family);
		}
	}
	
	private IEnumerator DelayHideNotification ()
	{
		yield return new WaitForSeconds(60f * MinutesUntilHideNotification);
		NotificationLabel.enabled = false;
	}
	
	private void OnRemoteControlConnected()
	{
		NotificationLabel.enabled = false;
	}
	#endregion
}