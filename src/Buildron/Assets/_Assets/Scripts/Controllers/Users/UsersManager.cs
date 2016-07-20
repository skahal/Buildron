#region Usings
using UnityEngine;
using System.Collections;
using Buildron.Domain;
using Zenject;
using Buildron.Domain.Builds;
using Buildron.Domain.Users;


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

	[Inject]
	private IBuildService m_buildService;
	#endregion
	
	#region Properties
	public Vector3 FirstSpawnPosition = new Vector3(-10, -3, -10);
	public Vector3 DistanceBetweenUsers = new Vector3(2, 0, 0);
	public int NumberUserPerRows = 5;
	public Vector3 DistanceBetweenUsersRows = new Vector3(0, 0, 2);
	[Inject]
	public UserController.Factory Factory { get; set; }
	#endregion
	
	#region Methods
	private void Awake ()
	{
		m_buildService.BuildFound += (sender, e) => {
			e.Build.StatusChanged += (sender1, e1) => {
				CreateUserGameObject(e.Build);
			};
		};

		m_currentSpawnPosition = FirstSpawnPosition;
	}

	private void CreateUserGameObject (Build build)
	{
		if (build.TriggeredBy == null) {
			build.TriggeredByChanged += delegate {
				CreateUserGameObject (build);
			};
			
		} else {
			CreateUserGameObject (build);
		}
	}

	void CreateUserGameObject (IBuild build)
	{
		var go = UserController.GetGameObject (build.TriggeredBy);
		
		if (go != null) {
			go.GetComponent<UserController> ().Data = build.TriggeredBy;
		} else {
			go = UserController.CreateGameObject (build.TriggeredBy, Factory);
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