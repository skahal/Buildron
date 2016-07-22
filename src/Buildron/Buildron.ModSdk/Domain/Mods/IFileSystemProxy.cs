using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Buildron.Domain.Mods
{
    /// <summary>
    /// Defines an interface to a file system proxy.
    /// </summary>
    public interface IFileSystemProxy
    {
        string[] GetFiles(string path, string searchPattern, bool recursive);

        string[] GetDirectories(string path, string searchPattern, bool recursive);
    }
}
