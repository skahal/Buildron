using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buildron.Domain;
using Buildron.Domain.Builds;

namespace Buildron.Infrastructure.BuildsProviders.Filter
{
	/// <summary>
	/// Filter build event interceptor.
	/// </summary>
    public class FilterBuildEventInterceptor : IBuildEventInterceptor
    {
		#region Fields
		private readonly IBuildFilter m_buildFilter;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="Buildron.Infrastructure.BuildsProviders.Filter.FilterBuildEventInterceptor"/> class.
		/// </summary>
		/// <param name="buildFilter">The build filter.</param>
		public FilterBuildEventInterceptor(IBuildFilter buildFilter)
		{
			m_buildFilter = buildFilter;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Called when a build status changed.
		/// </summary>
		/// <param name="buildEvent">The build event.</param>
        public void OnStatusChanged(BuildEvent buildEvent)
        {
			buildEvent.Canceled = !m_buildFilter.Filter(buildEvent.Build);
        }

		/// <summary>
		/// Called when a build triggered by changed.
		/// </summary>
		/// <param name="buildEvent">The build event.</param>
        public void OnTriggeredByChanged(BuildEvent buildEvent)
        {
			buildEvent.Canceled = !m_buildFilter.Filter(buildEvent.Build);
        }
		#endregion
    }
}
