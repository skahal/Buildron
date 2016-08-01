using System;
using Buildron.Domain.Mods;

/// <summary>
/// File system proxy extension methods.
/// </summary>
public static class FileSystemProxyExtensions
{
	/// <summary>
	/// Search for files on entire mod exclusive file system.
	/// </summary>
	/// <returns>The files.</returns>
	/// <param name="fs">The file system.</param>
	/// <param name="searchPattern">The search pattern. Example: *.png</param>
	/// <param name="recursive">If the search should be recursive.</param>
    public static string[] SearchFiles(this IFileSystemProxy fs, string searchPattern, bool recursive = true)
    {            
        return fs.GetFiles(String.Empty, searchPattern, recursive);
    }

	/// <summary>
	/// Search for directories on entire mod exclusive file system.
	/// </summary>
	/// <returns>The files.</returns>
	/// <param name="fs">The file system.</param>
	/// <param name="searchPattern">The search pattern. Example: *Test*</param>
	/// <param name="recursive">If the search should be recursive.</param>
	public static string[] SearchDirectories(this IFileSystemProxy fs, string searchPattern, bool recursive = true)
    {
        return fs.GetDirectories (String.Empty, searchPattern, recursive);
    }
}