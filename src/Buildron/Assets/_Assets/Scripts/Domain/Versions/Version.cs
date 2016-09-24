namespace Buildron.Domain.Versions
{
	/// <summary>
	/// Represents a version.
	/// </summary>
	public class Version
	{
		/// <summary>
		/// Gets or sets the client identifier.
		/// </summary>
		/// <value>The client identifier.</value>
		public string ClientId { get; set; }

		/// <summary>
		/// Gets or sets the client instance.
		/// </summary>
		/// <value>The client instance.</value>
		public int ClientInstance { get; set; }
	}
}