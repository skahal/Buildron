using System;
using System.Linq;
using UnityEngine;
using Buildron.Domain.Mods;
using Buildron.Controllers.Builds;

public static class BuildGameObjectsProxyExtensions
{
	public static bool AreVisiblesFromLeft (this IBuildController[] builds)
	{
		return builds.All (b => b.LeftCollider.IsVisibleFrom (Camera.main));
	}

	public static bool AreVisiblesFromRight (this IBuildController[] builds)
	{
		return builds.All (b => b.RightCollider.IsVisibleFrom (Camera.main));
	}

	public static bool AreVisiblesFromHorizontal (this IBuildController[] builds)
	{
		var camera = Camera.main;

		return builds.All (b => b.LeftCollider.IsVisibleFrom (camera) && b.RightCollider.IsVisibleFrom (camera));
	}

	public static bool AreVisiblesFromTop (this IBuildController[] builds)
	{
		return builds.All (b => b.TopCollider.IsVisibleFrom (Camera.main));
	}

	public static bool AreVisiblesFromBottom (this IBuildController[] builds)
	{
		return builds.All (b => b.BottomCollider.IsVisibleFrom (Camera.main));
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

