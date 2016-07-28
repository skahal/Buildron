using System;
using System.Linq;
using UnityEngine;
using Buildron.Domain.Mods;
using System.Collections.Generic;

public static class GameObjectControllerExtensions
{
	public static bool AreVisiblesFromLeft (this IGameObjectController[] controllers)
	{
		var camera = Camera.main;

		return controllers.All (b => b.LeftCollider.IsVisibleFrom (camera));
	}

	public static int CountVisiblesFromLeft (this IGameObjectController[] controllers)
	{
		var camera = Camera.main;

		return controllers.Count (b => b.LeftCollider.IsVisibleFrom (camera));
	}

	public static bool AreVisiblesFromRight (this IGameObjectController[] controllers)
	{
		var camera = Camera.main;

		return controllers.All (b => b.RightCollider.IsVisibleFrom (Camera.main));
	}

	public static int CountVisiblesFromRight (this IGameObjectController[] controllers)
	{
		var camera = Camera.main;

		return controllers.Count (b => b.RightCollider.IsVisibleFrom (camera));
	}

	public static bool AreVisiblesFromHorizontal (this IGameObjectController[] controllers)
	{
		var camera = Camera.main;

		return controllers.All (b => b.LeftCollider.IsVisibleFrom (camera) && b.RightCollider.IsVisibleFrom (camera));
	}

	public static bool AreVisiblesFromTop (this IGameObjectController[] controllers)
	{
		var camera = Camera.main;

		return controllers.All (b => b.TopCollider.IsVisibleFrom (camera));
	}

	public static int CountVisiblesFromTop (this IGameObjectController[] controllers)
	{
		var camera = Camera.main;

		return controllers.Count (b => b.TopCollider.IsVisibleFrom (camera));
	}

	public static bool AreVisiblesFromBottom (this IGameObjectController[] controllers)
	{
		var camera = Camera.main;

		return controllers.All (b => b.BottomCollider.IsVisibleFrom (camera));
	}

	public static int CountVisiblesFromBottom (this IGameObjectController[] controllers)
	{
		var camera = Camera.main;

		return controllers.Count (b => b.BottomCollider.IsVisibleFrom (camera));
	}

	public static bool AreVisiblesFromVertical (this IGameObjectController[] controllers)
	{
		var camera = Camera.main;

		return controllers.All (b => b.TopCollider.IsVisibleFrom (camera) && b.BottomCollider.IsVisibleFrom (camera));
	}

	public static IGameObjectController[] Visible (this IGameObjectController[] controllers)
	{
		var camera = Camera.main;

		return controllers.Where (b => b.CenterCollider != null && b.CenterCollider.IsVisibleFrom(camera)).ToArray ();
	}

	public static IGameObjectController[] Stopped (this IGameObjectController[] controllers)
	{
		return controllers.Where (b => b.Rigidbody != null && Mathf.Abs (b.Rigidbody.velocity.y) <= 0.1f).ToArray ();
	}

	/// <summary>
	/// Verify if all controllers physics are sleeping.
	/// </summary>
	/// <remarks>
	/// Works well with the value of "Sleep Threshold" in the "Project settings\Physics" as "0.05".
	/// </remarks>
	/// <returns>True if all controllers are sleeping.</returns>
	public static bool AreAllSleeping(this IGameObjectController[] controllers)
	{
		return controllers.All(b => b.Rigidbody.IsSleeping());
	}

	/// <summary>
	/// Issue #9: https://github.com/skahal/Buildron/issues/9
	/// If a build was removed, maybe there are space between builds totems and some can be sleeping.
	/// Wake everyone!
	/// </summary>
	public static void WakeUpSleepingBuilds(this IGameObjectController[] controllers)
	{      
		foreach (var build in controllers)
		{
			var rb = build.Rigidbody;

			if (rb.IsSleeping())
			{
				rb.WakeUp();
			}
		}
	}

	/// <summary>
	/// Freezes all game objects.
	/// </summary>
	public static void FreezeAll(this IGameObjectController[] controllers)
	{
		foreach (var controller in controllers)
		{
			var rb = controller.Rigidbody;
			rb.isKinematic = true;

			// Allows build game object be moved on X and Y (sorting swap effect).
			rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
		}
	}

	/// <summary>
	/// Unfreezes all game objects.
	/// </summary>
	public static void UnfreezeAll(this IGameObjectController[] controllers)
	{
		foreach (var controller in controllers)
		{
			var rb = controller.Rigidbody;
			rb.isKinematic = false;

			// Allows build game object be moved only Y (explosion effects).
			rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
		}
	}

    public static IGameObjectController[] GetVisiblesOrderByPosition(this IGameObjectController[] controllers)
    {
        var query = from c in controllers
                    orderby
                        Mathf.CeilToInt(c.gameObject.transform.position.x) ascending,
                        Mathf.CeilToInt(c.gameObject.transform.position.y) descending,
                        c.gameObject.name ascending
                    select c;

        return query.ToArray();
    }
}
