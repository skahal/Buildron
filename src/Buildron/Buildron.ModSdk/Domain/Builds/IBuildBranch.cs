namespace Buildron.Domain.Builds
{
	/// <summary>
	/// Defines an interface to a build's branch.
	/// </summary>
	public interface IBuildBranch
	{
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		string Name { get; set; }
	}
}