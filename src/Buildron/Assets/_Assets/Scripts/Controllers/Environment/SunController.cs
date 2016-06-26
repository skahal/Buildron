using UnityEngine;
using System.Collections;

[AddComponentMenu("Buildron/Environment/SunController")]
public class SunController : MonoBehaviour
{
	#region Editor properties
	public Vector3 SunrisePosition = new Vector3(-100, 35, 13);
	public Vector3 SunsetPosition = new Vector3(100, 35, 13);
	public float UpdateInterval = 5;
	#endregion
	
	private void Awake ()
	{
		transform.position = SunrisePosition;
		StartCoroutine(UpdateSunPosition());	
	}
	
	private IEnumerator UpdateSunPosition ()
	{
		while (true) {
			
			transform.position = Vector3.Lerp(transform.position, SunsetPosition, Time.deltaTime * 0.01f);
			yield return new WaitForSeconds(UpdateInterval);
		}
	}
}

