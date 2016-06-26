#region Usings
using UnityEngine;
using System.Collections;
using Skahal.Logging;
#endregion

/// <summary>
/// Ball controller.
/// </summary>
[AddComponentMenu("Buildron/Controllers/Misc/BallController")]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class BallController : MonoBehaviour {
	
	#region Editor properties
	public Vector2 XForceRandomRange = new Vector2(-100, 100);	
	public float YForce = 1000;
	public float ZForce = 1000;
	#endregion
	
	#region Methods
	private void Initialize (Vector3 position)
	{
		transform.position = position + Vector3.up;
		GetComponent<Rigidbody>().AddForce (new Vector3 (Random.Range (XForceRandomRange.x, XForceRandomRange.y), YForce, ZForce));
	}
	
	public static GameObject CreateGameObject (Vector3 position)
	{
		SHLog.Debug ("Requesting ball from poll.");
		var ball = SHPoolsManager.GetGameObject ("Ball");
		ball.SendMessage("Initialize", position);
		SHLog.Debug ("Ball received from poll.");
		
		return ball;
	}
	#endregion
}