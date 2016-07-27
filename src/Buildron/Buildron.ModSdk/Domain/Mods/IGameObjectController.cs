using System;
using Skahal.Common;
using UnityEngine;

namespace Buildron.Domain.Mods
{
	public interface IGameObjectController : IEventSubscriber
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

