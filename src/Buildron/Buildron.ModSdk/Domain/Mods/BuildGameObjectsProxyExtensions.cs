using System;
using System.Linq;
using UnityEngine;
using Buildron.Domain.Mods;
using Buildron.Controllers.Builds;

public static class BuildGameObjectsProxyExtensions
{
	public static bool AreVisiblesFromLeft (this IBuildController[] builds)
	{
		var camera = Camera.main;

		return builds.All (b => b.LeftCollider.IsVisibleFrom (camera));
	}

	public static int CountVisiblesFromLeft (this IBuildController[] builds)
	{
		var camera = Camera.main;

		return builds.Count (b => b.LeftCollider.IsVisibleFrom (camera));
	}

	public static bool AreVisiblesFromRight (this IBuildController[] builds)
	{
		var camera = Camera.main;

		return builds.All (b => b.RightCollider.IsVisibleFrom (Camera.main));
	}

	public static int CountVisiblesFromRight (this IBuildController[] builds)
	{
		var camera = Camera.main;

		return builds.Count (b => b.RightCollider.IsVisibleFrom (camera));
	}

	public static bool AreVisiblesFromHorizontal (this IBuildController[] builds)
	{
		var camera = Camera.main;

		return builds.All (b => b.LeftCollider.IsVisibleFrom (camera) && b.RightCollider.IsVisibleFrom (camera));
	}

	public static bool AreVisiblesFromTop (this IBuildController[] builds)
	{
		var camera = Camera.main;

		return builds.All (b => b.TopCollider.IsVisibleFrom (camera));
	}

	public static int CountVisiblesFromTop (this IBuildController[] builds)
	{
		var camera = Camera.main;

		return builds.Count (b => b.TopCollider.IsVisibleFrom (camera));
	}

	public static bool AreVisiblesFromBottom (this IBuildController[] builds)
	{
		var camera = Camera.main;

		return builds.All (b => b.BottomCollider.IsVisibleFrom (camera));
	}

	public static int CountVisiblesFromBottom (this IBuildController[] builds)
	{
		var camera = Camera.main;

		return builds.Count (b => b.BottomCollider.IsVisibleFrom (camera));
	}

	public static bool AreVisiblesFromVertical (this IBuildController[] builds)
	{
		var camera = Camera.main;

		return builds.All (b => b.TopCollider.IsVisibleFrom (camera) && b.BottomCollider.IsVisibleFrom (camera));
	}

	public static IBuildController[] Visible (this IBuildController[] builds)
	{
		var camera = Camera.main;

		return builds.Where (b => b.CenterCollider != null && b.CenterCollider.IsVisibleFrom(camera)).ToArray ();
	}

	public static IBuildController[] Stopped (this IBuildController[] builds)
	{
		return builds.Where (b => b.Rigidbody != null && Mathf.Abs (b.Rigidbody.velocity.y) <= 0.1f).ToArray ();
	}
}

