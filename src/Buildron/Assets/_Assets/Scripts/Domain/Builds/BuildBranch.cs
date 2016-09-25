using System;

namespace Buildron.Domain.Builds
{
	public class BuildBranch : IBuildBranch
	{
		public BuildBranch()
		{
			Name = "default";
		}

		public string Name { get; set; }
	}
}
