using System;
using Buildron.Domain.Builds;
using System.Collections.Generic;

public class EmulatorBuildConfiguration : IBuildConfiguration
{
	public EmulatorBuildConfiguration ()
	{
		Id = "1";
		Name = "Configuration #1";
		Project = new EmulatorBuildProject ();
	}

	#region IBuildConfiguration implementation

	public string Id { get; set; }

	public string Name { get; set; }

	public IBuildProject Project { get; set; }

	public IList<IBuildStep> Steps { get; set; }

	#endregion
}
