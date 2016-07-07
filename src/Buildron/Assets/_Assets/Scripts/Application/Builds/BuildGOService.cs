using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buildron.Domain;
using UnityEngine;
using Zenject;
using Skahal.Logging;
using Buildron.Domain.Builds;

namespace Buildron.Application
{
    /// <summary>
    /// Application service to handle with build game objects.
    /// </summary>
    public class BuildGOService : GOServiceBase<IBuild, BuildController>
    {
		#region Fields
		private ISHLogStrategy m_log;
		#endregion

        #region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Application.BuildGOService"/> class.
		/// </summary>
		/// <param name="factory">Factory.</param>
		/// <param name="log">Log.</param>
		public BuildGOService(BuildController.Factory factory, ISHLogStrategy log) 
            : base (factory)
        {
			m_log = log;
        }
        #endregion

        #region Methods
        public IList<GameObject> GetVisibles()
        {
            return GetVisiblesQuery().ToList();

        }

        public int CountVisibles()
        {
            return GetVisiblesQuery().Count();
        }

        public bool HasNotVisiblesFromTop()
        {
            var builds = GameObject.FindGameObjectsWithTag("Build").Select(b => b.GetComponent<BuildController>());

            return builds.Any(
                    b => b != null
                && b.HasReachGround
                && b.IsVisible
                && !b.IsHistoryBuild
                && Mathf.Abs(b.Rigidbody.velocity.y) <= b.VisibleMaxYVelocity
                && !b.TopEdge.IsVisibleFrom(Camera.main)
                && (b.LeftEdge.IsVisibleFrom(Camera.main)
                || b.RightEdge.IsVisibleFrom(Camera.main)
                || b.BottomEdge.IsVisibleFrom(Camera.main)));
        }

        public bool HasNotVisiblesFromSides()
        {
            var builds = GameObject.FindGameObjectsWithTag("Build").Select(b => b.GetComponent<BuildController>());

            return builds.Any(
                    b => b != null
                && b.HasReachGround
                && b.IsVisible
                && !b.IsHistoryBuild
				&& Mathf.Abs(b.Rigidbody.velocity.y) <= b.VisibleMaxYVelocity
                && b.TopEdge.IsVisibleFrom(Camera.main)
                && (!b.LeftEdge.IsVisibleFrom(Camera.main)
                || !b.RightEdge.IsVisibleFrom(Camera.main)
                || !b.BottomEdge.IsVisibleFrom(Camera.main)));
        }

        public IList<GameObject> GetVisiblesOrderByPosition()
        {
            var buildsGO = GameObject.FindGameObjectsWithTag("Build");
            var buildsControllers = buildsGO.Select(b => b.GetComponent<BuildController>());

            var query = from c in buildsControllers
                        where c.Body != null
                            && c.IsVisible
                            && !c.IsHistoryBuild
                        orderby
							Mathf.CeilToInt(c.Body.transform.position.x) ascending,
							Mathf.CeilToInt(c.Body.transform.position.y) descending,
                            c.gameObject.name ascending
                        select c.gameObject;


            return query.ToList();
        }

        /// <summary>
        /// Freezes all builds game objects
        /// </summary>
        public void FreezeAll()
        {
            foreach (var go in GetVisiblesOrderByPosition())
            {
                var rb = go.GetComponent<Rigidbody>();
                rb.isKinematic = true;

                // Allows build game object be moved on X and Y (sorting swap effect).
                rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
        }

        /// <summary>
        /// Unfreezes all builds game objects
        /// </summary>
        public void UnfreezeAll()
        {
            foreach (var go in GetVisiblesOrderByPosition())
            {
                var rb = go.GetComponent<Rigidbody>();
                rb.isKinematic = false;

                // Allows build game object be moved only Y (explosion effects).
                rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
        }

        /// <summary>
        /// Verify if all builds physics are sleeping.
        /// </summary>
        /// <remarks>
        /// Works well with the value of "Sleep Threshold" in the "Project settings\Physics" as "0.05".
        /// </remarks>
        /// <returns>True if all builds are sleeping.</returns>
        public bool AreAllSleeping()
        {
			return GetVisibles().All(b => b.GetComponent<BuildController>().Rigidbody.IsSleeping());
        }

        public bool HasAllReachGround()
        {
            return GetVisibles().All(b => b.GetComponent<BuildController>().HasReachGround);
        }

		/// <summary>
		/// Issue #9: https://github.com/skahal/Buildron/issues/9
		/// If a build was removed, maybe there are space between builds totems and some can be sleeping.
		/// Wake everyone!
		/// </summary>
		public void WakeUpSleepingBuilds()
		{      
			foreach (var visibleBuild in GetVisibles())
			{
				var rb = visibleBuild.GetComponent<BuildController>().Rigidbody;

				if (rb.IsSleeping())
				{
					m_log.Debug("Wake up build game object: {0}", visibleBuild.name);
					rb.WakeUp();
				}
			}
		}

		protected override string GetName (IBuild model)
		{
			return model.Id;
		}

		public override GameObject CreateGameObject (IBuild model)
		{
			var go = GetGameObject(model);

			if (go == null)
			{
				var build = Factory.Create();
				build.Model = model;
				go = build.gameObject;
				go.name = GetName(model);
			}

			return go;
		}

		private IEnumerable<GameObject> GetVisiblesQuery()
		{
			var builds = GameObject.FindGameObjectsWithTag("Build").Select(b => b.GetComponent<BuildController>());

			return builds.Where(
				b => b != null
				&& b.Body != null
				&& b.IsVisible
				&& !b.IsHistoryBuild).Select(b => b.gameObject);
		}
		#endregion
    }
}
