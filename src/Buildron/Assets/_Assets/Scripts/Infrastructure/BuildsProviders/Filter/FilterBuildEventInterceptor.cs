using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buildron.Domain;

namespace Buildron.Infrastructure.BuildsProvider.Filter
{
    public class FilterBuildEventInterceptor : IBuildEventInterceptor
    {
        public void OnStatusChanged(BuildEvent buildEvent)
        {
            buildEvent.Canceled = !FilterBuildsProvider.FilterBuild(buildEvent.Build);
        }

        public void OnTriggeredByChanged(BuildEvent buildEvent)
        {
            buildEvent.Canceled = !FilterBuildsProvider.FilterBuild(buildEvent.Build);
        }
    }
}
