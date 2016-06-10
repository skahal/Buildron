#region Usings
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
#endregion

namespace Buildron.Domain
{
	#region Enums
	public enum BuildStatus
	{
        Unknown,
		Error,
		Failed,
		Canceled,
		Success,
		Queued,
		Running,
		RunningUnitTests,
		RunningFunctionalTests,
		RunningDuplicatesFinder,
		RunningCodeAnalysis,
		RunningDeploy
	}
	#endregion
	
	public sealed class Build : IComparable<Build>, ICloneable
	{
		#region Constants
		private const int SecondsToLockQueueStatus = 20;
		#endregion
		
		#region Events
		public event EventHandler<BuildStatusChangedEventArgs> StatusChanged;
		public event EventHandler TriggeredByChanged;
		#endregion
	
		#region Fields
		private BuildStatus m_status;
		private BuildUser m_triggeredBy;
		private static int s_instancesCount;
		private DateTime m_lockCurrentStatusUntil = DateTime.Now;        
		#endregion
		
		#region Constructors
        static Build()
        {
            EventInterceptors = new List<IBuildEventInterceptor>();
        }

		public Build ()
		{
            Status = BuildStatus.Unknown;
			Configuration = new BuildConfiguration ();
			Sequence = ++s_instancesCount;
		}
        #endregion

        #region Properties
        /// <summary>
        /// Gets the event interceptors.
        /// </summary>
        public static IList<IBuildEventInterceptor> EventInterceptors { get; private set; }

        public string Id { get; set; }
		
		public int Sequence { get; set; }

		public BuildConfiguration Configuration { get; set; }

		public BuildStatus Status {
			get {
				return m_status;
			}
		
			set {
				if (m_status != value && DateTime.Now >= m_lockCurrentStatusUntil) {
					
					//if (value == BuildStatus.Running && m_status > BuildStatus.Running) {
					//	return;
					//} 
					
					if (value == BuildStatus.Queued) {
						m_lockCurrentStatusUntil = DateTime.Now.AddSeconds(SecondsToLockQueueStatus);
					}

                    PreviousStatus = m_status;
					m_status = value;
			
					OnStatusChanged (new BuildStatusChangedEventArgs(this, PreviousStatus));
				}
			}
		}

        /// <summary>
        /// Gets the previous status.
        /// </summary>
        public BuildStatus PreviousStatus { get; private set; }
		
		public BuildStep LastRanStep { get; set; }
		
		public string LastChangeDescription { get; set; }
		
		public DateTime Date { get; set; }
				
		public bool IsSuccess {
			get {
				return m_status == BuildStatus.Success;
			}
		}
		
		public bool IsRunning {
			get {
				return m_status >= BuildStatus.Running;
			}
		}
		
		public bool IsQueued {
			get {
				return m_status == BuildStatus.Queued;
			}
		}
		
		public bool IsFailed
		{
			get {
				return m_status <= BuildStatus.Canceled;
			}
		}

		public float PercentageComplete { get; set; }
		
		/// <summary>
		/// Gets or sets the user that triggered the build.
		/// </summary>
		public BuildUser TriggeredBy {
			get {
				return m_triggeredBy;
			}
		
			set {
				if (m_triggeredBy != value) {
					
					m_triggeredBy = value;
			
					if (m_triggeredBy != null) {
						OnTriggeredByChanged (EventArgs.Empty);
					}
				}
			}
		}
		#endregion
		
		#region Methods
		public override string ToString()
		{
			return String.Format ("{0} - {1}", Configuration.Project.Name, Configuration.Name);
		}
	
		public int CompareTo (Build other)
		{
			var currentText = String.Format ("{0} - {1}", Configuration.Project.Name, Configuration.Name);
			var otherText = String.Format ("{0} - {1}", other.Configuration.Project.Name, other.Configuration.Name);
			return currentText.CompareTo (otherText);
		}
	
		public object Clone ()
		{
			var c = (Build) this.MemberwiseClone ();
			return c;
		}

		private void OnStatusChanged(BuildStatusChangedEventArgs args)
		{
			if (StatusChanged != null) {
                var buildEvent = CallInterceptors((i, e) => i.OnStatusChanged(e));

                if (!buildEvent.Canceled)
                {
                    StatusChanged(this, args);
                }
			}
		}

		private void OnTriggeredByChanged(EventArgs args)
		{
			if (TriggeredByChanged != null) {
                var buildEvent = CallInterceptors((i, e) => i.OnTriggeredByChanged(e));

                if (!buildEvent.Canceled)
                {
                    TriggeredByChanged(this, EventArgs.Empty);
                }
			}
		}

        private BuildEvent CallInterceptors(Action<IBuildEventInterceptor, BuildEvent> method)
        {
            var buildEvent = new BuildEvent(this);

            foreach(var interceptor in EventInterceptors)
            {
                method(interceptor, buildEvent);
            }

            return buildEvent;
        }
		#endregion
	}
}

