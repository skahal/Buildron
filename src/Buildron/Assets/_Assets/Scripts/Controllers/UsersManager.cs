#region Usings
using UnityEngine;
using System.Collections;
using Buildron.Domain;
#endregion

/// <summary>
/// Manages the UserController creations.
/// </summary>
[AddComponentMenu("Buildron/Controllers/UsersManager")]
public class UsersManager : MonoBehaviour
{
	#region Fields
	private Vector3 m_currentSpawnPosition;
	private int m_currentRowUserCount;
	private int m_rowsCount = 1;
	#endregion
	
	#region Editor Properties
	public Vector3 FirstSpawnPosition = new Vector3(-10, -3, -10);
	public Vector3 DistanceBetweenUsers = new Vector3(2, 0, 0);
	public int NumberUserPerRows = 5;
	public Vector3 DistanceBetweenUsersRows = new Vector3(0, 0, 2);
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
		CreateUserGameObject (buildGO);
	}
	
	private void OnBuildSuccess (GameObject buildGO)
	{
		CreateUserGameObject (buildGO);
	}
	
	private void OnBuildRunning (GameObject buildGO)
	{
		CreateUserGameObject (buildGO);
	}
	
	private void OnBuildQueued (GameObject buildGO)
	{
		CreateUserGameObject (buildGO);
	}
	
	private void CreateUserGameObject (GameObject buildGO)
	{
		var build = buildGO.GetComponent<BuildController> ().Data;
	
		if (build.TriggeredBy == null) {
			build.TriggeredByChanged += delegate {
				CreateUserGameObject (build);
			};
			
		} else {
			CreateUserGameObject (build);
		}
	}

	void CreateUserGameObject (Build build)
	{
		var go = UserController.GetGameObject (build.TriggeredBy);
		
		if (go != null) {
			go.GetComponent<UserController> ().Data = build.TriggeredBy;
		} else {
			go = UserController.CreateGameObject (build.TriggeredBy);
			go.transform.position = m_currentSpawnPosition;
			go.transform.parent = transform;			
			
			m_currentSpawnPosition += DistanceBetweenUsers;
			m_currentRowUserCount++;
			
			if (m_currentRowUserCount >= NumberUserPerRows) {
				m_currentRowUserCount = 0;
				m_currentSpawnPosition = FirstSpawnPosition;
				m_currentSpawnPosition += DistanceBetweenUsersRows * m_rowsCount;
				m_rowsCount++;
			}
		}
	}
	#endregion
}