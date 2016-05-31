using UnityEngine;
using Buildron.Infrastructure.BuildsProvider.TeamCity;

namespace Buildron.Domain
{
	public static class CIServerService
	{
		#region Constants
		private const string ServerTypeKey = "CIServer.ServerType";
		private const string TitleKey = "CIServer.Title";
		private const string IPKey = "CIServer.IP";
		private const string DomainKey = "CIServer.Domain";
		private const string UserNameKey = "CIServer.UserName";
		private const string PasswordKey = "CIServer.Password";
		private const string RefreshSecondsKey = "CIServer.RefreshSeconds";
		private const string FxSoundsEnabledKey = "CIServer.FxSoundsEnabled";
		private const string HistoryTotemEnabledKey = "CIServer.HistoryTotemEnabled";
		private const string BuildsTotemsNumberKey = "CIServer.BuildsTotemsNumber";
		#endregion
		
		#region Methods
		public static void SaveCIServer (CIServer server)
		{
			PlayerPrefs.SetInt (ServerTypeKey, (int)server.ServerType);
			PlayerPrefs.SetString (TitleKey, server.Title);
			PlayerPrefs.SetString (IPKey, server.IP);
			PlayerPrefs.SetString (DomainKey, server.Domain);
			PlayerPrefs.SetString (UserNameKey, server.UserName);
			PlayerPrefs.SetString (PasswordKey, server.Password);
			PlayerPrefs.SetFloat (RefreshSecondsKey, server.RefreshSeconds);
			PlayerPrefs.SetInt (FxSoundsEnabledKey, server.FxSoundsEnabled ? 1 : 0);
			PlayerPrefs.SetInt (HistoryTotemEnabledKey, server.HistoryTotemEnabled ? 1 : 0);
			PlayerPrefs.SetInt (BuildsTotemsNumberKey, (int)server.BuildsTotemsNumber);
		}
		
		public static CIServer GetCIServer ()
		{
			var server = new CIServer ();
			server.ServerType = (CIServerType)PlayerPrefs.GetInt (ServerTypeKey, (int)CIServerType.TeamCity);
			server.Title = PlayerPrefs.GetString (TitleKey, "Buildron");
			server.IP = PlayerPrefs.GetString (IPKey, string.Empty);
			server.UserName = PlayerPrefs.GetString (UserNameKey, string.Empty);
			server.Domain = PlayerPrefs.GetString (DomainKey, string.Empty);
			server.Password = PlayerPrefs.GetString (PasswordKey, string.Empty);
			server.RefreshSeconds = PlayerPrefs.GetFloat (RefreshSecondsKey, 10);
			server.FxSoundsEnabled = PlayerPrefs.GetInt (FxSoundsEnabledKey, 1) == 1;
			server.HistoryTotemEnabled = PlayerPrefs.GetInt (HistoryTotemEnabledKey, 1) == 1;
			server.BuildsTotemsNumber = PlayerPrefs.GetInt (BuildsTotemsNumberKey, 2);
			
			return server;
		}
		#endregion
	}
}