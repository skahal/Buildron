#region Usings
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
#endregion

namespace Buildron.Domain
{
	[DebuggerDisplay("{Id} - {Name}")]
	public class BuildConfiguration
	{
		#region Constructors
		public BuildConfiguration ()
		{
			Steps = new List<BuildStep> ();
			Project = new BuildProject();
		}
		#endregion
		
		#region Properties
		public string Id { get; set; }
		public string Name { get; set; }
		public IList<BuildStep> Steps { get; set; }
		public BuildProject Project { get; set; }
		#endregion
	}
		
}