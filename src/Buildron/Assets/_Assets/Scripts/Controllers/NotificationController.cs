#region Usings
using UnityEngine;
using System.Collections;
using Buildron.Domain.Notifications;
using Buildron.Domain.Versions;
using Buildron.Domain;
using Skahal.Logging;
using UnityEngine.UI;
using Zenject;


#endregion

/// <summary>
/// Notification controller for messages from Builcron back-end
/// </summary>
[AddComponentMenu("Buildron/Controllers/NotificationController")]
public class NotificationController : MonoBehaviour, IInitializable 
{
	#region Fields
	[Inject]
	private NotificationService m_notificationService;

	[Inject]
	private ISHLogStrategy m_log;
	#endregion

	#region Editor properties
	public float MinutesCheckNotificationsInterval = 10;
	public float MinutesUntilHideNotification = 2;
	public Text NotificationLabel;
	#endregion
	
	#region Methods
	public void Initialize()
	{
		m_notificationService.NotificationReceived += (sender, e) => 
		{		
			m_log.Debug ("NotificationController: notification received '{0}'.", e.Notification.Text);
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
			m_log.Debug ("NotificationController: next check in {0} minutes", MinutesCheckNotificationsInterval);
			yield return new WaitForSeconds(60f * MinutesCheckNotificationsInterval);
			
			m_log.Debug ("NotificationController: checking notifications...");
			m_notificationService.CheckNotifications (ClientKind.Buildron, SHDevice.Family);
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