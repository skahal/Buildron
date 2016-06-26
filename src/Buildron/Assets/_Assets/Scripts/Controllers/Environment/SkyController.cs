#region Usings
using UnityEngine;
using System.Collections;
#endregion

/// <summary>
/// Sky controller.
/// </summary>
public class SkyController : MonoBehaviour {
	
	#region Editor properties
	public GameObject Sky;
	public Vector3 ScaleModifierByBuild = new Vector3(2, 2, 2);
	#endregion
	
	#region Life cycle
	private	void Start ()
	{
		Messenger.Register (gameObject, "OnBuildReachGround");	
	
	}
	
	private void OnBuildReachGround ()
	{
		Sky.transform.localScale += ScaleModifierByBuild;
	}
	#endregion
}