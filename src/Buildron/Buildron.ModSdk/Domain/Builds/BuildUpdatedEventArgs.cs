namespace Buildron.Domain.Builds
{
	/// <summary>
	/// Build updated event arguments.
	/// </summary>
	public class BuildUpdatedEventArgs : BuildEventArgsBase
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Builds.BuildUpdatedEventArgs"/> class.
		/// </summary>
		/// <param name="build">Build.</param>
		public BuildUpdatedEventArgs (IBuild build) 
			: base (build)
		{
		}
		#endregion
	}
}