using Buildron.Domain.Builds;

namespace Buildron.Infrastructure.BuildsProviders.Filter
{
	/// <summary>
	/// Defines an interface to a build filter.
	/// </summary>
	public interface IBuildFilter
	{
		/// <summary>
		/// Filters the build.
		/// </summary>
		/// <returns><c>true</c>, if build was filtered, <c>false</c> otherwise.</returns>
		/// <param name="build">Build.</param>
		bool Filter(Build build);
	}
}
