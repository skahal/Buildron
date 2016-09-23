namespace Buildron.Domain.Versions
{
	/// <summary>
	/// Represents a version update info.
	/// </summary>
	public struct VersionUpdateInfo
	{
		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>The description.</value>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the URL.
		/// </summary>
		/// <value>The URL.</value>
		public string Url { get; set; }
	}
}