using Buildron.Domain.Builds;

namespace Buildron.Domain.Builds
{
    /// <summary>
    /// Define a interface for a build event intercpetor.
    /// </summary>
    public interface IBuildEventInterceptor
    {
        /// <summary>
        /// Called when a build status changed.
        /// </summary>
        /// <param name="buildEvent">The build event.</param>
        void OnStatusChanged(BuildEvent buildEvent);

        /// <summary>
        /// Called when a build triggered by changed.
        /// </summary>
        /// <param name="buildEvent">The build event.</param>
        void OnTriggeredByChanged(BuildEvent buildEvent);
    }
}
