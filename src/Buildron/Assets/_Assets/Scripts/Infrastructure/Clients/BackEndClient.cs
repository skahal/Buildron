#region Usings
using Buildron.Domain.Versions;
using UnityEngine;
using System.Collections;
using Skahal.Common;
using Buildron.Domain.Notifications;
#endregion

namespace Buildron.Infrastructure.Clients
{
	public class BackEndClient : IVersionClient, INotificationClient
	{
		#region Constants
		private const string BaseUrl = "http://buildronback-end.apphb.com/Services/Clients/{0}.ashx?id={1}&device={2}&kind={3}&version={4}";
		#endregion
		
		#region Fields
		private System.EventHandler<ClientRegisteredEventArgs> m_clientRegistered;
		private System.EventHandler<UpdateInfoReceivedEventArgs> m_updateInfoReceived;
		private System.EventHandler<NotificationReceivedEventArgs> m_notificationReceived;
		#endregion
		
		#region IVersionClient implementation
		public event System.EventHandler<ClientRegisteredEventArgs> ClientRegistered {
			add { m_clientRegistered += value; }
			remove { m_clientRegistered -= value; }	
		}
		
		public event System.EventHandler<UpdateInfoReceivedEventArgs> UpdateInfoReceived {
			add { m_updateInfoReceived += value; }
			remove { m_updateInfoReceived -= value; }
		}	
		
		public void RegisterClient (string clientId, ClientKind kind, SHDeviceFamily device)
		{
			var url = GetUrl ("RegisterClient", clientId, kind, device);
			
			Requester.Instance.GetText (url, (obj) => 
			{
				Hashtable r = obj.hashtableFromJson ();
				var values = (Hashtable)r ["Values"];
				
				var clientInstance = System.Convert.ToInt32 (values ["TOTAL_CLIENTS_COUNT"]);
				var args = new ClientRegisteredEventArgs (clientId, clientInstance);
				m_clientRegistered.Raise (this, args);
			});
		}

		public void CheckUpdates (string clientId, ClientKind kind, SHDeviceFamily device)
		{
			var url = GetUrl ("CheckUpdates", clientId, kind, device);
			
			Requester.Instance.GetText (url, (obj) => 
			{
				Hashtable r = obj.hashtableFromJson ();

                if (r != null)
                {
                    var values = (Hashtable)r["Values"];
                    var updateInfo = new VersionUpdateInfo();
                    updateInfo.Description = values["UPDATE_TEXT"].ToString();

                    if (values.Count > 1)
                    {
                        updateInfo.Url = values["UPDATE_LINK"].ToString();
                    }

                    var args = new UpdateInfoReceivedEventArgs(updateInfo);
                    m_updateInfoReceived.Raise(this, args);
                }
			});
		}
		
		private static string GetUrl(string command, string clientId, ClientKind kind, SHDeviceFamily device)
		{
			return string.Format(
				BaseUrl,
				command, 
				clientId,
				device,
				kind,
				SHGameInfo.Version);
				
		}
		#endregion

		#region INotificationClient implementation
		public event System.EventHandler<NotificationReceivedEventArgs> NotificationReceived {
			add { m_notificationReceived += value; }
			remove { m_notificationReceived -= value; }	
		}
			
		public void CheckNotifications(string clientId, ClientKind kind, SHDeviceFamily device)
		{
			var url = GetUrl("CheckNotifications", clientId, kind, device);
			
			Requester.Instance.GetText(url, (obj) => 
			{
				Hashtable r = obj.hashtableFromJson();
				var values = (Hashtable) r ["Values"];
				
				var notification = new Notification(r["Name"].ToString(), values ["NOTIFICATION_TEXT"].ToString());
				
				if (!string.IsNullOrEmpty(notification.Text))
				{
					m_notificationReceived.Raise(this, new NotificationReceivedEventArgs(notification));
				}
			});
		}
		#endregion
	}
}