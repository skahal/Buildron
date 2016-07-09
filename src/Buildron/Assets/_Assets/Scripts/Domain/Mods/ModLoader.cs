using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skahal.Logging;
using Buildron.Domain.Builds;
using Buildron.Domain.CIServers;
using Buildron.Domain.RemoteControls;
using Buildron.Domain.Users;
using UnityEngine;
using System.Reflection;

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
			m_log = new PrefixedLogStrategy (log, "[MOD-LOADER] ");
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

           // var assetBundle = AssetBundle.LoadFromFile("/Users/giacomelli/Dropbox/Skahal/Apps/Buildron/build/Mods/consolemod");
            //var assetBundle = AssetBundle.LoadFromFile(@"C:\Dropbox\Skahal\Apps\Buildron\build\Mods\consolemod");

           // m_log.Debug("{0} Assets loaded.", assetBundle.GetAllAssetNames().Length);


            // VER ISSO
            // https://docs.unity3d.com/Manual/scriptsinassetbundles.html
            // Fazer CreateModsAssetBundles criar a versão TextAsset .txt de todos os .cs da pasta do mod. 
            // Depois carregar pelo reflection.
//            var assets = assetBundle.LoadAllAssets<TextAsset>();
            Assembly modAssembly = null;


			try 
			{
				var mod = AppDomain.CurrentDomain.CreateInstanceFromAndUnwrap ("/Users/giacomelli/Dropbox/Console/test.dll", "Buildron.Mods.Console.Mod") as IMod;

				var context = new ModContext (mod, m_originalLog, m_buildService, m_ciServerService, m_remoteControlService, m_userService);
				mod.Initialize (context);

				m_log.Debug ("Mod created: {0}", mod.Name);
			}
			catch(Exception ex) {
				m_log.Error (ex.Message);
			}
				
           // http://answers.unity3d.com/questions/259569/how-to-compile-script-to-include-it-to-assetbundle.html
           // https://docs.unity3d.com/Manual/UsingDLL.html

//            foreach (var txt in assets)
//            {
//                m_log.Debug("Loading code from asset {0}...", txt.name);
//
//                try
//                {
//					modAssembly = System.Reflection.Assembly.Load(txt.bytes, );
//                    m_log.Debug("Assembly {0} loaded.", modAssembly.FullName);
//                }
//                catch (Exception ex)
//                {
//                    m_log.Warning("Error loading code from asset: {0}", ex.Message);
//                }
//            }
//
//			m_log.Debug ("Types in bundle: ");
//			Type[] bundleTypes;
//
//			try 
//			{
//			 	bundleTypes = modAssembly.GetTypes ();
//			}
//			catch(ReflectionTypeLoadException ex) {
//				m_log.Warning ("Error loading types: {0}", ex.Message);
//
//				bundleTypes = ex.Types.Where (t => t != null).ToArray ();
//			}
//
//			foreach (var bt in bundleTypes) {
//				m_log.Debug ("\t {0}", bt.Name);
//			}
//
//			//var modTypes = bundleTypes.Where (t => !t.IsAbstract && typeof(IMod).IsAssignableFrom (t)).ToArray ();
//			var modTypes = bundleTypes.Where (t => !t.IsAbstract && t.GetInterface("IMod") != null).ToArray ();
//			m_log.Debug ("Found {0} mods", modTypes.Length);

//			foreach (var modType in modTypes) 
//			{
//				m_log.Debug ("Loading mod from class '{0}'...", modType.FullName);
//				var mod = Activator.CreateInstance (modType) as IMod;
//
//				if (mod == null) {
//					m_log.Warning ("Cannot create mod, there is no default constructor.");
//					continue;
//				}
//
//				m_log.Debug ("Initializing mod {0}...", mod.Name);
//				var context = new ModContext (mod, m_originalLog, m_buildService, m_ciServerService, m_remoteControlService, m_userService);
//				mod.Initialize (context);
//			}
//
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
