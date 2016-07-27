using System;
using System.Collections.Generic;
using System.Linq;
using Buildron.Domain.Builds;
using Buildron.Domain.CIServers;
using Buildron.Domain.RemoteControls;
using Buildron.Domain.Users;
using Skahal.Common;
using Skahal.Logging;
using UnityEngine;

namespace Buildron.Domain.Mods
{
    public class ModLoader : IModLoader
    {
		#region Fields
		private ISHLogStrategy m_originalLog;
		private ISHLogStrategy m_log;
		private IModsProvider[] m_modsProviders;
		private readonly IBuildService m_buildService;
		private readonly ICIServerService m_ciServerService;
		private readonly IRemoteControlService m_remoteControlService;
		private readonly IUserService m_userService;
		private List<ModInstanceInfo> m_loadedMods = new List<ModInstanceInfo>();
		#endregion

		#region Constructors
		public ModLoader(
			ISHLogStrategy log, 
			IModsProvider[] modsProviders, 
			IBuildService buildService, 
			ICIServerService ciServerService, 
			IRemoteControlService remoteControlService, 
			IUserService userService)
		{
			Throw.AnyNull (new { log, modsProviders, buildService, ciServerService, remoteControlService, userService });

			m_originalLog = log;
			m_log = new PrefixedLogStrategy (log, "[MOD-LOADER] ");
			m_modsProviders = modsProviders;
			m_buildService = buildService;
			m_ciServerService = ciServerService;
			m_remoteControlService = remoteControlService;
			m_userService = userService;
		}
		#endregion

		#region Methods
		public void Initialize()
		{           
			m_log.Debug ("Initialization started...");
			m_log.Debug ("Mods providers available: {0}", String.Join(", ", m_modsProviders.Select(m => m.GetType().Name).ToArray()));

			foreach (var modProvider in m_modsProviders) {
                try
                {
                    m_log.Debug("Using mod provider {0}", modProvider.GetType().Name);
                    var modInfos = modProvider.GetModInfos().ToArray();
                    m_log.Debug("{0} mods found by {1}: {2}", modInfos.Length, modProvider.GetType().Name, String.Join(", ", modInfos.Select(m => m.Name).ToArray()));

                    foreach (var modInfo in modInfos)
                    {
                        try
                        {
                            if (ValidateMod(modInfo))
                            {
                                m_log.Debug("Creating instance of {0}...", modInfo.Name);

                                var instanceInfo = modProvider.CreateInstance(modInfo);
                                InitializeMod(instanceInfo);
                            }
                        }
                        catch (Exception ex)
                        {
                            m_log.Error("{0}: {1}\n{2}", ex.GetType().Name, ex.Message, ex.StackTrace);
                        }
                    }
                }
                catch (Exception ex)
                {
                    m_log.Error("{0}: {1}\n{2}", ex.GetType().Name, ex.Message, ex.StackTrace);
                }
			}
		
			m_log.Debug ("Initialization finished.");
		}

        public void UnloadMods()
        {
            foreach (var mod in m_loadedMods)
            {
                m_log.Debug("Destroying mod '{0}'...", mod.Info.Name);
                mod.Destroy();   
            }

            m_loadedMods.Clear();
            m_log.Debug("All mods unloaded.");
        }

        private void InitializeMod(ModInstanceInfo instance)
        {
			m_log.Debug ("Creating mod context...");
            m_loadedMods.Add(instance);

            var context = new ModContext (instance, m_originalLog, m_buildService, m_ciServerService, m_remoteControlService, m_userService);

			m_log.Debug ("Initializing mod {0}...", instance.Info.Name);
			instance.Mod.Initialize (context);			

			m_log.Debug ("Mod successful initialized: {0}", instance.Info.Name);
	    }

		private bool ValidateMod(ModInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException("mod");
            }

            if (String.IsNullOrEmpty(info.Name))
            {
                throw new InvalidOperationException("Mod should have a name.");
            }

			if (m_loadedMods.Any(m => m.Info.Name.Equals(info.Name, StringComparison.OrdinalIgnoreCase)))
            {
				m_log.Warning("There is another mod already loaded with the name '{0}'.", info.Name);
				return false;
            }

			return true;
        }	 
		#endregion
    }
}
