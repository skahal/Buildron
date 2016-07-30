using System;
using Buildron.Domain.Builds;

public class EmulatorBuildStep : IBuildStep
{
	#region IBuildStep implementation

	public string Name { get; set; }

	public BuildStepType StepType { get; set; }

	#endregion
}
