using System;
using UnityEngine;

namespace Buildron.Controllers.Builds
{
	public interface IBuildController
	{
		GameObject gameObject { get; }
		Rigidbody Rigidbody { get; }

		Collider CenterCollider { get; }
		Collider TopCollider { get; }
		Collider LeftCollider { get; }
		Collider RightCollider { get; }
		Collider BottomCollider { get; }
	}
}