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
}

