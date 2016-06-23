#region Usings
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
#endregion

namespace Buildron.Domain
{
	/// <summary>
	/// Represents a build configuration.
	/// </summary>
	[DebuggerDisplay("{Id} - {Name}")]
	public class BuildConfiguration
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.BuildConfiguration"/> class.
		/// </summary>
		public BuildConfiguration ()
		{
			Steps = new List<BuildStep> ();
			Project = new BuildProject();
		}
		#endregion
		
		#region Properties
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the steps.
		/// </summary>
		/// <value>The steps.</value>
		public IList<BuildStep> Steps { get; set; }

		/// <summary>
		/// Gets or sets the project.
		/// </summary>
		/// <value>The project.</value>
		public BuildProject Project { get; set; }
		#endregion
	}
		
}