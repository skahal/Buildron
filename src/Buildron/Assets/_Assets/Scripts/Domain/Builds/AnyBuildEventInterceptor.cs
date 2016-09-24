using System;

namespace Buildron.Domain.Builds
{
    /// <summary>
    /// A build event interceptor to any event.
    /// </summary>
    public class AnyBuildEventInterceptor : IBuildEventInterceptor
    {
        #region Fields
        private Action<BuildEvent> m_action;
        #endregion

        #region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Builds.AnyBuildEventInterceptor"/> class.
		/// </summary>
		/// <param name="action">Action.</param>
        public AnyBuildEventInterceptor(Action<BuildEvent> action)
        {
            m_action = action;
        }
        #endregion

        #region Methods        
        /// <summary>
        /// Called when a build status changed.
        /// </summary>
        /// <param name="buildEvent">The build event.</param>
        public void OnStatusChanged(BuildEvent buildEvent)
        {
            m_action(buildEvent);
        }

        /// <summary>
        /// Called when a build triggered by changed.
        /// </summary>
        /// <param name="buildEvent">The build event.</param>
        public void OnTriggeredByChanged(BuildEvent buildEvent)
        {
            m_action(buildEvent);
        }
        #endregion
    }
}
