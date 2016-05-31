#region Usings
using Buildron.Domain.Versions;
using UnityEngine;
#endregion

namespace Buildron.Infrastructure.Repositories
{
	public class VersionRepository : IVersionRepository
	{
		#region IVersionRepository implementation
		public void Save (Version version)
		{
			PlayerPrefs.SetString ("Version.ClientId", version.ClientId);
			PlayerPrefs.SetInt ("Version.ClientInstance", version.ClientInstance);
		}

		public Version Find ()
		{
			Version version = null;
			
			if (PlayerPrefs.HasKey ("Version.ClientId")) {
				version = new Version ();
				version.ClientId = PlayerPrefs.GetString ("Version.ClientId");
				version.ClientInstance = PlayerPrefs.GetInt ("Version.ClientInstance");
			}
			
			return version;
		}
		#endregion
	}
}