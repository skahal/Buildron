using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skahal.Logging;
using Buildron.Domain.Builds;
using Buildron.Domain.CIServers;
using Buildron.Domain.RemoteControls;
using Buildron.Domain.Users;

namespace Buildron.Domain.Mods
{
	public class ModLoader : IModLoader
    {
		#region Fields
		private ISHLogStrategy m_originalLog;
		private ISHLogStrategy m_log;
		private readonly IBuildService m_buildService;
		private readonly ICIServerService m_ciServerService;
		private readonly IRemoteControlService m_remoteControlService;
		private readonly IUserService m_userService;
        private List<IMod> m_loadedMods = new List<IMod>();
		#endregion

		#region Constructors
		public ModLoader(ISHLogStrategy log, IBuildService buildService, ICIServerService ciServerService, IRemoteControlService remoteControlService, IUserService userService)
		{
			m_originalLog = log;
			m_log = new PrefixedLogStrategy (log, "ModLoader: ");
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
			var modTypes = typeof(ModLoader).Assembly.GetTypes ().Where (t => !t.IsAbstract && typeof(IMod).IsAssignableFrom (t)).ToArray ();
			m_log.Debug ("Found {0} mods", modTypes.Length);

			foreach (var modType in modTypes) 
			{
				m_log.Debug ("Loading mod from class '{0}'...", modType.FullName);
				var mod = Activator.CreateInstance (modType) as IMod;

				if (mod == null) {
					m_log.Warning ("Cannot create mod, there is no default constructor.");
					continue;
				}

				m_log.Debug ("Initializing mod {0}...", mod.Name);
				var context = new ModContext (mod, m_originalLog, m_buildService, m_ciServerService, m_remoteControlService, m_userService);
				mod.Initialize (context);
			}

			m_log.Debug ("Initialization finished.");
		}

        public void LoadMod(IMod mod)
        {
            ValidateMod(mod);
            m_loadedMods.Add(mod);
        }

        private void ValidateMod(IMod mod)
        {
            if (mod == null)
            {
                throw new ArgumentNullException("mod");
            }

            if (String.IsNullOrEmpty(mod.Name))
            {
                throw new InvalidOperationException("Mod should have a name.");
            }

            if (m_loadedMods.Any(m => m.Name.Equals(mod)))
            {
                throw new InvalidOperationException("There is another mod already loaded with the name '{0}'.".With(mod.Name));
            }
        }

				 
		#endregion
    }
}
