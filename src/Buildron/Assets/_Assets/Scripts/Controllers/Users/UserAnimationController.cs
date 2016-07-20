#region Usings
using UnityEngine;
using System.Collections;
using Buildron.Domain;
using Skahal.Logging;
using System.Collections.Generic;
using Zenject;
using Buildron.Domain.Builds;
using Buildron.Domain.Users;


#endregion

/// <summary>
/// User animation controller.
/// </summary>
public class UserAnimationController : MonoBehaviour {

    #region Fields
    [Inject]
    private IBuildService m_buildService;

    private BuildStatus? m_currentStatus;
	private Queue<string> m_animationsQueue = new Queue<string> ();
	#endregion
	
	#region Properties
	public IUser Data { get; set; }
	public string SuccessAnimation = "ledgeFall";
	public string RunningAnimation = "idle";
	public string FailedAnimation = "gotHit";
	#endregion
	
	#region Methods
	private void Start ()
	{
		StartCoroutine (RefreshAnimation ());	
	}
	
	private IEnumerator RefreshAnimation ()
	{
		while (true) {
			yield return new WaitForSeconds(0.1f);
			
			if (!GetComponent<Animation>().isPlaying && m_animationsQueue.Count > 0) {
				GetComponent<Animation>().Play (m_animationsQueue.Dequeue ());
			}
		}
	}
	
	public void Play ()
	{
		var build = m_buildService.GetMostRelevantBuildForUser (Data);
				
		if (build == null) {
			AnimateAsSuccess ();
		} else {
			if (m_currentStatus != build.Status) {
				GetComponent<Animation>().Stop();
				m_currentStatus = build.Status;
				
				if (build.IsRunning() || build.IsQueued()) {
					AnimateAsRunning ();
				
				} else if (build.IsFailed()) {
					AnimateAsFailed ();
				
				} else {
					AnimateAsSuccess ();
				}	
			}
		}
	}

	private void AnimateAsRunning()
	{
		Play (RunningAnimation);
	}

	private void AnimateAsFailed()
	{
		Play (FailedAnimation);
	}

	private void AnimateAsSuccess ()
	{
		Play (SuccessAnimation);
	}
	
	private void Play (string name)
	{
		m_animationsQueue.Enqueue (name);
	}
	
	private void PlaySingle (string name)
	{
		GetComponent<Animation>().Stop ();
		m_currentStatus = null;
		Play (name);
		Play ();
	}
	
	public void PlayKick ()
	{
		PlaySingle ("kick");
	}
	
	public void PlayPunch ()
	{
		PlaySingle ("punch");
	}
	#endregion
}
