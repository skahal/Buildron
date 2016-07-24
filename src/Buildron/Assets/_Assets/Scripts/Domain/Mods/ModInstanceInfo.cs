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
			IDataProxy data,
			IBuildGameObjectsProxy buildGameObjects,
			IUserGameObjectsProxy userGameObjects)
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
			BuildGameObjects = buildGameObjects;
			UserGameObjects = userGameObjects;
		}

		public IMod Mod { get; private set; }
		public ModInfo Info { get; private set; }
		public IAssetsProxy Assets { get; private set; }
		public IGameObjectsProxy GameObjects { get; private set; }
		public IUIProxy UI { get; private set; }
		public IFileSystemProxy FileSystem { get; private set; }
		public IGameObjectsPoolProxy GameObjectsPool { get;  private set; }
		public IDataProxy Data { get; private set; }
		public IBuildGameObjectsProxy BuildGameObjects { get; private set; }

		public IUserGameObjectsProxy UserGameObjects { get; private set; }

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

