#region Usings
using UnityEngine;
using System.Collections;
using Buildron.Domain;
#endregion

/// <summary>
/// Manages the BuildUserController creations.
/// </summary>
[AddComponentMenu("Buildron/Controllers/BuildUsersManager")]
public class BuildUsersManager : MonoBehaviour
{
	#region Fields
	private Vector3 m_currentSpawnPosition;
	private int m_currentRowBuildUserCount;
	private int m_rowsCount = 1;
	#endregion
	
	#region Editor Properties
	public Vector3 FirstSpawnPosition = new Vector3(-10, -3, -10);
	public Vector3 DistanceBetweenBuildUsers = new Vector3(2, 0, 0);
	public int NumberBuildUserPerRows = 5;
	public Vector3 DistanceBetweenBuildUsersRows = new Vector3(0, 0, 2);
	#endregion
	
	#region Methods
	private void Awake ()
	{
		Messenger.Register (gameObject, 
			"OnBuildFailed",
			"OnBuildSuccess",
			"OnBuildRunning",
			"OnBuildQueued");
		
		m_currentSpawnPosition = FirstSpawnPosition;
	}

	private void OnBuildFailed (GameObject buildGO)
	{
		CreateBuildUserGameObject (buildGO);
	}
	
	private void OnBuildSuccess (GameObject buildGO)
	{
		CreateBuildUserGameObject (buildGO);
	}
	
	private void OnBuildRunning (GameObject buildGO)
	{
		CreateBuildUserGameObject (buildGO);
	}
	
	private void OnBuildQueued (GameObject buildGO)
	{
		CreateBuildUserGameObject (buildGO);
	}
	
	private void CreateBuildUserGameObject (GameObject buildGO)
	{
		var build = buildGO.GetComponent<BuildController> ().Data;
	
		if (build.TriggeredBy == null) {
			build.TriggeredByChanged += delegate {
				CreateBuildUserGameObject (build);
			};
			
		} else {
			CreateBuildUserGameObject (build);
		}
	}

	void CreateBuildUserGameObject (Build build)
	{
		var go = BuildUserController.GetGameObject (build.TriggeredBy);
		
		if (go != null) {
			go.GetComponent<BuildUserController> ().Data = build.TriggeredBy;
		} else {
			go = BuildUserController.CreateGameObject (build.TriggeredBy);
			go.transform.position = m_currentSpawnPosition;
			go.transform.parent = transform;			
			
			m_currentSpawnPosition += DistanceBetweenBuildUsers;
			m_currentRowBuildUserCount++;
			
			if (m_currentRowBuildUserCount >= NumberBuildUserPerRows) {
				m_currentRowBuildUserCount = 0;
				m_currentSpawnPosition = FirstSpawnPosition;
				m_currentSpawnPosition += DistanceBetweenBuildUsersRows * m_rowsCount;
				m_rowsCount++;
			}
		}
	}
	#endregion
}