using System;
using System.Collections.Generic;
using System.Diagnostics;
using Buildron.Domain.Mods;
using Buildron.Domain.Users;

namespace Buildron.Domain.Builds
{
    /// <summary>
    /// Represents a build in continuous integration server.
    /// </summary>
    [DebuggerDisplay("{Configuration.Project.Name} - {Configuration.Name}: {Status}")]
    public sealed class Build : IBuild
    {
        #region Constants        
        /// <summary>
        /// The seconds to lock queue status
        /// </summary>
        private const int SecondsToLockQueueStatus = 20;
        #endregion

        #region Events        
        /// <summary>
        /// Occurs when status has changed.
        /// </summary>
        public event EventHandler<BuildStatusChangedEventArgs> StatusChanged;

        /// <summary>
        /// Occurs when triggered by has changed.
        /// </summary>
        public event EventHandler<BuildTriggeredByChangedEventArgs> TriggeredByChanged;
        #endregion

        #region Fields
        private BuildStatus m_status;
        private IUser m_triggeredBy;
        private static int s_instancesCount;
        #endregion

        #region Constructors
		/// <summary>
		/// Initializes the <see cref="Buildron.Domain.Builds.Build"/> class.
		/// </summary>
        static Build()
        {
            EventInterceptors = new List<IBuildEventInterceptor>();
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Builds.Build"/> class.
		/// </summary>
        public Build()
        {
            Status = BuildStatus.Unknown;
            Configuration = new BuildConfiguration();
            Sequence = ++s_instancesCount;
			Branch = new BuildBranch();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the event interceptors.
        /// </summary>
        public static IList<IBuildEventInterceptor> EventInterceptors { get; private set; }

		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
        public string Id { get; set; }

		/// <summary>
		/// Gets or sets the sequence.
		/// </summary>
		/// <value>The sequence.</value>
        public int Sequence { get; set; }

		/// <summary>
		/// Gets or sets the configuration.
		/// </summary>
		/// <value>The configuration.</value>
        public IBuildConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public BuildStatus Status
        {
            get
            {
                return m_status;
            }

            set
            {
                if (m_status != value)
                {
                    PreviousStatus = m_status;
                    m_status = value;

                    OnStatusChanged(new BuildStatusChangedEventArgs(this, PreviousStatus));
                }
            }
        }

        /// <summary>
        /// Gets the previous status.
        /// </summary>
        public BuildStatus PreviousStatus { get; private set; }

		/// <summary>
		/// Gets or sets the last ran step.
		/// </summary>
		/// <value>The last ran step.</value>
        public IBuildStep LastRanStep { get; set; }

		/// <summary>
		/// Gets or sets the last change description.
		/// </summary>
		/// <value>The last change description.</value>
        public string LastChangeDescription { get; set; }

		/// <summary>
		/// Gets or sets the date.
		/// </summary>
		/// <value>The date.</value>
        public DateTime Date { get; set; }

		/// <summary>
		/// Gets or sets the percentage complete.
		/// </summary>
	    public float PercentageComplete { get; set; }

        /// <summary>
        /// Gets or sets the user that triggered the build.
        /// </summary>
        public IUser TriggeredBy
        {
            get
            {
                return m_triggeredBy;
            }

            set
            {
				if (m_triggeredBy == null || m_triggeredBy.CompareTo(value) != 0)
                {
                    var previousTriggeredBy = m_triggeredBy;
                    m_triggeredBy = value;

                    if (m_triggeredBy != null)
                    {
                        OnTriggeredByChanged(new BuildTriggeredByChangedEventArgs(this, previousTriggeredBy));
                    }
                }
            }
        }

		/// <summary>
		/// Gets the branch.
		/// </summary>
		/// <value>The branch.</value>
		public IBuildBranch Branch { get; private set; }
        #endregion

        #region Methods        
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format("{0} - {1}", Configuration.Project.Name, Configuration.Name);
        }

        /// <summary>
        /// Compares to other build.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public int CompareTo(IBuild other)
        {
            if (other == null)
            {
                return 1;
            }

            var currentText = String.Format("{0} - {1}", Configuration.Project.Name, Configuration.Name);
            var otherText = String.Format("{0} - {1}", other.Configuration.Project.Name, other.Configuration.Name);

            return currentText.CompareTo(otherText);
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>The clone.</returns>
        public object Clone()
        {
            var c = (IBuild)this.MemberwiseClone();

            return c;
        }

        /// <summary>
        /// Raises the <see cref="E:StatusChanged" /> event.
        /// </summary>
        /// <param name="args">The <see cref="BuildStatusChangedEventArgs"/> instance containing the event data.</param>
        private void OnStatusChanged(BuildStatusChangedEventArgs args)
        {
            if (StatusChanged != null)
            {
                var buildEvent = CallInterceptors((i, e) => i.OnStatusChanged(e));

                if (!buildEvent.Canceled)
                {
                    StatusChanged(this, args);
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:TriggeredByChanged" /> event.
        /// </summary>
        /// <param name="args">The <see cref="BuildTriggeredByChangedEventArgs"/> instance containing the event data.</param>
        private void OnTriggeredByChanged(BuildTriggeredByChangedEventArgs args)
        {
            if (TriggeredByChanged != null)
            {
                var buildEvent = CallInterceptors((i, e) => i.OnTriggeredByChanged(e));

                if (!buildEvent.Canceled)
                {
                    TriggeredByChanged(this, args);
                }
            }
        }

        /// <summary>
        /// Calls the interceptors.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns></returns>
        private BuildEvent CallInterceptors(Action<IBuildEventInterceptor, BuildEvent> method)
        {
            var buildEvent = new BuildEvent(this);

            foreach (var interceptor in EventInterceptors)
            {
                method(interceptor, buildEvent);
            }

            return buildEvent;
        }
        #endregion
    }
}