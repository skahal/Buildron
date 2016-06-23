namespace Buildron.Domain
{
	/// <summary>
	/// Build found event arguments.
	/// </summary>
	public class BuildFoundEventArgs : BuildEventArgsBase
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.BuildFoundEventArgs"/> class.
		/// </summary>
		/// <param name="build">Build.</param>
		public BuildFoundEventArgs (Build build) 
			: base (build)
		{
		}
		#endregion
	}
}