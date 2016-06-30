using Buildron.Application;
using UnityEngine;
using Zenject;

/// <summary>
/// Builds controller.
/// </summary>
public class BuildsController : MonoBehaviour
{
	#region Fields
    [Inject]
	private BuildGOService m_buildGOService;
	#endregion

	#region Methods
    void Start()
    {
        Messenger.Register(gameObject,
            "OnBuildHidden");
    }

    void OnBuildHidden()
    {
        m_buildGOService.WakeUpSleepingBuilds();
    }
	#endregion
}
