using System;
using System.Linq;
using UnityEngine;
using Buildron.Domain.Mods;
using Buildron.Controllers.Builds;

public static class BuildGameObjectsProxyExtensions
{
	public static bool AreVisiblesFromLeft (this IBuildController[] builds)
	{
		return builds.All (b => b.LeftEdge.IsVisibleFrom (Camera.main));
	}

	public static bool AreVisiblesFromRight (this IBuildController[] builds)
	{
		return builds.All (b => b.RightEdge.IsVisibleFrom (Camera.main));
	}

	public static bool AreVisiblesFromHorizontal (this IBuildController[] builds)
	{
		var camera = Camera.main;

		return builds.All (b => b.LeftEdge.IsVisibleFrom (camera) && b.RightEdge.IsVisibleFrom (camera));
	}

	public static bool AreVisiblesFromTop (this IBuildController[] builds)
	{
		return builds.All (b => b.TopEdge.IsVisibleFrom (Camera.main));
	}

	public static bool AreVisiblesFromBottom (this IBuildController[] builds)
	{
		return builds.All (b => b.BottomEdge.IsVisibleFrom (Camera.main));
	}

	public static bool AreVisiblesFromVertical (this IBuildController[] builds)
	{
		var camera = Camera.main;

		return builds.All (b => b.TopEdge.IsVisibleFrom (camera) && b.BottomEdge.IsVisibleFrom (camera));
	}

	public static bool HasNotVisiblesFromTop (this IBuildController[] builds)
	{
		var camera = Camera.main;

		return builds.All (b => 
			!b.TopEdge.IsVisibleFrom (camera)
		&& (b.LeftEdge.IsVisibleFrom (camera)
		|| b.RightEdge.IsVisibleFrom (camera)
		|| b.BottomEdge.IsVisibleFrom (camera)));
	}

	public static bool HasNotVisiblesFromSides (this IBuildController[] builds)
	{
		var camera = Camera.main;

		return builds.All (b => 
			b.TopEdge.IsVisibleFrom (camera)
		&& (!b.LeftEdge.IsVisibleFrom (camera)
		|| !b.RightEdge.IsVisibleFrom (camera)
		|| !b.BottomEdge.IsVisibleFrom (camera)));
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

