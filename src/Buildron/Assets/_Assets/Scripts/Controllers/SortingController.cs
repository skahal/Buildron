using System.Collections.Generic;
using Buidron.Domain;
using Buildron.Domain;
using Buildron.Domain.Sorting;
using Skahal.Logging;
using Skahal.Threading;
using Skahal.Tweening;
using UnityEngine;
using Zenject;
using Buildron.Application;
using System.Linq;

/// <summary>
/// Sorting controller.
/// </summary>
public class SortingController : MonoBehaviour
{
	#region Fields
	[Inject]
	private BuildGOService m_buildGOService;
	#endregion

	#region Methods
	private void Start ()
	{      
		BuildService.BuildsRefreshed += (sender, e) => {
			// New builds were found or if an existing one changed the status, sort it.
			if (e.BuildsFound.Count > 0 || e.BuildsStatusChanged.Count > 0) {
				PerformOnBuildSortUpdated ();
			}
		};        

		Messenger.Register (
			gameObject,
			"OnBuildSortUpdated");
	}

	private void PerformOnBuildSortUpdated ()
	{
		SHThread.Start (
			1f, // This 1 second give the time to build physics activate when became visible because a filter sent from RC.
			() => {
				SHThread.WaitFor (
					() => {
						var areAllSleeping = m_buildGOService.AreAllSleeping ();
						SHLog.Warning (
							"Waiting all builds physics sleep. Are all sleeping: {0}",
							areAllSleeping);

						return areAllSleeping;
					},
					() => {
						var state = ServerState.Instance;
						SHLog.Debug ("Sorting - IsSorting: {0}, AlgorithmType: {1}, SortBy: {2}", state.IsSorting, state.BuildSortingAlgorithmType, state.BuildSortBy);

						if (!state.IsSorting) {
							var sorting = SortingAlgorithmFactory.CreateSortingAlgorithm<Build> (state.BuildSortingAlgorithmType);
							OnBuildSortUpdated (new BuildSortUpdatedEventArgs (sorting, state.BuildSortBy));
						}
					});
			});
	}

	private void OnBuildSortUpdated (BuildSortUpdatedEventArgs args)
	{		
		var sorting = args.SortingAlgorithm;
		var comparer = BuildService.GetComparer (args.SortBy);
        var builds = m_buildGOService
            .GetVisiblesOrderByPosition()
            .Select(go => go.GetComponent<BuildController>().Model)
            .ToList();        

        sorting.SortingBegin += SortingBegin;
        sorting.SortingItemsSwapped += SortingItemsSwapped;
        sorting.SortingEnded += SortingEnded;

        StartCoroutine(sorting.Sort(builds, comparer));        
	}

    private void SortingBegin (object sender, SortingBeginEventArgs args)
    {
        if (!args.WasAlreadySorted)
        {
            ServerState.Instance.IsSorting = true;
            m_buildGOService.FreezeAll();

            var sorting = sender as ISortingAlgorithm<Build>;
            UpdateStatusBar("Sorting by {0}  using: {1}".With(sorting.Comparer, sorting.Name));
        }
    }

	private void SortingItemsSwapped (object sender, SortingItemsSwappedEventArgs<Build> args)
	{
		var b1 = args.Item1;
		var b2 = args.Item2;

		var b1GO = m_buildGOService.GetGameObject (b1);
		var b2GO = m_buildGOService.GetGameObject (b2);

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

	private void SortingEnded (object sender, SortingEndedEventArgs args)
	{
        if (!args.WasAlreadySorted)
        {
            m_buildGOService.UnfreezeAll();

            ServerState.Instance.IsSorting = false;
            UpdateStatusBar("Sorting finished.", 2f);
        }
	}

	private void UpdateStatusBar (string text, float secondsTimeout = 0)
	{
		SHLog.Warning (text);
		StatusBarController.SetStatusText (text, secondsTimeout);		
	}

	#endregion
}