using System.Collections.Generic;
using System.Diagnostics;

namespace Buildron.Domain.Builds
{
	/// <summary>
	/// Represents a build configuration.
	/// </summary>
	[DebuggerDisplay("{Id} - {Name}")]
	public class BuildConfiguration : IBuildConfiguration
    {
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Builds.BuildConfiguration"/> class.
		/// </summary>
		public BuildConfiguration ()
		{
			Steps = new List<IBuildStep> ();
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
		public IList<IBuildStep> Steps { get; set; }

		/// <summary>
		/// Gets or sets the project.
		/// </summary>
		/// <value>The project.</value>
		public IBuildProject Project { get; set; }
		#endregion
	}
		
}