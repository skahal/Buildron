using System;

namespace Buildron.Domain.Mods
{
	public class ModInstanceInfo
	{
        private readonly IModsProvider m_provider;

		public ModInstanceInfo (
            IMod mod, 
            ModInfo info, 
            IModsProvider provider, 
            IAssetsProxy assets, 
            IGameObjectsProxy gameObjects, 
            IGameObjectsPoolProxy gameObjectsPool,
            IUIProxy ui, 
            IFileSystemProxy fileSystem,
			IDataProxy data)
		{
			Mod = mod;
			Info = info;
            m_provider = provider;
			Assets = assets;
			GameObjects = gameObjects;
            GameObjectsPool = gameObjectsPool;
			UI = ui;
            FileSystem = fileSystem;
			Data = data;
		}

		public IMod Mod { get; private set; }
		public ModInfo Info { get; private set; }
		public IAssetsProxy Assets { get; set; }
		public IGameObjectsProxy GameObjects { get; set; }
		public IUIProxy UI { get; set; }
        public IFileSystemProxy FileSystem { get; set; }
        public IGameObjectsPoolProxy GameObjectsPool { get;  set; }

		public IDataProxy Data { get; set; }

        public void Destroy()
        {
            var modAsDisposable = Mod as IDisposable;

            if (modAsDisposable != null)
            {
                modAsDisposable.Dispose();
            }

            m_provider.DestroyInstance(this);
        }
	}
}

