namespace Buildron.Domain.Mods
{
	/// <summary>
	/// Defines an interface to a file system proxy.
	/// </summary>
	public interface IFileSystemProxy
    {
		/// <summary>
		/// Gets the files on the path using the search pattern.
		/// </summary>
		/// <returns>The files.</returns>
		/// <param name="path">The path of the where should get the files. Use relative values here, because you don't know where is your mods folder.</param>
		/// <param name="searchPattern">The search pattern. Example: *.png</param>
		/// <param name="recursive">If should search in subdirectories recursively.</param>
		string[] GetFiles(string path, string searchPattern, bool recursive);

		/// <summary>
		/// Gets the directories on the paht using the search pattern.
		/// </summary>
		/// <returns>The directories.</returns>
		/// <param name="path">The path of the where should get the directories. Use relative values here, because you don't know where is your mods folder.</param>
		/// <param name="searchPattern">The search pattern. Example: *.png</param>
		/// <param name="recursive">If should search in subdirectories recursively.</param>
		string[] GetDirectories(string path, string searchPattern, bool recursive);
    }
}