using System.Linq;
using Buildron.Domain.Mods;
using UnityEngine;

/// <summary>
/// Game object controller extension methods.
/// </summary>
public static class GameObjectControllerExtensions
{
	/// <summary>
	/// Verify whether all game object controllers are visible from main camera left side.
	/// </summary>
	/// <returns>The whether all are visibles from left.</returns>
	/// <param name="controllers">The game object controllers.</param>
	public static bool AreVisiblesFromLeft (this IGameObjectController[] controllers)
	{
		var camera = Camera.main;

		return controllers.All (b => b.LeftCollider.IsVisibleFrom (camera));
	}

	/// <summary>
	/// Count game object controllers that are visible from main camera left side.
	/// </summary>
	/// <returns>The count of visibles from left.</returns>
	/// <param name="controllers">The game object controllers.</param>
	public static int CountVisiblesFromLeft (this IGameObjectController[] controllers)
	{
		var camera = Camera.main;

		return controllers.Count (b => b.LeftCollider.IsVisibleFrom (camera));
	}

	/// <summary>
	/// Verify whether all game object controllers are visible from main camera right side.
	/// </summary>
	/// <returns>The whether all are visibles from right.</returns>
	/// <param name="controllers">The game object controllers.</param>
	public static bool AreVisiblesFromRight (this IGameObjectController[] controllers)
	{
		var camera = Camera.main;

		return controllers.All (b => b.RightCollider.IsVisibleFrom (Camera.main));
	}

	/// <summary>
	/// Count game object controllers that are visible from main camera right side.
	/// </summary>
	/// <returns>The count of visibles from right.</returns>
	/// <param name="controllers">The game object controllers.</param>
	public static int CountVisiblesFromRight (this IGameObjectController[] controllers)
	{
		var camera = Camera.main;

		return controllers.Count (b => b.RightCollider.IsVisibleFrom (camera));
	}
	/// <summary>
	/// Verify whether all game object controllers are visible from main camera horizontal sides.
	/// </summary>
	/// <returns>The whether all are visibles from horizontal sides.</returns>
	/// <param name="controllers">The game object controllers.</param>
	public static bool AreVisiblesFromHorizontal (this IGameObjectController[] controllers)
	{
		var camera = Camera.main;

		return controllers.All (b => b.LeftCollider.IsVisibleFrom (camera) && b.RightCollider.IsVisibleFrom (camera));
	}
	/// <summary>
	/// Verify whether all game object controllers are visible from main camera top side.
	/// </summary>
	/// <returns>The whether all are visibles from top.</returns>
	/// <param name="controllers">The game object controllers.</param>
	public static bool AreVisiblesFromTop (this IGameObjectController[] controllers)
	{
		var camera = Camera.main;

		return controllers.All (b => b.TopCollider.IsVisibleFrom (camera));
	}
	/// <summary>
	/// Count game object controllers that are visible from main camera top side.
	/// </summary>
	/// <returns>The count of visibles from top.</returns>
	/// <param name="controllers">The game object controllers.</param>
	public static int CountVisiblesFromTop (this IGameObjectController[] controllers)
	{
		var camera = Camera.main;

		return controllers.Count (b => b.TopCollider.IsVisibleFrom (camera));
	}
	/// <summary>
	/// Verify whether all game object controllers are visible from main camera bottom side.
	/// </summary>
	/// <returns>The whether all are visibles from bottom.</returns>
	/// <param name="controllers">The game object controllers.</param>
	public static bool AreVisiblesFromBottom (this IGameObjectController[] controllers)
	{
		var camera = Camera.main;

		return controllers.All (b => b.BottomCollider.IsVisibleFrom (camera));
	}
	/// <summary>
	/// Count game object controllers that are visible from main camera bottom side.
	/// </summary>
	/// <returns>The count of visibles from bottom.</returns>
	/// <param name="controllers">The game object controllers.</param>
	public static int CountVisiblesFromBottom (this IGameObjectController[] controllers)
	{
		var camera = Camera.main;

		return controllers.Count (b => b.BottomCollider.IsVisibleFrom (camera));
	}

	/// <summary>
	/// Verify whether all game object controllers are visible from main camera vertical sides.
	/// </summary>
	/// <returns>The whether all are visibles from vertical.</returns>
	/// <param name="controllers">The game object controllers.</param>
	public static bool AreVisiblesFromVertical (this IGameObjectController[] controllers)
	{
		var camera = Camera.main;

		return controllers.All (b => b.TopCollider.IsVisibleFrom (camera) && b.BottomCollider.IsVisibleFrom (camera));
	}

	/// <summary>
	/// Get game object controllers that are visible from main camera.
	/// </summary>
	/// <param name="controllers">The controllers.</param>
	public static IGameObjectController[] Visible (this IGameObjectController[] controllers)
	{
		var camera = Camera.main;

		return controllers.Where (b => 
			b.CenterCollider != null 
			&& b.CenterCollider.IsVisibleFrom(camera)).ToArray ();
	}

	/// <summary>
	/// Get game object controllers that are stopped.
	/// </summary>
	/// <param name="controllers">The controllers.</param>
	public static IGameObjectController[] Stopped (this IGameObjectController[] controllers)
	{		
		return controllers.Where (
			b => b.Rigidbody != null 
			&& Mathf.Abs (b.Rigidbody.velocity.x) <= 0.1f
			&& Mathf.Abs(b.Rigidbody.velocity.y) <= 0.1f
			&& Mathf.Abs(b.Rigidbody.velocity.z) <= 0.1f
			).ToArray ();
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
	/// <param name="controllers">The controllers.</param>
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

	/// <summary>
	/// Gets the visibles game object controllers ordered by position.
	/// </summary>
	/// <returns>The visibles order by position.</returns>
	/// <param name="controllers">Controllers.</param>
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