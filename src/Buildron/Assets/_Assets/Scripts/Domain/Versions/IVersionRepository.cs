namespace Buildron.Domain.Versions
{
	/// <summary>
	/// Defines an interface for version repository.
	/// </summary>
	public interface IVersionRepository
	{
		/// <summary>
		/// Save the specified version.
		/// </summary>
		/// <param name="version">Version.</param>
		void Save(Version version);

		/// <summary>
		/// Find the version.
		/// </summary>
		Version Find();
	}
}