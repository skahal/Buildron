using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buildron.Domain;

namespace Buildron.Infrastructure.BuildsProvider.Filter
{
	/// <summary>
	/// Filter build event interceptor.
	/// </summary>
    public class FilterBuildEventInterceptor : IBuildEventInterceptor
    {
		#region Fields
		private FilterBuildsProvider m_filterBuildsProvider;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="Buildron.Infrastructure.BuildsProvider.Filter.FilterBuildEventInterceptor"/> class.
		/// </summary>
		/// <param name="filterBuildsProvider">Filter builds provider.</param>
		public FilterBuildEventInterceptor(FilterBuildsProvider filterBuildsProvider)
		{
			m_filterBuildsProvider = filterBuildsProvider;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Called when a build status changed.
		/// </summary>
		/// <param name="buildEvent">The build event.</param>
        public void OnStatusChanged(BuildEvent buildEvent)
        {
			buildEvent.Canceled = !m_filterBuildsProvider.FilterBuild(buildEvent.Build);
        }

		/// <summary>
		/// Called when a build triggered by changed.
		/// </summary>
		/// <param name="buildEvent">The build event.</param>
        public void OnTriggeredByChanged(BuildEvent buildEvent)
        {
			buildEvent.Canceled = !m_filterBuildsProvider.FilterBuild(buildEvent.Build);
        }
		#endregion
    }
}
