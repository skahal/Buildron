#region Usings
using Buildron.Domain;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Skahal.Logging;
using System.Linq;
using Zenject;
using Buildron.Domain.Builds;


#endregion

[RequireComponent(typeof(AudioSource))]
public class BuildSoundEffectController : MonoBehaviour
{
    #region Fields
    [Inject]
    private ICIServerService m_ciServerService;

    private static List<AudioClip> s_sounds = new List<AudioClip>();
	#endregion

	#region Editor properties
	public SHSoundConfig FoundAudio;
	public SHSoundConfig FailedAudio;
	public SHSoundConfig QueuedAudio;
	public SHSoundConfig RunningAudio;
	public SHSoundConfig SuccessAudio;
	#endregion
	
	#region Methods
	private void Start ()
	{
		if (m_ciServerService.GetCIServer ().FxSoundsEnabled) {
			LoadSounds();

			Messenger.Register (gameObject, 
			"OnBuildFailed", 
			"OnBuildRunning",
			"OnBuildSuccess",
			"OnBuildQueued",
			"OnBuildReachGround");
		} else {
			Destroy (this);
		}
	}

	private void LoadSounds ()
	{
		if(s_sounds.Count == 0) 
		{
			StartCoroutine(LoadSound());
		}
	}

	private IEnumerator LoadSound ()
	{
		var folderPath = Application.dataPath.Substring (0, Application.dataPath.LastIndexOf ("/")) + "/mods/sounds/current/";
		folderPath = FixPath(folderPath);
		var soundFiles = Directory.GetFiles(folderPath, "*.wav", SearchOption.AllDirectories);
		
		SHLog.Debug("Found {0} sound files on folder {1} and subfolders.", soundFiles.Length, folderPath);
		
		foreach(var file in soundFiles)
		{
			var filename = "file://" + file;
			filename = FixPath(filename);

			var www = new WWW (filename);
			yield return www;
			var clip = www.GetAudioClip (true);
			clip.name = filename;

			SHLog.Debug("Sound file loaded: {0}", clip.name);
			s_sounds.Add(clip);
		}
	}

	private string FixPath(string path)
	{
		return path.Replace(@"\", "/");
	}
	
	private void OnBuildReachGround (GameObject buildGO)
	{
		PlayAudio (buildGO);
	}
	
	private void OnBuildFailed (GameObject buildGO)
	{
		PlayAudio (buildGO);	
	}
	
	private void OnBuildRunning (GameObject buildGO)
	{
		var controller = buildGO.GetComponent<BuildController> ();
		
		if (controller.Model.Status == BuildStatus.Running) {
			PlayAudio (buildGO);
		}
	}
	
	private void OnBuildSuccess (GameObject buildGO)
	{
		PlayAudio (buildGO);
	}
	
	private void OnBuildQueued (GameObject buildGO)
	{
		PlayAudio (buildGO);
	}
	
	private void PlayAudio (GameObject buildGO)
	{
		var build =  buildGO.GetComponent<BuildController> ().Model;

		if (build.TriggeredBy != null) {
			var username = build.TriggeredBy.UserName;
			var status = build.Status;

			var filter = string.Format ("/{0}/{1}", username, status);
			var availableSounds = s_sounds.Where (s => s.name.Contains (filter)).ToList ();

			if (availableSounds.Count == 0) {
				filter = string.Format ("/current/{0}", status);
				availableSounds = s_sounds.Where (s => s.name.Contains (filter)).ToList ();
			}

			SHLog.Debug ("Found {0} sounds for user {1} and status {2}.", availableSounds.Count, username, status);

			if (availableSounds.Count > 0 && buildGO.GetInstanceID () == gameObject.GetInstanceID ()) {
				transform.position = buildGO.transform.position;
				GetComponent<AudioSource> ().volume = 1f;
				GetComponent<AudioSource> ().clip = availableSounds [Random.Range (0, availableSounds.Count)];
				GetComponent<AudioSource> ().Play ();
			}
		}
	}
	
	#endregion
}