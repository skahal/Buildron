#region Usings
using UnityEngine;
using System.Collections;
using Buildron.Domain;
using Zenject;
using Buildron.Application;


#endregion

/// <summary>
/// Builds history controller.
/// </summary>
public class BuildsHistoryController : MonoBehaviour {
	
	#region Fields
	private GameObject m_container;
	private int m_historyCount;
	#endregion
	
	#region Properties
	public Vector3 HistoryTotemPosition = new Vector3(0, 0, 20);
	public float YCreationMultiplier = 12;

	[Inject]
	public BuildGOService Service { get; set; }
	#endregion
	
	#region Methods
	private void Start ()
	{
		m_container = new GameObject ("History");
		
		Messenger.Register (
			gameObject,
			"OnBuildSuccess",
			"OnBuildFailed",
			"OnCIServerReady"
			);
	}
	
	private void OnBuildSuccess (GameObject buildGO)
	{
		CreateHistoryBuild (buildGO);
	}
	
	private void OnBuildFailed (GameObject buildGO)
	{
		CreateHistoryBuild (buildGO);
	}
	
	private void OnCIServerReady ()
	{
		if (!CIServerService.GetCIServer ().HistoryTotemEnabled) {
			enabled = false;
			Destroy(this);	
		}
	}
	
	private void CreateHistoryBuild (GameObject buildGO)
	{
		var originalController = buildGO.GetComponent<BuildController> ();
		
		if (!originalController.IsHistoryBuild) {
			var originalBuild = originalController.Model;
			
			if (originalBuild.Date.Date == System.DateTime.Now.Date) {
				m_historyCount ++;
				Build historyBuild = (Build)originalBuild.Clone (); 
				historyBuild.Id = string.Format ("{0}_{1}_history", historyBuild.Id, System.DateTime.Now.Ticks);
				
				var cloneGO = Service.CreateGameObject (historyBuild);
				cloneGO.tag = "History";
				var controller = cloneGO.GetComponent<BuildController> ();
				controller.IsHistoryBuild = true;
				controller.ProjectText = string.Format ("{0} - {1}", m_historyCount, historyBuild.Configuration.Project.Name);
				cloneGO.transform.position = new Vector3 (HistoryTotemPosition.x, YCreationMultiplier + (m_historyCount * YCreationMultiplier), HistoryTotemPosition.z);
				cloneGO.transform.parent = m_container.transform;

				Messenger.Send ("OnBuildHistoryCreated");
			}
		}
	}
	
	public static GameObject[] GetAll()
	{
		return GameObject.FindGameObjectsWithTag("History");
	}
	#endregion
}
