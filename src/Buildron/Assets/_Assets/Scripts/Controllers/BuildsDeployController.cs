#region Usings
using UnityEngine;
using System.Collections;
using Buildron.Domain;
using System.Collections.Generic;
using UnityEngine.UI;


#endregion

/// <summary>
/// Builds deploy controller.
/// </summary>
public class BuildsDeployController : MonoBehaviour
{
	#region Fields
	private GameObject m_container;
	private Vector3 m_initialDeployPosition;
	private Vector3 m_currentDeployPosition;
	private int m_deployedBuildsCount;
	private int m_currentTotemIndex;
	private Queue<GameObject> m_buildsToDeploy = new Queue<GameObject> ();
	public int m_totemsNumber = 2;
	#endregion
	
	#region Editor properties
	public Vector3 DeployCenterPosition = new Vector3(0, 20, 1.5f);
	public float DeployInterval = 0.5f;
	public float TotemsDistance = 10;
	public Text BuildsCountLabel;
	#endregion
	
	private	void Awake ()
	{
		m_container = new GameObject ("Builds");
		
		Messenger.Register (
			gameObject, 
			"OnCIServerReady");
	}
	
	private void OnCIServerReady ()
	{
		m_totemsNumber = CIServerService.GetCIServer ().BuildsTotemsNumber;
		m_initialDeployPosition = CalculateInitialPosition ();
		m_currentDeployPosition = m_initialDeployPosition;
	
		BuildService.BuildFound += delegate(object sender, BuildFoundEventArgs e) {		
			UpdateBuild (e.Build);
		};
		
		StartCoroutine (DeployBuilds ());
	}
	
	private void UpdateBuild (Build b)
	{
		if (!BuildController.ExistsGameObject (b)) {
			var go = BuildController.CreateGameObject (b);
			go.transform.parent = m_container.transform;
			m_currentDeployPosition.x = m_initialDeployPosition.x + (m_currentTotemIndex * TotemsDistance);
			go.transform.position = m_currentDeployPosition;
			go.SetActive (false);
			
			m_buildsToDeploy.Enqueue (go);
		}
		
		m_currentTotemIndex++;
		
		if (m_currentTotemIndex >= m_totemsNumber) {
			m_currentTotemIndex = 0;
			m_initialDeployPosition = CalculateInitialPosition ();
		}
		
		m_currentDeployPosition += Vector3.up;
	}
	
	private Vector3 CalculateInitialPosition ()
	{
		var intialPosition = DeployCenterPosition;
		var totemsNumberModifier = m_totemsNumber / 2;
		var totemsDistanceModifier = TotemsDistance;
		
		if (m_totemsNumber % 2 == 0) {
			totemsDistanceModifier -= TotemsDistance / m_totemsNumber;
		}	
		
		intialPosition.x = intialPosition.x - totemsNumberModifier * totemsDistanceModifier;
	
		return intialPosition;
	}
	
	private IEnumerator DeployBuilds ()
	{
		while (true) {
			if (m_buildsToDeploy.Count > 0) {
				m_buildsToDeploy.Dequeue ().SetActive (true);
				m_deployedBuildsCount++;
				BuildsCountLabel.text = string.Format ("Builds\n{0}", m_deployedBuildsCount);
			}
		
			yield return new WaitForSeconds(DeployInterval);	
		}
	}
}