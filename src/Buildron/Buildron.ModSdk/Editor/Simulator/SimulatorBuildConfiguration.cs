using System;
using Buildron.Domain.Builds;
using System.Collections.Generic;

public class SimulatorBuildConfiguration : IBuildConfiguration
{
	public SimulatorBuildConfiguration ()
	{
		Id = "1";
		Name = "Configuration #1";
		Project = new SimulatorBuildProject ();
	}

	#region IBuildConfiguration implementation

	public string Id { get; set; }

	public string Name { get; set; }

	public IBuildProject Project { get; set; }

	public IList<IBuildStep> Steps { get; set; }

	#endregion
}
