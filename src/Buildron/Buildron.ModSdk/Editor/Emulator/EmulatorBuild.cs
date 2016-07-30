using System;
using Buildron.Domain.Builds;
using Buildron.Domain.Users;
using Skahal.Common;

public class EmulatorBuild : IBuild
{
	#region Fields
	private static int s_buildsCount;
	#endregion

	#region Constructors
	public EmulatorBuild ()
	{
		Id = (++s_buildsCount).ToString ();
		Configuration = new EmulatorBuildConfiguration{
			Name = "Build #{0} ".With(Id)
		};
		Date = DateTime.Now;

		Status = SHRandomHelper.NextEnum<BuildStatus> ();
		LastRanStep = new EmulatorBuildStep {
			StepType = SHRandomHelper.NextEnum<BuildStepType> ()
		};

		if(this.IsRunning()) {
			PercentageComplete = UnityEngine.Random.Range(0f, 1f);
		}

		TriggeredBy = new EmulatorUser ();
		TriggeredBy.Builds.Add (this);
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

	public BuildStatus Status  { get; set; }

	public IUser TriggeredBy  { get; set; }

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
