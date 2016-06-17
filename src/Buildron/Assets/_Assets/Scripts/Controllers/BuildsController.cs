using UnityEngine;
using System.Collections;
using Buildron.Application;
using Zenject;

public class BuildsController : MonoBehaviour {

	[Inject]
	public BuildGOService Service { get; set; }

	void Start () {
		Messenger.Register (gameObject, 
			"OnBuildHidden");
	}
	
	void OnBuildHidden () {
		Service.WakeUpSleepingBuilds ();
	}
}
