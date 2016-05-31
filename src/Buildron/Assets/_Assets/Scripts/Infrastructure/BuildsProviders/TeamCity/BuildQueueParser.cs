#region Usings
using System.Collections.Generic;
using System.Text.RegularExpressions;
#endregion

namespace Buildron.Infrastructure.BuildsProvider.TeamCity
{
	/// <summary>
	/// Build queue parser.
	/// </summary>
	public static class BuildQueueParser
	{
		#region Fields
		private static Regex s_getBuildConfigurationIdsRegex = new Regex("name=\"ref(bt\\d+)\"", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		#endregion
		
		#region Methods
		/// <summary>
		/// Parses the build configurations identifiers from queue html (http://TeamCityServer/queue.html).
		/// </summary>
		/// <returns>
		/// The build configurations identifiers from queue html.
		/// </returns>
		/// <param name='html'>
		/// Html.
		/// </param>
		public static IList<string> ParseBuildConfigurationsIdsFromQueueHtml (string html)
		{
			var ids = new List<string> ();
			var matches = s_getBuildConfigurationIdsRegex.Matches (html);
			
			foreach (Match m in matches) {
				ids.Add(m.Groups[1].Value);
			}
				
			return ids;
		}
		#endregion
	}
}