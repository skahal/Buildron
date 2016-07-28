using System.Collections.Generic;
using Buildron.Domain;
using Buildron.Domain.Sorting;
using Skahal.Logging;
using Skahal.Threading;
using Skahal.Tweening;
using UnityEngine;
using Zenject;
using System.Linq;
using Buildron.Domain.Builds;
using Buildron.Domain.Servers;
using Buildron.Domain.Mods;

/// <summary>
/// Sorting controller.
/// </summary>
public class SortingController : MonoBehaviour
{
	#region Fields
    [Inject]
    private IBuildService m_buildService;

	[Inject]
	private IServerService m_serverService;

	[Inject]
	private ISHLogStrategy m_log;

    [Inject]
    private IBuildGameObjectsProxy m_buildsGO;
    #endregion

    #region Methods
    private void Start ()
	{
        m_buildService.BuildsRefreshed += (sender, e) => {
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
        SHCoroutine.Start(
            1f, // This 1 second give the time to build physics activate when became visible because a filter sent from RC.
            () =>
            {
                SHCoroutine.WaitFor(
                    () =>
                    {
                        var areAllSleeping = m_buildsGO.GetAll().AreAllSleeping();
                        m_log.Warning(
                            "Waiting all builds physics sleep. Are all sleeping: {0}",
                            areAllSleeping);

                        return areAllSleeping;
                    },
                    () =>
                    {
                        var state = m_serverService.GetState();
                        m_log.Debug("Sorting - IsSorting: {0}, AlgorithmType: {1}, SortBy: {2}", state.IsSorting, state.BuildSortingAlgorithmType, state.BuildSortBy);

                        if (!state.IsSorting)
                        {
                            var sorting = SortingAlgorithmFactory.CreateSortingAlgorithm<IBuild>(state.BuildSortingAlgorithmType);
                            OnBuildSortUpdated(new BuildSortUpdatedEventArgs(sorting, state.BuildSortBy));
                        }
                    });
            });
    }

	private void OnBuildSortUpdated (BuildSortUpdatedEventArgs args)
	{
        var sorting = args.SortingAlgorithm;
        var comparer = BuildComparerFactory.Create(args.SortBy);
        var builds = m_buildsGO.GetAll().GetVisiblesOrderByPosition()
            .Select(go => ((IBuildController)go).Model)
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
            m_serverService.GetState().IsSorting = true;
            m_buildsGO.GetAll().FreezeAll();

            var sorting = sender as ISortingAlgorithm<IBuild>;
            UpdateStatusBar("Sorting by {0}  using: {1}".With(sorting.Comparer, sorting.Name));
        }
    }

	private void SortingItemsSwapped (object sender, SortingItemsSwappedEventArgs<IBuild> args)
	{
        var b1 = args.Item1;
        var b2 = args.Item2;

        var all =  m_buildsGO.GetAll();
        var b1GO = all.GetGameObject(b1);
        var b2GO = all.GetGameObject(b2);

        if (b1GO == null || b2GO == null)
        {
            m_log.Warning("Aborting swap because could not found one of the builds game object: b1: {0}, b2: {1}", b1GO, b2GO);
        }

        m_log.Debug("Swapping position between {0} and {1}...", b1GO.name, b2GO.name);

        var b1Position = b1GO.transform.position;

        AnimateSwap(b1GO, b2GO.transform.position);
        AnimateSwap(b2GO, b1Position);
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
            m_buildsGO.GetAll().UnfreezeAll();

            m_serverService.GetState().IsSorting = false;
            UpdateStatusBar("Sorting finished.", 2f);
        }
    }

	private void UpdateStatusBar (string text, float secondsTimeout = 0)
	{
		m_log.Warning (text);
		StatusBarController.SetStatusText (text, secondsTimeout);		
	}

	#endregion
}