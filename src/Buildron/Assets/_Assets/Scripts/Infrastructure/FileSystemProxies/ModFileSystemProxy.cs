using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Buildron.Domain.Mods;

namespace Buildron.Infrastructure.FileSystemProxies
{
    public class ModFileSystemProxy : IFileSystemProxy
    {
        #region Fields
        private string m_rootPath;
        #endregion

        #region Constructors
        public ModFileSystemProxy(string rootPath)
        {
            m_rootPath = rootPath;
        }
        #endregion

        #region Methods
        public string[] GetDirectories(string path, string searchPattern, bool recursive)
        {           
            return Directory.GetDirectories(GetAbsolutePath(path), searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        public string[] GetFiles(string path, string searchPattern, bool recursive)
        {
            return Directory.GetFiles(GetAbsolutePath(path), searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        private string GetAbsolutePath(string path)
        {
            // TODO: maybe should validate if mod is trying to access a folder outside m_rootPath.
            return Path.Combine(m_rootPath, FixPath(path));
        }

        private string FixPath(string path)
        {
            return path.Replace(@"\", "/");
        }
        #endregion
    }
}
