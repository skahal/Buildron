using Buildron.Domain;
using Buildron.Domain.Builds;
using Buildron.Infrastructure.BuildsProvider.Jenkins;
using Buildron.Domain.CIServers;

namespace Buildron.Infrastructure.BuildsProvider.Hudson
{
	/// <summary>
	/// Hudson builds provider.
	/// </summary>
	public class HudsonBuildsProvider : JenkinsBuildsProvider {
		
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="Buildron.Infrastructure.BuildsProvider.Hudson.HudsonBuildsProvider"/> class.
		/// </summary>
		/// <param name='server'>
		/// Server.
		/// </param>
		public HudsonBuildsProvider (CIServer server) : base(server)
		{
			Name = "Hudson";
			AuthenticationRequirement = AuthenticationRequirement.Optional;
			AuthenticationTip = "If your Hudson server does not require authentication, leave username and password empty, otherwise, type a Hudson's username and password.\n*BASIC HTTP authentication will be used.";
		}
		#endregion
	}
}