using System.Collections.Generic;

namespace Buildron.Domain.Builds
{
	/// <summary>
	/// Defines an interface to a build configuration.
	/// </summary>
    public interface IBuildConfiguration
    {
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
        string Id { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
        string Name { get; set; }

		/// <summary>
		/// Gets or sets the project.
		/// </summary>
		/// <value>The project.</value>
        IBuildProject Project { get; set; }

		/// <summary>
		/// Gets or sets the steps.
		/// </summary>
		/// <value>The steps.</value>
        IList<IBuildStep> Steps { get; set; }
    }
}