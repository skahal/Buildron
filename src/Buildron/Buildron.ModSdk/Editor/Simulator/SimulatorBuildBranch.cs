using System;
using Buildron.Domain.Builds;

public class SimulatorBuildBranch : IBuildBranch
{
	public SimulatorBuildBranch()
	{
		Name = "default";
	}

	public string Name { get; set; }
}