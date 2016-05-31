#region Usings
using System.Xml;
using Buildron.Domain;
using System.Collections.Generic;
using Skahal.Logging;
#endregion

/// <summary>
/// A parser for BuildUser .
/// </summary>
public static class BuildUserParser
{
	#region Fields
	public static Dictionary<string, BuildUser> s_buildUsers = new Dictionary<string, BuildUser>();
	#endregion
	
	#region Methods
	public static void Reset ()
	{
		s_buildUsers.Clear();	
	}
	
	/// <summary>
	/// Parse a BuildUser from a XmlDocument.
	/// </summary>
	/// <param name='xmlDoc'>
	/// Xml document.
	/// </param>
	public static BuildUser ParseFromTriggered (Build build, XmlDocument xmlDoc)
	{
		BuildUser user = null;
		var triggeredNode = xmlDoc.SelectSingleNode ("build/triggered");
		var userNode = triggeredNode.SelectSingleNode ("user");
		
		if (userNode == null || string.IsNullOrEmpty (userNode.OuterXml)) {
			var changesNode = xmlDoc.SelectSingleNode ("build/changes");
			var triggerDetails = "RETRY BUILD TRIGGER";
			
			if (triggeredNode.Attributes ["details"] != null) {
				triggerDetails = triggeredNode.Attributes ["details"].Value.ToUpperInvariant ();
			}

			var countAttr = changesNode.Attributes ["count"];

			if (countAttr != null && countAttr.Value.Equals ("0")) {
				switch (triggerDetails) {
				case "SCHEDULE TRIGGER":
					user = new BuildUser ();
					user.Kind = BuildUserKind.ScheduledTrigger;
					break;
						
				case "RETRY BUILD TRIGGER":
					user = new BuildUser ();
					user.Kind = BuildUserKind.RetryTrigger;
					break;
				}
				
				if (user != null) {
					user.Name = triggerDetails;
					user.UserName = triggerDetails;
					user.Builds.Add (build);
				}
			}		
			
		} else {
			var nameAttribute = userNode.Attributes ["name"];
			var userNameAttribute = userNode.Attributes ["username"];
			
			if (nameAttribute == null) {
				nameAttribute = userNameAttribute;
			}
			
			if (nameAttribute != null)
			{
				user = new BuildUser ();
				user.Name = nameAttribute.Value;
				user.UserName = ParseUserName (userNode.Attributes ["username"].Value);
				user.Builds.Add (build);	
			} else {
				SHLog.Debug ("TESTE");
			}
			
		}
		
		return user;
	}
	
	public static bool IsHuman (string userName)
	{
		switch (userName.ToUpperInvariant ()) {
		case "SCHEDULE TRIGGER":
		case "RETRY BUILD TRIGGER":
			return false;
			
		default: 
			return true;
		}
	}
	
	public static BuildUser ParseFromUser (Build build, XmlDocument xmlDoc)
	{
		BuildUser user = null;
		var userNode = xmlDoc.SelectSingleNode ("user");
		
		if (userNode != null) {
			user = new BuildUser ();
			
			user.UserName = ParseUserName(userNode.Attributes ["username"].Value);
			
			// Name.
			var nameAttribute = userNode.Attributes ["name"];
			
			if (nameAttribute == null) {
				user.Name = user.UserName;
			} else {
				user.Name = nameAttribute.Value;
			}
			
			// Email.
			var emailAttribute = userNode.Attributes ["email"];
			
			if (emailAttribute != null) {
				user.Email = emailAttribute.Value;
			}
			
			user.Builds.Add (build);
		}
		
		return user;
	}
	
	/// <summary>
	/// Parses from change.
	/// </summary>
	/// <returns>
	/// The from change.
	/// </returns>
	/// <param name='build'>
	/// Build.
	/// </param>
	/// <param name='xmlDoc'>
	/// Xml document.
	/// </param>
	public static BuildUser ParseFromChange (Build build, XmlDocument xmlDoc)
	{
		BuildUser user = null;
		
		var userNode = xmlDoc.SelectSingleNode ("change/user");
		
		if (userNode != null) {
			
			var username = ParseUserName(userNode.Attributes ["username"].Value);
			
			if (s_buildUsers.ContainsKey (username)) {
				user = s_buildUsers [username];
				
				if (!user.Builds.Contains (build)) {
					user.Builds.Add (build);
				}
				
			} else {
				user = new BuildUser ();
				user.Name = username;
				user.UserName = username;
				user.Builds.Add (build);
			
				s_buildUsers.Add (user.UserName, user);
			}
		}
		
		return user;
	}
	
	public static string ParseUserName (string fullUserNameWithDomain)
	{
		var userName = fullUserNameWithDomain;
		var indexOfSlash = userName.IndexOf ("\\");
		
		if (indexOfSlash > -1) {
			userName = fullUserNameWithDomain.Substring(indexOfSlash + 1);
		}
	
		return userName;
	}
	#endregion
}