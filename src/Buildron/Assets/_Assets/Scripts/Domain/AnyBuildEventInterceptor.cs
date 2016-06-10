using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Buildron.Domain
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
        /// <exception cref="NotImplementedException"></exception>
        public void OnStatusChanged(BuildEvent buildEvent)
        {
            m_action(buildEvent);
        }

        /// <summary>
        /// Called when a build triggered by changed.
        /// </summary>
        /// <param name="buildEvent">The build event.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnTriggeredByChanged(BuildEvent buildEvent)
        {
            m_action(buildEvent);
        }
        #endregion
    }
}
