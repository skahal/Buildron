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

/// <summary>
/// Sorting controller.
/// </summary>
public class SortingController : MonoBehaviour
{
    #region Fields
    private bool m_shouldUpdateStatusBar;
    #endregion

	#region Properties
	[Inject]
	public BuildGOService BuildGOService { get; set; }
	#endregion

    #region Methods
    private void Start()
    {      
        BuildService.BuildsRefreshed += (sender, e) =>
        {
            // New builds were found or if an existing one changed the status, sort it.
            if (e.BuildsFound.Count > 0 || e.BuildsStatusChanged.Count > 0)
            {
                PerformOnBuildSortUpdated();
            }
        };        

        Messenger.Register(
            gameObject,
            "OnBuildSortUpdated",
            "OnSortingItemsSwapped",
            "OnSortingEnded");
    }

    private void PerformOnBuildSortUpdated()
    {
        SHThread.Start(
        1f, // This 1 second give the time to build physics activate when became visible because a filter sent from RC.
        () =>
        {
            SHThread.WaitFor(
            () =>
            {
                var areAllSleeping = BuildGOService.AreAllSleeping();
                SHLog.Warning(
                    "Waiting all builds physics sleep. Are all sleeping: {0}",
                    areAllSleeping);

                return areAllSleeping;
            },
            () =>
            {
                var state = ServerState.Instance;
                SHLog.Debug("Sorting - IsSorting: {0}, AlgorithmType: {1}, SortBy: {2}", state.IsSorting, state.BuildSortingAlgorithmType, state.BuildSortBy);

                if (!state.IsSorting)
                {
                    OnBuildSortUpdated(new BuildSortUpdatedEventArgs(state.BuildSortingAlgorithmType, state.BuildSortBy));
                }
            });
        });
    }

    private void OnBuildSortUpdated(BuildSortUpdatedEventArgs args)
    {
        if (!ServerState.Instance.IsSorting)
        {            
           ServerState.Instance.IsSorting = true;

            // TODO: args.SortingAlgorithm is ignored because RC is not passing this at this time.            
            var sorting = SortingAlgorithmFactory.CreateRandomSortingAlgorithm<Build>();
            var comparer = BuildService.GetComparer(args.SortBy);            

            m_shouldUpdateStatusBar = !(sorting is NoneSortingAlgorithm<Build>);
            UpdateStatusBar("Sorting by " + comparer + " using: " + sorting.Name);

            var buildsGO = BuildGOService.GetVisiblesOrderByPosition();
            var builds = new List<Build>();

            foreach (var go in buildsGO)
            {
                go.GetComponent<Rigidbody>().isKinematic = true;
                builds.Add(go.GetComponent<BuildController>().Model);
            }

            sorting.Sort(builds, comparer);
        }
    }

    private void OnSortingItemsSwapped(object[] items)
    {
        var b1 = (Build)items[0];
        var b2 = (Build)items[1];

        var b1GO = BuildGOService.GetGameObject(b1);
        var b2GO = BuildGOService.GetGameObject(b2);

        SHLog.Debug("Swapping position between {0} and {1}...", b1GO.name, b2GO.name);

        var b1Position = b1GO.transform.position;

        AnimateSwap(b1GO, b2GO.transform.position);
        AnimateSwap(b2GO, b1Position);
    }

    private void AnimateSwap(GameObject go, Vector3 toPosition)
    {
        iTweenHelper.MoveTo(
            go,
            iT.MoveTo.position, toPosition,
            iT.MoveTo.time, SortingAlgorithmFactory.SwappingTime - 0.1f,
            iT.MoveTo.easetype, iTween.EaseType.easeInOutBack);
    }


    private void OnSortingEnded()
    {
        foreach (var go in BuildGOService.GetVisiblesOrderByPosition())
        {
            go.GetComponent<Rigidbody>().isKinematic = false;
        }

        ServerState.Instance.IsSorting = false;
        UpdateStatusBar("Sorting finished.", 2f);
    }

    private void UpdateStatusBar(string text, float secondsTimeout = 0)
    {
        SHLog.Warning(text);

        if (m_shouldUpdateStatusBar)
        {            
            StatusBarController .SetStatusText(text, secondsTimeout);
        }
    }
    #endregion
}