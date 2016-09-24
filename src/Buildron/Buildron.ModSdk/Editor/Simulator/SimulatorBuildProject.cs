using System;
using Buildron.Domain.Builds;

public class SimulatorBuildProject : IBuildProject
{
	public SimulatorBuildProject ()
	{
		Name = "Project #1";
	}

	#region IBuildProject implementation

	public string Name { get; set; }

	#endregion
}
