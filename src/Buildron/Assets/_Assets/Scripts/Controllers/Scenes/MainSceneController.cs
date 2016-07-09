using System;
using System.Collections;
using System.Collections.Generic;
using Buildron.Domain;
using Skahal.Logging;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Buildron.Application;
using Buildron.Domain.Builds;
using Buildron.Domain.CIServers;
using Buildron.Domain.RemoteControls;

[AddComponentMenu("Buildron/Scenes/MainSceneController")]
public class MainSceneController : MonoBehaviour, IInitializable
{
    #region Fields
    private int m_deployedBuildsCount;
    private bool m_isRefreshingBuilds;
    private DateTime m_lastUpdateTime = DateTime.Now;

    [Inject]
    private IBuildService m_buildService;

    [Inject]
    private IRemoteControlService m_remoteControlService;

    [Inject]
    private ICIServerService m_ciServerService;

    [Inject]
    private BuildGOService m_buildGOService { get; set; }
    #endregion

    #region Properties

    public float DeployBuildSeconds = 0.5f;
    public float MaxTimeWithoutUpdate = 120f;
    public Vector3 FirstColumnDeployPosition = new Vector3(-2.5f, 10, 0);
    public Vector3 SecondColumnDeployPosition = new Vector3(2.5f, 10, 0);
    public Vector3 HistoryColumnDeployPosition = new Vector3(0, 10, 20);
    public Text LastUpdateLabel;
    public Text LogMessageLabel;
    public Text ServerIPLabel;
    public InputField Title;

    #endregion

    #region Methods

    #region Initialize

    private void Awake()
    {
        SetLogMessage(string.Empty);
        UpdateServerIP();

        Messenger.Register(
            gameObject,
            "OnCIServerReady",
            "OnBuildRunRequested",
            "OnBuildStopRequested");
    }

    public void Initialize()
    {
        var server = m_ciServerService.GetCIServer();
        Title.text = server.Title;
    }

    public void StoreTitle()
    {
        var server = m_ciServerService.GetCIServer();
        server.Title = Title.text;
        m_ciServerService.SaveCIServer(server);
    }

    private void OnCIServerReady()
    {
        InitializeBuildService();
        StartCoroutine(UpdateBuildsStatus());
        StartCoroutine(CheckLastUpdate());
    }

    private void OnBuildRunRequested()
    {
        ExecuteFocusedBuildCommand(m_buildService.RunBuild);
    }

    private void OnBuildStopRequested()
    {
        ExecuteFocusedBuildCommand(m_buildService.StopBuild);
    }

    private void ExecuteFocusedBuildCommand(Action<IRemoteControl, string> command)
    {
        var visibles = m_buildGOService.GetVisibles();

        if (visibles.Count == 1)
        {
            command(m_remoteControlService.GetConnectedRemoteControl(), visibles[0].GetComponent<BuildController>().Model.Id);
        }
    }

    private void InitializeBuildService()
    {
        SetLogMessage("Contacting {0} server. Please, wait...", m_buildService.ServerName);

        m_buildService.BuildFound += delegate (object sender, BuildFoundEventArgs e)
        {
            if (m_buildService.Builds.Count == 1)
            {
                SetLogMessage("");
            }
        };

        m_buildService.BuildsRefreshed += delegate
        {
            m_isRefreshingBuilds = false;
        };

        m_ciServerService.CIServerStatusChanged += (e, args) =>
        {
            if (args.Server.Status == CIServerStatus.Down)
            {
                m_isRefreshingBuilds = false;
                SetLogMessage("{0} server is unavailable!", m_buildService.ServerName);
                Messenger.Send("OnServerDown");
            }
            else
            {
                if (m_buildService.Builds.Count != 0)
                {
                    SetLogMessage("");
                }
            }
        };
    }

    #endregion

    #region Updates

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private IEnumerator UpdateBuildsStatus()
    {
        var refreshSeconds = m_ciServerService.GetCIServer().RefreshSeconds;

        while (true)
        {
            try
            {
                if (!m_isRefreshingBuilds)
                {
                    SetLastUpdateMessage("Updating...");
                    m_isRefreshingBuilds = true;
                    m_buildService.RefreshAllBuilds();
                    m_lastUpdateTime = DateTime.Now;
                    SetLastUpdateMessage("Last update\n{0:HH:mm:ss}", m_lastUpdateTime);
                    UpdateServerIP();
                }
            }
            catch (System.Exception ex)
            {
                m_isRefreshingBuilds = false;
                var baseEx = ex.GetBaseException();
                SetLogMessage("ERROR: can't update. Please, check your connection with continuous integration server.\n{0}: {1}.", baseEx.GetType().Name, baseEx.Message);
                SHLog.Warning(ex.Message);
            }

            SHLog.Debug("Builds updated. Next update in {0} seconds.", refreshSeconds);
            yield return new WaitForSeconds(refreshSeconds);
        }
    }

    private IEnumerator CheckLastUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(MaxTimeWithoutUpdate);

            var diff = DateTime.Now - m_lastUpdateTime;

            if (diff.TotalSeconds > MaxTimeWithoutUpdate)
            {
				SHLog.Warning ("MaxTimeWithoutUpdate reach, mark to refresh builds");
                m_isRefreshingBuilds = false;
            }
        }
    }
    #endregion

    #region Helpers

    private void SetLastUpdateMessage(string msg, params object[] args)
    {
        LastUpdateLabel.text = System.String.Format(msg, args);
    }

    private void SetLogMessage(string msg, params object[] args)
    {
        LogMessageLabel.text = System.String.Format(msg, args);
    }

    private void UpdateServerIP()
    {
        ServerIPLabel.text = Network.player.ipAddress;
    }

    #endregion

    #endregion
}
