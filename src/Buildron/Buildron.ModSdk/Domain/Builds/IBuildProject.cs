namespace Buildron.Domain.Builds
{
	/// <summary>
	/// Defines an interface to a build's project.
	/// </summary>
    public interface IBuildProject
    {
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
        string Name { get; set; }
    }
}