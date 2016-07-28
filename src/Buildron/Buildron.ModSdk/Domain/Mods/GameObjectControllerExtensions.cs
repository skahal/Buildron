using System;
using System.Linq;
using UnityEngine;
using Buildron.Domain.Mods;

public static class GameObjectControllerExtensions
{
	public static bool AreVisiblesFromLeft (this IGameObjectController[] builds)
	{
		var camera = Camera.main;

		return builds.All (b => b.LeftCollider.IsVisibleFrom (camera));
	}

	public static int CountVisiblesFromLeft (this IGameObjectController[] builds)
	{
		var camera = Camera.main;

		return builds.Count (b => b.LeftCollider.IsVisibleFrom (camera));
	}

	public static bool AreVisiblesFromRight (this IGameObjectController[] builds)
	{
		var camera = Camera.main;

		return builds.All (b => b.RightCollider.IsVisibleFrom (Camera.main));
	}

	public static int CountVisiblesFromRight (this IGameObjectController[] builds)
	{
		var camera = Camera.main;

		return builds.Count (b => b.RightCollider.IsVisibleFrom (camera));
	}

	public static bool AreVisiblesFromHorizontal (this IGameObjectController[] builds)
	{
		var camera = Camera.main;

		return builds.All (b => b.LeftCollider.IsVisibleFrom (camera) && b.RightCollider.IsVisibleFrom (camera));
	}

	public static bool AreVisiblesFromTop (this IGameObjectController[] builds)
	{
		var camera = Camera.main;

		return builds.All (b => b.TopCollider.IsVisibleFrom (camera));
	}

	public static int CountVisiblesFromTop (this IGameObjectController[] builds)
	{
		var camera = Camera.main;

		return builds.Count (b => b.TopCollider.IsVisibleFrom (camera));
	}

	public static bool AreVisiblesFromBottom (this IGameObjectController[] builds)
	{
		var camera = Camera.main;

		return builds.All (b => b.BottomCollider.IsVisibleFrom (camera));
	}

	public static int CountVisiblesFromBottom (this IGameObjectController[] builds)
	{
		var camera = Camera.main;

		return builds.Count (b => b.BottomCollider.IsVisibleFrom (camera));
	}

	public static bool AreVisiblesFromVertical (this IGameObjectController[] builds)
	{
		var camera = Camera.main;

		return builds.All (b => b.TopCollider.IsVisibleFrom (camera) && b.BottomCollider.IsVisibleFrom (camera));
	}

	public static IGameObjectController[] Visible (this IGameObjectController[] builds)
	{
		var camera = Camera.main;

		return builds.Where (b => b.CenterCollider != null && b.CenterCollider.IsVisibleFrom(camera)).ToArray ();
	}

	public static IGameObjectController[] Stopped (this IGameObjectController[] builds)
	{
		return builds.Where (b => b.Rigidbody != null && Mathf.Abs (b.Rigidbody.velocity.y) <= 0.1f).ToArray ();
	}

	/// <summary>
	/// Verify if all builds physics are sleeping.
	/// </summary>
	/// <remarks>
	/// Works well with the value of "Sleep Threshold" in the "Project settings\Physics" as "0.05".
	/// </remarks>
	/// <returns>True if all builds are sleeping.</returns>
	public static bool AreAllSleeping(this IGameObjectController[] builds)
	{
		return builds.All(b => b.Rigidbody.IsSleeping());
	}

	/// <summary>
	/// Issue #9: https://github.com/skahal/Buildron/issues/9
	/// If a build was removed, maybe there are space between builds totems and some can be sleeping.
	/// Wake everyone!
	/// </summary>
	public static void WakeUpSleepingBuilds(this IGameObjectController[] builds)
	{      
		foreach (var build in builds)
		{
			var rb = build.Rigidbody;

			if (rb.IsSleeping())
			{
				rb.WakeUp();
			}
		}
	}

	/// <summary>
	/// Freezes all builds game objects
	/// </summary>
	public static void FreezeAll(this IGameObjectController[] builds)
	{
		foreach (var build in builds)
		{
			var rb = build.Rigidbody;
			rb.isKinematic = true;

			// Allows build game object be moved on X and Y (sorting swap effect).
			rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
		}
	}

	/// <summary>
	/// Unfreezes all builds game objects
	/// </summary>
	public static void UnfreezeAll(this IGameObjectController[] builds)
	{
		foreach (var build in builds)
		{
			var rb = build.Rigidbody;
			rb.isKinematic = false;

			// Allows build game object be moved only Y (explosion effects).
			rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
		}
	}
}
