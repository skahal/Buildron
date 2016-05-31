#region Usings
using System.Collections.Generic;
using Buidron.Domain;
using Buildron.Domain;
using Buildron.Domain.Sorting;
using Skahal.Logging;
using UnityEngine;
using Skahal.Tweening;


#endregion

/// <summary>
/// Sorting controller.
/// </summary>
public class SortingController : MonoBehaviour {
	
	#region Methods
	private void Start ()
	{
		Messenger.Register (
			gameObject, 
			"OnBuildSortUpdated",
			"OnSortingItemsSwapped",
			"OnSortingEnded");
	}
	
	private void OnBuildSortUpdated (BuildSortUpdatedEventArgs args)
	{
		if (!ServerState.Instance.IsSorting) {
			ServerState.Instance.IsSorting = true;
			var sorting = SortingAlgorithmFactory.CreateSortingAlgorithm<Build> (args.SortingAlgorithm);	
			var comparer = BuildService.GetComparer(args.SortBy);
			
			StatusBarController.SetStatusText ("Sorting by " + comparer + " using: " + sorting.Name);
			
			var buildsGO = BuildController.GetVisiblesOrderByPosition ();
			var builds = new List<Build> ();
		
			foreach (var go in buildsGO) {
				go.GetComponent<Rigidbody>().isKinematic = true;
				builds.Add (go.GetComponent<BuildController> ().Data);
			}
				
			sorting.Sort (builds, comparer);
		}
	}
	
	private void OnSortingItemsSwapped (object[] items)
	{
		var b1 = (Build)items [0];
		var b2 = (Build)items [1];
		
		var b1GO = BuildController.GetGameObject (b1);
		var b2GO = BuildController.GetGameObject (b2);
		
		SHLog.Debug ("Swapping position between {0} and {1}...", b1GO.name, b2GO.name);
		
		var b1Position = b1GO.transform.position;
		
		AnimateSwap (b1GO, b2GO.transform.position);
		AnimateSwap (b2GO, b1Position);	
	}
	
	private void AnimateSwap (GameObject go, Vector3 toPosition)
	{
		iTweenHelper.MoveTo (
			go, 
			iT.MoveTo.position, toPosition,
			iT.MoveTo.time, SortingAlgorithmFactory.SwappingTime - 0.1f,
			iT.MoveTo.easetype, iTween.EaseType.easeInOutBack);
	}
	
	private void OnSortingEnded ()
	{
		foreach (var go in BuildController.GetVisiblesOrderByPosition ()) {
			go.GetComponent<Rigidbody>().isKinematic = false;
		}
		
		ServerState.Instance.IsSorting = false;
		
		StatusBarController.SetStatusText ("Sorting finished.", 2f);
	}
	#endregion
}