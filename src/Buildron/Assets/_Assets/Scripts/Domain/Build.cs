#region Usings
using System;
using System.Diagnostics;
#endregion

namespace Buildron.Domain
{
	#region Enums
	public enum BuildStatus
	{
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
	
	public class Build : IComparable<Build>, ICloneable
	{
		#region Constants
		private const int SecondsToLockQueueStatus = 20;
		#endregion
		
		#region Events
		public event EventHandler StatusChanged;
		public event EventHandler TriggeredByChanged;
		#endregion
	
		#region Fields
		private BuildStatus m_status;
		private BuildUser m_triggeredBy;
		private static int s_instancesCount;
		private DateTime m_lockCurrentStatusUntil = DateTime.Now;
		#endregion
		
		#region Constructors
		public Build ()
		{
			Configuration = new BuildConfiguration ();
			Sequence = ++s_instancesCount;
		}
		#endregion
	
		#region Properties
		public string Id { get; set; }
		
		public int Sequence { get; set; }

		public BuildConfiguration Configuration { get; set; }

		public BuildStatus Status {
			get {
				return m_status;
			}
		
			set {
				if (m_status != value && DateTime.Now >= m_lockCurrentStatusUntil) {
					
					if (value == BuildStatus.Running && m_status > BuildStatus.Running) {
						return;
					} 
					
					if (value == BuildStatus.Queued) {
						m_lockCurrentStatusUntil = DateTime.Now.AddSeconds(SecondsToLockQueueStatus);
					}
					
					m_status = value;
			
					if (StatusChanged != null) {
						StatusChanged (this, EventArgs.Empty);
					}
				}
			}
		}
		
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
			
					if (m_triggeredBy != null && TriggeredByChanged != null) {
						TriggeredByChanged (this, EventArgs.Empty);
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
		#endregion

		#region IComparable[Build] implementation
		public int CompareTo (Build other)
		{
			var currentText = String.Format ("{0} - {1}", Configuration.Project.Name, Configuration.Name);
			var otherText = String.Format ("{0} - {1}", other.Configuration.Project.Name, other.Configuration.Name);
			return currentText.CompareTo (otherText);
		}
		#endregion

		#region ICloneable implementation
		public object Clone ()
		{
			var c = (Build) this.MemberwiseClone ();
			return c;
		}
		#endregion
	}
}

