using Buildron.Domain.Sorting;

namespace Buildron.Domain.Servers
{
	/// <summary>
	/// Server service extensions.
	/// </summary>
	public static class ServerServiceExtensions
	{
		/// <summary>
		/// Updates the sorting.
		/// </summary>
		/// <param name="serverService">Server service.</param>
		/// <param name="algorithmType">Algorithm type.</param>
		/// <param name="sortBy">Sort by.</param>
		public static void UpdateSorting(this IServerService serverService, SortingAlgorithmType algorithmType, SortBy sortBy)
		{
			var serverState = serverService.GetState();
			serverState.BuildSortingAlgorithmType = algorithmType;
			serverState.BuildSortBy = sortBy;
			serverService.SaveState(serverState);
		}
	}
}