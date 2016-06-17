#region Usings
using System;
using System.Collections;
using System.Collections.Generic;
using Buildron.Domain;
using Skahal.Logging;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Buildron.Application;


#endregion

[AddComponentMenu("Buildron/Scenes/MainSceneController")]
public class MainSceneController : MonoBehaviour 
{
	#region Fields
	private Queue<GameObject> m_buildsToDeploy = new Queue<GameObject>();
	private int m_deployedBuildsCount;
	private bool m_isRefreshingBuilds;
	private bool m_serverIsDown;
	#endregion
	
	#region Properties
	public float DeployBuildSeconds = 0.5f;
	public int MaxDeployedBuilds = 32;
	public Vector3 FirstColumnDeployPosition = new Vector3(-2.5f, 10, 0);
	public Vector3 SecondColumnDeployPosition = new Vector3(2.5f, 10, 0);
	public Vector3 HistoryColumnDeployPosition = new Vector3(0, 10, 20);
	public Text LastUpdateLabel;
	public Text LogMessageLabel;
	public Text ServerIPLabel;
	public InputField Title;
	public float DelaySecondsUntilMarkAsServerIsDown = 10;

	[Inject]
	public BuildGOService BuildGOService { get; set; }
	#endregion
	
	#region Methods
	
	#region Initialize
	private void Awake ()
	{
		SetLogMessage (string.Empty);
		UpdateServerIP ();

		Messenger.Register (
			gameObject, 
			"OnCIServerReady",
			"OnBuildRunRequested",
			"OnBuildStopRequested");
		
		var server = CIServerService.GetCIServer ();
		Title.text = server.Title;
	}

	public void StoreTitle()
	{
		var server = CIServerService.GetCIServer ();
		server.Title = Title.text;
		CIServerService.SaveCIServer (server);
	}

	private void OnCIServerReady ()
	{
		InitializeBuildService (); 	
		InitializeServer ();
		
		StartCoroutine (UpdateBuildsStatus ());	
		StartCoroutine (DeployBuilds ());
	}
	
	private void OnBuildRunRequested ()
	{
		ExecuteFocusedBuildCommand (BuildService.RunBuild);
	}
	
	private void OnBuildStopRequested ()
	{
		ExecuteFocusedBuildCommand (BuildService.StopBuild);
	}

	private void ExecuteFocusedBuildCommand (Action<RemoteControl, string> command)
	{
		var visibles = BuildGOService.GetVisibles ();
		
		if (visibles.Count == 1) {
			command (RemoteControlService.GetConnectedRemoteControl (), visibles [0].GetComponent<BuildController> ().Model.Id);
		}
	}
	
	private void InitializeBuildService ()
	{
		SetLogMessage ("Contacting {0} server. Please, wait...", BuildService.ServerName);
		
		BuildService.BuildFound += delegate(object sender, BuildFoundEventArgs e) {
			if (BuildService.BuildsCount == 1) {
				SetLogMessage ("");	
			}
		};
		
		BuildService.BuildsRefreshed += delegate {
			m_isRefreshingBuilds = false;
		};
		
		BuildService.CIServerStatusChanged += (e, args) => {
			if (args.Server.Status == CIServerStatus.Down) {
				StartCoroutine (DelayServerIsDown ());
				m_serverIsDown = true;
				m_isRefreshingBuilds = false;
			}
			else {
				m_serverIsDown = false;

				if (BuildService.BuildsCount != 0) {
					SetLogMessage ("");	
				}
			}
		};
	}
	
	private IEnumerator DelayServerIsDown ()
	{
		// If server is already down, so there is another DelayServerIsDown in action.
		if (!m_serverIsDown) {
			yield return new WaitForSeconds(DelaySecondsUntilMarkAsServerIsDown);
			
			// After the wait, the server still down?
			if (m_serverIsDown) {
				m_isRefreshingBuilds = false;
				SetLogMessage ("{0} server is unavailable!", BuildService.ServerName);
				Messenger.Send("OnServerDown");
			}
		}
	}

	private void InitializeServer ()
	{
		ServerService.Initialize ();
	}
	#endregion
	
	#region Updates
	private void Update ()
	{
		if (Input.GetKey (KeyCode.Escape)) {
			Application.Quit ();
		} 
	}
	
	private IEnumerator UpdateBuildsStatus ()
	{
		var refreshSeconds = CIServerService.GetCIServer ().RefreshSeconds;
		
		while (true) {				
			try {
				if (!m_isRefreshingBuilds) {
					SetLastUpdateMessage ("Updating...");
					m_isRefreshingBuilds = true;
					BuildService.RefreshAllBuilds ();	
					SetLastUpdateMessage ("Last update\n{0:HH:mm:ss}", System.DateTime.Now);
					UpdateServerIP();
				}
			} catch (System.Exception ex) {
				m_isRefreshingBuilds = false;
				var baseEx = ex.GetBaseException ();
				SetLogMessage ("ERROR: can't update. Please, check your connection with continuous integration server.\n{0}: {1}.", baseEx.GetType ().Name, baseEx.Message);
				SHLog.Warning (ex.Message);	
			}
			
			SHLog.Debug ("Builds updated. Next update in {0} seconds.", refreshSeconds);
			yield return new WaitForSeconds(refreshSeconds);
		}
	}
	
	private IEnumerator DeployBuilds ()
	{
		while (m_deployedBuildsCount < MaxDeployedBuilds) {
			if (m_buildsToDeploy.Count > 0) {
				m_buildsToDeploy.Dequeue ().SetActive (true);
				m_deployedBuildsCount++;
			}
		
			yield return new WaitForSeconds(DeployBuildSeconds);	
		}
	}
	#endregion
	
	#region Helpers
	private void SetLastUpdateMessage (string msg, params object[] args)
	{
		LastUpdateLabel.text = System.String.Format (msg, args);
	}
	
	private void SetLogMessage (string msg, params object[] args)
	{
		LogMessageLabel.text = System.String.Format (msg, args);
	}
	
	private void UpdateServerIP()
	{
		ServerIPLabel.text = Network.player.ipAddress;
	}
	#endregion
	
	#endregion
}
