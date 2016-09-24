namespace Buildron.Domain.Builds
{
    /// <summary>
    /// Represents an build event used by IBuildInterceptors.
    /// </summary>
    public sealed class BuildEvent
    {
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Builds.BuildEvent"/> class.
		/// </summary>
		/// <param name="build">Build.</param>
        public BuildEvent(IBuild build)
        {
            Build = build;
        }
		#endregion		

		#region Properties
		/// <summary>
		/// Gets the build.
		/// </summary>
		/// <value>The build.</value>
        public IBuild Build { get; private set; }

		/// <summary>
		/// Gets or sets a value indicating whether the event was canceled.
		/// </summary>
		/// <value><c>true</c> if this instance canceled; otherwise, <c>false</c>.</value>
        public bool Canceled { get; set; }
		#endregion
    }
}
