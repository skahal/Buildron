#region Usings
using UnityEngine;
using System.Collections;
#endregion

[AddComponentMenu("Buildron/Environment/EnvironmentController")]
public class EnvironmentController : MonoBehaviour {
	
	#region Editor properties
	public skydomeScript2 SkyController;
	public float ForceHour = -1;
	#endregion
	
	#region Methods
	private void Start ()
	{
		StartCoroutine(UpdateTime());
	}

	private IEnumerator UpdateTime ()
	{
		while (true) {
			
			if (ForceHour > -1) {
				SkyController.TIME = ForceHour;
			} else {
				var now = System.DateTime.Now;
				SkyController.TIME = now.Hour + (now.Minute / 60f);
				SkyController.JULIANDATE = System.Convert.ToSingle(now.ToOADate() + 2415018.5f);
			}
			
			yield return new WaitForSeconds(60);
		}
	}
	#endregion
}