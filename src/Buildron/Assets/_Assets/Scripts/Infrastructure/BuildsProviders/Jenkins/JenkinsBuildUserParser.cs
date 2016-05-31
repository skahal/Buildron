#region Usings
using Buildron.Domain;
using System.Xml;
using System.Text.RegularExpressions;
using System;
using System.Globalization;
#endregion

namespace Buildron.Infrastructure.BuildsProvider.Jenkins
{	
	/// <summary>
	/// A parser for BuildUser.
	/// </summary>
	public static class JenkinsBuildUserParser
	{
		#region Fields
		private static Regex s_findTimerRegex = new Regex ("(Started by timer|Iniciado pelo temporizador)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		#endregion

		
		public static BuildUser ParseUserFromBuildResponse (XmlDocument xmlDoc)
		{
			BuildUser user = null;
			var userNode = xmlDoc.SelectSingleNode ("//action/cause/userName");
			
			if (userNode == null) {
				userNode = xmlDoc.SelectSingleNode ("//changeSet/item/author/fullName");
			}
			
			if (userNode != null) {
				user = new BuildUser ();
				user.UserName = userNode.InnerText;
			}
			
			if (userNode == null) {
				var shortDescription = xmlDoc.SelectSingleNode ("//action/cause/shortDescription");
				
				if (shortDescription != null && s_findTimerRegex.IsMatch(shortDescription.InnerText)) {
					user = new BuildUser ();
					user.UserName = "timer";
					user.Kind = BuildUserKind.ScheduledTrigger;
				}
			}
			

			return user;
		}
		
		public static BuildUser ParseUserFromUserResponse (XmlDocument xmlDoc)
		{
			var user = new BuildUser ();
			user.UserName = xmlDoc.SelectSingleNode ("//user/id").InnerText;
			user.Name = xmlDoc.SelectSingleNode ("//user/fullName").InnerText;
			
			var emailNode = xmlDoc.SelectSingleNode ("//user/property/address");
			
			if (emailNode != null) {
				user.Email = emailNode.InnerText;
			}
			
			return user;
		}
	}
}