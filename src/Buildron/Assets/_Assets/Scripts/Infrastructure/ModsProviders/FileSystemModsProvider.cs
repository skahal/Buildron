using System;
using Buildron.Domain.Mods;
using System.Collections.Generic;
using Skahal.Logging;
using System.IO;
using System.Reflection;
using UnityEngine;
using System.Linq;
using Buildron.Infrastructure.AssetsProxies;
using Skahal.Common;

namespace Buildron.Infrastructure.ModsProvider
{
	public class FileSystemModsProvider : IModsProvider
	{
		#region Fields
		private readonly string m_rootFolder;
		private readonly ISHLogStrategy m_log;
		#endregion

		public FileSystemModsProvider (string rootFolder, ISHLogStrategy log)
		{
			Throw.AnyNull (new { rootFolder, log });

			m_rootFolder = rootFolder;
			m_log = log;
		}

		#region Methods
		public IEnumerable<ModInfo> GetModInfos()
		{
			m_log.Debug ("Getting mods informations from folder {0}...", m_rootFolder);
			var modInfos = new List<ModInfo>();
				
			var modsFolders = Directory.GetDirectories (m_rootFolder);
			m_log.Debug ("{0} mods folders", modsFolders.Length);

			foreach (var modFolder in modsFolders) {
				var modFolderName = Path.GetFileName (modFolder);

				if (modFolderName.EndsWith (".disabled", StringComparison.OrdinalIgnoreCase)) {
					m_log.Debug (modFolderName);
					continue;
				}

				modInfos.Add (new ModInfo(modFolderName));
			}

			return modInfos;
		}

		public ModInstanceInfo CreateInstance(ModInfo modInfo)
		{
			var modFolderName = modInfo.Name;
			var modFolder = Path.Combine(m_rootFolder, modFolderName);
			var modAssemblyPath = Path.Combine(modFolder, "{0}.dll".With(modFolderName));
			var modTypeFullName = "{0}.Mod".With(modFolderName);
			var modAssetBundlePath = Path.Combine(modFolder, modFolderName.ToLowerInvariant());

			m_log.Debug("Loading assembly '{0}'...", modAssemblyPath);
			var modAssembly = Assembly.LoadFile(modAssemblyPath);
			var mod = Activator.CreateInstance(modAssembly.GetType(modTypeFullName)) as IMod;

			if (mod == null)
			{
				throw new InvalidOperationException("{0} does not implement IMod interface. Will not be loaded.".With(modTypeFullName));
			}
			else 
			{
				AssetBundle assetBundle = null;

				if (File.Exists(modAssetBundlePath)) {
					m_log.Debug("Loading mod asset bundle from {0}...", modAssetBundlePath);
					assetBundle = AssetBundle.LoadFromFile(modAssetBundlePath);

					m_log.Debug("{0} Assets loaded.", assetBundle.GetAllAssetNames().Length);
				}

				return new ModInstanceInfo(mod, modInfo, new AssetBundleAssetsProxy(assetBundle));
			}
		}
		#endregion
	}
}

