using System;
using UnityEngine;

namespace Buildron.Controllers.Builds
{
	public interface IBuildController
	{
		bool IsVisible { get; }
		bool HasReachGround { get; }
		GameObject gameObject { get; }
		Rigidbody Rigidbody { get; }

		Collider CenterCollider { get; }
		Collider TopEdge { get; }
		Collider LeftEdge { get; }
		Collider RightEdge { get; }
		Collider BottomEdge { get; }
	}
}