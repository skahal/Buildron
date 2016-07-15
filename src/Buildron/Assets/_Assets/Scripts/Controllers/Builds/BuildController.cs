using System;
using Buildron.Domain;
using Buildron.Domain.Builds;
using Skahal;
using Skahal.Camera;
using Skahal.Logging;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Buildron.Domain.Users;
using Buildron.Domain.RemoteControls;

/// <summary>
/// Build controller.
/// </summary>
public class BuildController : SHController<IBuild>
{
    #region Fields
    private static UnityEngine.Object s_buildFailedExplosionPrefab;
    private static UnityEngine.Object s_buildSuccessFireworksPrefab;
    private static UnityEngine.Object s_buildHidingEffectPrefab;

    [Inject]
    private IRemoteControlService m_remoteControlService;

    private BuildProgressBarController m_progressBar;
	private bool m_groundReachdAlreadRaised;
	private Text m_projectLabel;
	private Text m_configurationLabel;
	private Image m_runningStatusIcon;
	private Image m_userAvatarIcon;
    private Collider m_bodyCollider;
    private Renderer m_bodyRenderer;
    private Renderer m_focusedPanelRenderer;
	private bool m_isFirstCheckState = true;
    private BuildStatus m_lastCheckStateStatus;
	private GameObject m_focusedPanel;
    #endregion

	#region Properties
    /// <summary>
    /// Gets or sets user service.
    /// </summary>
    [Inject]
	public IUserService UserService { get; set; }    

   	public bool IsHistoryBuild { get; set; }
	public bool IsVisible { get; private set; }
	public bool HasReachGround { get; private set; }

	public GameObject Body { get; private set; }

	public Rigidbody Rigidbody { get; private set; }

	public Collider TopEdge { get; private set; }
	public Collider BottomEdge { get; private set; }
	public Collider LeftEdge { get; private set; }
	public Collider RightEdge { get; private set; }
	
	public string ProjectText {
		get {
			return m_projectLabel.text;
		}
		
		set {
			if (m_projectLabel == null) {
				m_projectLabel = transform.FindChild ("Canvas/ProjectLabel").GetComponent<Text> ();
			}
			
			m_projectLabel.text = value;
		}
	}
	#endregion
	
	#region Editor properties
	public Material BuildHidingMaterial;
	
	public Color SuccessColor = new Color (1, 0, 0.95f, 1);
	public Color FailedColor = new Color(1, 0, 0, 1);
	public Color RunningColor = new Color (1, 0.98f, 0, 1);
	public Color QueuedColor = new Color (0.36f, 0.45f, 0.55f, 1);
	
	public Material UserAvatarIconMaterial;
	public Vector3 StatusChangedForce = new Vector3(0, 10, 0);
	public float BuildHighlightZChange = -2;
	public float VisibleMaxYVelocity = 0.1f;
	public Sprite[] BuildRunningIcons;
	#endregion
	
	#region Life cycle 
	private void Start ()
	{        
		IsVisible = true;
		
		if (m_projectLabel == null) {
			ProjectText = Model.Configuration.Project.Name;
		}
		
		m_configurationLabel = transform.FindChild ("Canvas/ConfigurationLabel").GetComponent<Text> ();
		m_configurationLabel.text = Model.Configuration.Name;
		
		m_runningStatusIcon = transform.FindChild ("Canvas/RunningStatusIcon").GetComponent<Image> ();
		m_runningStatusIcon.enabled = false;
		m_userAvatarIcon = transform.FindChild ("Canvas/UserAvatarIcon").GetComponent<Image> ();
		m_userAvatarIcon.enabled = false;
		
		Body = transform.FindChild ("Buildron-Totem").gameObject;
		m_focusedPanel = transform.FindChild ("FocusedPanel").gameObject;		
		TopEdge = transform.FindChild ("Edges/TopEdge").GetComponent<Collider>();
		BottomEdge = transform.FindChild ("Edges/BottomEdge").GetComponent<Collider>();
		LeftEdge = transform.FindChild ("Edges/LeftEdge").GetComponent<Collider>();
		RightEdge = transform.FindChild ("Edges/RightEdge").GetComponent<Collider>();

        Rigidbody = GetComponent<Rigidbody>();
        m_bodyCollider = GetComponent<Collider>();
        m_bodyRenderer = Body.GetComponent<Renderer>();
        m_focusedPanelRenderer = m_focusedPanel.GetComponent<Renderer>();

        m_progressBar = GetComponentInChildren<BuildProgressBarController> ();
		CheckState ();
		MonitorTriggeredByPhotoUpdated ();
	
		if (!IsHistoryBuild) {
			Model.StatusChanged += delegate {
				if (Model.Status <= BuildStatus.Running && Model.Status != BuildStatus.Queued) {
					Rigidbody.AddForce (StatusChangedForce);
				}
				
				CheckState ();
			};
				
			Model.TriggeredByChanged += (sender, e) => {
				MonitorTriggeredByPhotoUpdated();

				UpdateUserAvatar ();	
			};
		}
		
		Messenger.Register (gameObject, 
			"OnRemoteControlConnected", 
			"OnRemoteControlDisconnected");
	} 

	private void MonitorTriggeredByPhotoUpdated()
	{
		if (Model.TriggeredBy != null) {
			Model.TriggeredBy.PhotoUpdated += delegate {
				UpdateUserAvatar ();
			};
		}
	}

    private void OnRemoteControlConnected ()
	{
		var rc = m_remoteControlService.GetConnectedRemoteControl ();
		var rcUserName = rc == null ? null : rc.UserName.ToLowerInvariant ();
		var userName = Model.TriggeredBy == null ? null : Model.TriggeredBy.UserName.ToLowerInvariant ();
		
		if (!String.IsNullOrEmpty (rcUserName) 
			&& !String.IsNullOrEmpty (userName)
			&& rcUserName.Equals (userName)) {
			m_userAvatarIcon.enabled = true;
		}
	}
	
	private void OnRemoteControlDisconnected ()
	{
		m_userAvatarIcon.enabled = true;
		UpdateUserAvatar();
	}
	
	private void CheckState ()
	{
        SHLog.Debug("{0} | {1} | {2}", m_isFirstCheckState, m_lastCheckStateStatus, Model.Status);
        var statusChanged = m_isFirstCheckState || m_lastCheckStateStatus != Model.Status;      

        if (Model.IsRunning) {
			m_progressBar.Show ();
		} else {
			m_progressBar.Hide ();
		}

        Color color;

        switch (Model.Status)
        {
            case BuildStatus.Failed:
            case BuildStatus.Error:
            case BuildStatus.Canceled:
                color = FailedColor;
                UpdateRunningStatusIcon(true);                

                if (statusChanged)
                {
                    CreateFailedEffects();
                    Messenger.Send("OnBuildFailed", gameObject);
                }
                break;

            case BuildStatus.Success:
                color = SuccessColor;
                UpdateRunningStatusIcon(true);                

                if (statusChanged)
                {
                    CreateSuccessEffects();
                    Messenger.Send("OnBuildSuccess", gameObject);
                }
                break;

            case BuildStatus.Queued:
                color = QueuedColor;
                UpdateRunningStatusIcon(true);

                if (statusChanged)
                {
                    Messenger.Send("OnBuildQueued", gameObject);
                }
                break;

            default:
                color = RunningColor;
                UpdateRunningStatusIcon(false);

                m_progressBar.UpdateValue(Model.PercentageComplete);

                if (statusChanged)
                {
                    Messenger.Send("OnBuildRunning", gameObject);
                }
                break;
        }

        UpdateUserAvatar();

        m_bodyRenderer.materials[1].SetColor("_Color", color);
        m_focusedPanelRenderer.materials[1].SetColor("_Color", color);

        m_isFirstCheckState = false;
        m_lastCheckStateStatus = Model.Status;
    }		
				
	private void UpdateRunningStatusIcon (bool hide)
	{	
		if (Model.LastRanStep == null) {
			m_runningStatusIcon.sprite = BuildRunningIcons[(int)BuildStepType.None];
		} else {
			m_runningStatusIcon.sprite = BuildRunningIcons[(int)Model.LastRanStep.StepType];
		}			
		
		m_runningStatusIcon.enabled = IsVisible && !hide;
	}
	
	private void UpdateUserAvatar ()
	{
		m_userAvatarIcon.enabled = !(!IsVisible || Model.IsRunning);

		if (Model.TriggeredBy != null) {
			var photo = Model.TriggeredBy.Photo;
	
			if (photo != null) {
				m_userAvatarIcon.sprite = photo.ToSprite ();
			}
		}
	}
	
	private void OnCollisionEnter ()
	{
		if (!m_groundReachdAlreadRaised && !IsHistoryBuild) {
			m_groundReachdAlreadRaised = true;
			HasReachGround = true;
			Messenger.Send ("OnBuildReachGround", gameObject);
		}
	}

	private void Hide ()
	{
		if (IsVisible)
        {            
			CheckState ();
			CreateHiddingEffect ();

            Rigidbody.isKinematic = true;
			m_bodyCollider.enabled = false;
            TopEdge.enabled = false;
            RightEdge.enabled = false;
            BottomEdge.enabled = false;
            LeftEdge.enabled = false;
            
			Body.SetActive(false);
			IsVisible = false;
			m_runningStatusIcon.enabled = false;
			m_userAvatarIcon.enabled = false;
			m_projectLabel.enabled = false;
			m_configurationLabel.enabled = false;
			m_progressBar.Hide ();
            HasReachGround = false;
            m_groundReachdAlreadRaised = false;
            Messenger.Send ("OnBuildHidden");
        }
	}

	private void Show ()
	{
		if (!IsVisible)
        {
			CheckState ();
			transform.position += Vector3.up * 20;

            Rigidbody.isKinematic = false;
			m_bodyCollider.enabled = true;
            TopEdge.enabled = true;
            RightEdge.enabled = true;
            BottomEdge.enabled = true;
            LeftEdge.enabled = true;

            Body.SetActive(true);
			IsVisible = true;			
			UpdateUserAvatar ();
			m_projectLabel.enabled = true;
			m_configurationLabel.enabled = true;

            if (Model.IsRunning)
            {
                m_runningStatusIcon.enabled = true;
                m_progressBar.Show();
            }

			Messenger.Send ("OnBuildVisible");
		}
	}
	
	private void CreateFailedEffects ()
	{
		if (!IsHistoryBuild) {

            if (s_buildFailedExplosionPrefab == null)
            {
                s_buildFailedExplosionPrefab = Resources.Load("BuildFailedExplosionPrefab");
            }

			var explosion = (GameObject)GameObject.Instantiate (s_buildFailedExplosionPrefab);
			explosion.transform.parent = transform;
			explosion.transform.position = transform.position;
			SHCameraHelper.Shake ();
		}
	}
	
	private void CreateSuccessEffects ()
	{
		if (!m_isFirstCheckState && !IsHistoryBuild) {

            if (s_buildSuccessFireworksPrefab == null)
            {
                s_buildSuccessFireworksPrefab = Resources.Load("BuildSuccessFireworksPrefab");
            }

			var fireworks = (GameObject)GameObject.Instantiate (s_buildSuccessFireworksPrefab);
			fireworks.transform.parent = transform;
			fireworks.transform.position = transform.position;
		}
	}
	
	private void CreateHiddingEffect ()
	{
        if (s_buildHidingEffectPrefab == null)
        {
            s_buildHidingEffectPrefab = Resources.Load("BuildHidingEffectPrefab");
        }

		var effect = (GameObject)GameObject.Instantiate (s_buildHidingEffectPrefab);
		effect.transform.parent = transform;
		effect.transform.position = transform.position;
		effect.GetComponent<ParticleSystem> ().Play ();
	}
    #endregion

    /// <summary>
    /// The build controller factory.
    /// </summary>
    public class Factory : Factory<BuildController>
	{
	}
}