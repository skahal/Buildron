using System;
using Buildron.Domain.Builds;

public class SimulatorBuildStep : IBuildStep
{
	#region IBuildStep implementation

	public string Name { get; set; }

	public BuildStepType StepType { get; set; }

	#endregion
}
