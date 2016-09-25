using System;
using Buildron.Domain.Builds;
using Buildron.Domain.Users;
using Skahal.Common;

public class SimulatorBuild : IBuild
{
	#region Fields
	private static int s_buildsCount;
	private BuildStatus m_status;
	#endregion

	#region Constructors
	public SimulatorBuild ()
	{
		Id = (++s_buildsCount).ToString ();
		Configuration = new SimulatorBuildConfiguration{
			Name = "Build #{0} ".With(Id)
		};
		Date = DateTime.Now;

		m_status = SHRandomHelper.NextEnum<BuildStatus> ();
		LastRanStep = new SimulatorBuildStep {
			StepType = SHRandomHelper.NextEnum<BuildStepType> ()
		};

		if(this.IsRunning()) {
			PercentageComplete = UnityEngine.Random.Range(0f, 1f);
		}

		TriggeredBy = new SimulatorUser ();
		TriggeredBy.Builds.Add (this);

		Branch = new SimulatorBuildBranch();
	}
	#endregion

	#region IBuild implementation
	public event EventHandler<BuildStatusChangedEventArgs> StatusChanged;

	public event EventHandler<BuildTriggeredByChangedEventArgs> TriggeredByChanged;

	public IBuildConfiguration Configuration { get; set; }

	public DateTime Date  { get; set; }

	public string Id { get; set; }

	public string LastChangeDescription  { get; set; }

	public IBuildStep LastRanStep  { get; set; }

	public float PercentageComplete  { get; set; }

	public BuildStatus PreviousStatus  { get; set; }

	public int Sequence  { get; set; }

	public BuildStatus Status  
	{
		get {
			return m_status;
		}

		set {
			PreviousStatus = m_status;

			m_status = value;
			StatusChanged.Raise(this, new BuildStatusChangedEventArgs(this, PreviousStatus));
		}
	}

	public IUser TriggeredBy  { get; set; }

	public IBuildBranch Branch { get; private set; }
	#endregion

	#region ICloneable implementation

	public object Clone ()
	{
        return MemberwiseClone();
	}

	#endregion

	#region IComparable implementation

	public int CompareTo (IBuild other)
	{
		throw new NotImplementedException ();
	}

	#endregion
}
