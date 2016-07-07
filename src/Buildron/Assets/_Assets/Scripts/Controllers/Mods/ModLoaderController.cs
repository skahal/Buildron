using UnityEngine;
using System.Collections;
using Zenject;
using Buildron.Domain.Mods;

public class ModLoaderController : MonoBehaviour, IInitializable 
{
	#region Fields
	[Inject]
	private IModLoader m_modLoader;
	#endregion

	#region Methods
	public void Initialize()
	{
		m_modLoader.Initialize();
	}
	#endregion
}
