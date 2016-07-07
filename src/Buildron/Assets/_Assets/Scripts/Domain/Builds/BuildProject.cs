namespace Buildron.Domain.Builds
{
	/// <summary>
	/// Represents a build project.
	/// </summary>
	public class BuildProject : IBuildProject
    {
		#region Properties
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }
		#endregion
	}
}