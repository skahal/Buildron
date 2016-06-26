using System.Xml;
using Buildron.Domain;
using System.Collections.Generic;
using Buildron.Domain.Builds;

/// <summary>
/// A parser for User .
/// </summary>
public static class UserParser
{
	#region Fields
	public static Dictionary<string, User> s_buildUsers = new Dictionary<string, User>();
	#endregion
	
	#region Methods
	public static void Reset ()
	{
		s_buildUsers.Clear();	
	}
	
	/// <summary>
	/// Parse a User from a XmlDocument.
	/// </summary>
	/// <param name='xmlDoc'>
	/// Xml document.
	/// </param>
	public static User ParseFromTriggered (Build build, XmlDocument xmlDoc)
	{
		User user = null;
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
					user = new User ();
					user.Kind = UserKind.ScheduledTrigger;
					break;
						
				case "RETRY BUILD TRIGGER":
					user = new User ();
					user.Kind = UserKind.RetryTrigger;
					break;
				}
				
				if (user != null) {
					user.Name = triggerDetails;
					user.UserName = triggerDetails;
					user.Builds.Add (build);
				}
			}		
			
		} 
		else {
			var nameAttribute = userNode.Attributes ["name"];
			var userNameAttribute = userNode.Attributes ["username"];
			
			if (nameAttribute == null) {
				nameAttribute = userNameAttribute;
			}
			
			if (nameAttribute != null)
			{
				user = new User ();
				user.Name = nameAttribute.Value;
				user.UserName = ParseUserName (userNode.Attributes ["username"].Value);
				user.Builds.Add (build);	
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
	
	public static User ParseFromUser (Build build, XmlDocument xmlDoc)
	{
		User user = null;
		var userNode = xmlDoc.SelectSingleNode ("user");
		
		if (userNode != null) {
			user = new User ();
			
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