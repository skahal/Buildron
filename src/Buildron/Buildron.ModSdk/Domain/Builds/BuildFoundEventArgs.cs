namespace Buildron.Domain.Builds
{
	/// <summary>
	/// Build found event arguments.
	/// </summary>
	public class BuildFoundEventArgs : BuildEventArgsBase
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Builds.BuildFoundEventArgs"/> class.
		/// </summary>
		/// <param name="build">Build.</param>
		public BuildFoundEventArgs (IBuild build) 
			: base (build)
		{
		}
		#endregion
	}
}