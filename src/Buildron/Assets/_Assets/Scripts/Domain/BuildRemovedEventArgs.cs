namespace Buildron.Domain
{
    /// <summary>
    /// Arguments for build removed events.
    /// </summary>
    public class BuildRemovedEventArgs : BuildEventArgsBase
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.BuildRemovedEventArgs"/> class.
		/// </summary>
		/// <param name="build">Build.</param>
		public BuildRemovedEventArgs (Build build)
			: base(build)
		{
		}
        #endregion
	}
}