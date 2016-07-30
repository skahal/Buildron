using System;
using Buildron.Domain.Builds;

public class EmulatorBuildProject : IBuildProject
{
	public EmulatorBuildProject ()
	{
		Name = "Project #1";
	}

	#region IBuildProject implementation

	public string Name { get; set; }

	#endregion
}
