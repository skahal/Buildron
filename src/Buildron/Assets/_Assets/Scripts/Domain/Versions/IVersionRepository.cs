namespace Buildron.Domain.Versions
{
	public interface IVersionRepository
	{
		void Save(Version version);
		Version Find();
	}
}