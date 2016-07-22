using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buildron.Domain.Mods;

public static class FileSystemExtensions
{
    public static string[] SearchFiles(this IFileSystemProxy fs, string searchPattern, bool recursive = true)
    {            
        return fs.GetFiles(String.Empty, searchPattern, recursive);
    }

    public static string[] SearchDirectories(this IFileSystemProxy fs, string searchPattern, bool recursive = true)
    {
        return fs.GetDirectories (String.Empty, searchPattern, recursive);
    }
}

