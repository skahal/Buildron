using System.Collections;
using System.Collections.Generic;
using Buildron.Domain;
using Skahal.Logging;
using UnityEngine;
using Skahal.Rendering;
using Skahal.Threading;
using Buildron.Application;
using Zenject;

#region Enums
/// <summary>
/// Camera state.
/// </summary>
public enum CameraState
{
	ShowingBuilds,
	ShowingFocusBuild,
	GoingToHistory,
	GoingToBuilds,
	ShowingHistory,
	ShowingConfigPanel
}
#endregion

/// <summary>
/// Camera controller.
/// </summary>
[AddComponentMenu("Buildron/Camera/CameraController")]
[RequireComponent(typeof(BlurEffect))]
public class CameraController : MonoBehaviour
{
    #region Fields
    [Inject]
    private IBuildService m_buildService;

    private Vector3 m_firstPosition;
	private int m_lastVisiblesCount;
	private Vector3 m_originalPosition;
	private Vector3 m_targetPosition;
	private Transform m_target;
	private BlurEffect m_serverDownBlurEffect;
	private Tonemapping m_serverDownToneMappingEffect;
	private CameraState m_state = CameraState.ShowingConfigPanel;
	private Vector3 m_historyPosition;
	private bool m_autoPosition = true;
    #endregion
	
	#region Properties
	public int MaxBuildsBeforeMove = 16;
	public Vector3 DistanceFromFocusBuild = new Vector3(0, 0, -3);
	public Vector3 DistanceFromOriginalPosition = new Vector3(0, 0, -1);
	public Vector3 DistanceFromHistory = new Vector3 (0, 0, -11);
	public float BuildHeight = 0.5f;
	public float BuildWidth = 2.5f;
	public float DiffYMoveEachVisibleBuild = 0.3f;
	public float DiffZMoveEachVisibleBuild = 0.6f;
	public float AdjustPositionInterval = 1f;
	public float VelocityToShowTop = 0.1f;
	public float VelocityToShowSides = 0.1f;
	public float MinY = 0;

	[Inject]
	public BuildGOService BuildGOService { get; set; }
	#endregion
	
	#region Methods
	private void Awake ()
	{		
		m_originalPosition = transform.position;
		m_firstPosition = m_originalPosition;
		m_targetPosition = transform.position;
		m_historyPosition = m_originalPosition + new Vector3 (0, 30, 25);	
	
		ServerState.Updated += (sender, e) => {
            var cameraPosition = ServerState.Instance.GetCameraPosition();            
            SHLog.Warning("Setting camera position to latest position: {0}", cameraPosition);
            m_autoPosition = false;
			m_originalPosition = cameraPosition;			
		};

		Messenger.Register (gameObject, 
			"OnCIServerReady",
			"OnBuildReachGround",
			"OnBuildHidden",
			"OnBuildVisible", 
			"OnShowHistoryRequested",
			"OnShowBuildsRequested",
			"OnServerDown",
			"OnZoomIn",
			"OnZoomOut",
			"OnGoLeft",
			"OnGoRight",
			"OnGoUp",
			"OnGoDown",
			"OnResetCamera",
			"OnBuildFilterUpdated");
		
		PrepareEffects ();
		StartCoroutine (AdjustCameraPosition ());
	}
	
	private void OnCIServerReady ()
	{
		transform.position = m_originalPosition;
		m_state = CameraState.ShowingBuilds;
	}
	
	private IEnumerator AdjustCameraPosition ()
	{
		while (true) {
			switch (m_state) {
			case CameraState.ShowingBuilds:
				m_targetPosition = CalculatePositionToShowAllBuilds ();
				break;
			
			case CameraState.ShowingFocusBuild:
				m_targetPosition = m_target.position + DistanceFromFocusBuild;
				break;
			
			case CameraState.GoingToHistory:
				m_targetPosition = m_historyPosition;
				break;
			
			case CameraState.GoingToBuilds:
				m_targetPosition = m_originalPosition;
				break;
			}	
			
			if (m_targetPosition.y < MinY) {
				m_targetPosition.y = MinY;	
			}
			
			yield return new WaitForSeconds(AdjustPositionInterval);
		}
	}
	
	private void LateUpdate ()
	{
		transform.position = Vector3.Lerp (transform.position, m_targetPosition, Time.deltaTime);
	}
	
	private Vector3 CalculatePositionToShowAllBuilds ()
	{
		if (m_autoPosition) {
			var currentVisiblesCount = BuildGOService.CountVisibles();
			var diff = m_lastVisiblesCount - currentVisiblesCount;
		
			if (diff > 0) {
				m_originalPosition = m_firstPosition;
			} else {
				var hasNotVisiblesFromTop = false;
				var hasNotVisiblesFromSides = BuildGOService.HasNotVisiblesFromSides ();

				if (!hasNotVisiblesFromSides) {
					hasNotVisiblesFromTop = BuildGOService.HasNotVisiblesFromTop ();
				}
			
				m_originalPosition += new Vector3 (
													0, 
													hasNotVisiblesFromTop ? VelocityToShowTop : 0, 
													hasNotVisiblesFromSides ? -VelocityToShowSides : 0);
			}
		
			m_lastVisiblesCount = currentVisiblesCount;

            SaveServerState();
        }
		
		return m_originalPosition;
	}
	
	private void OnBuildHidden ()
	{
		ChangeByBuilds ();		
	}
	
	private void OnBuildVisible ()
	{
		ChangeByBuilds ();		
	}
	
	private void ChangeByBuilds ()
	{
		if (BuildGOService.CountVisibles() == 1) {
			var visibleOne = BuildGOService.GetVisibles () [0];
			m_target = visibleOne.transform;
			m_state = CameraState.ShowingFocusBuild;
			Messenger.Send ("OnCameraZoomIn");
		} else {
			m_target = null;
			m_state = CameraState.ShowingBuilds;
			Messenger.Send ("OnCameraZoomOut");
		}
	}
	
	private void PrepareEffects ()
	{
		m_serverDownBlurEffect = GetComponent<BlurEffect> ();	
		m_serverDownBlurEffect.enabled = true;
		
		m_serverDownToneMappingEffect = GetComponent<Tonemapping> ();
		m_serverDownToneMappingEffect.enabled = true;

        m_buildService.CIServerStatusChanged += (e, args) => {
			var isDown = args.Server.Status == CIServerStatus.Down;
			m_serverDownBlurEffect.enabled = isDown;
			m_serverDownToneMappingEffect.enabled = isDown;
		};
	}

	private void OnShowHistoryRequested ()
	{
		var histories = BuildsHistoryController.GetAll ();
		
		if (histories.Length > 0) {
			m_state = CameraState.GoingToHistory;
			
			StatusBarController.SetStatusText ("Today's builds history");
			
			SHThread.Start (2f, () => 
			{
				SHThread.Loop (1.5f, 0, histories.Length, (t) => 
				{
					if (ServerState.Instance.IsShowingHistory) {
						m_state = CameraState.ShowingHistory;
						m_targetPosition = histories [histories.Length - 1 - Mathf.FloorToInt (t)].transform.position + DistanceFromHistory; 
						return true;
					}
					
					return false;
				});
			});
		}
	}

	private void OnShowBuildsRequested ()
	{
		m_state = CameraState.GoingToBuilds;
		StatusBarController.ClearStatusText ();
		
		SHThread.Start (3f, () => 
		{
			ChangeByBuilds ();
		});
	}
	
	private void Reposition (Vector3 increment)
	{
		m_originalPosition += increment;        
        m_autoPosition = false;
        SaveServerState();
    }
	
	private void OnZoomIn ()
	{
		Reposition (Vector3.forward);
	}
	
	private void OnZoomOut ()
	{
		Reposition (Vector3.back);
	}
	
	private void OnGoLeft ()
	{
		Reposition (Vector3.left);
	}
	
	private void OnGoRight ()
	{
		Reposition (Vector3.right);
	}
	
	private void OnGoUp ()
	{
		Reposition (Vector3.up);
	}
	
	private void OnGoDown ()
	{
		Reposition (Vector3.down);
	}
	
	private void OnResetCamera ()
	{
		m_originalPosition = m_firstPosition;
		m_autoPosition = true;
        SaveServerState();
    }

    private void SaveServerState()
    {
        ServerState.Instance.SetCameraPosition(m_originalPosition);
        ServerService.SaveState();
    }
	#endregion
}
