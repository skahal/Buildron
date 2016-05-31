#region Usings
using UnityEngine;
using System.Collections;
using Buildron.Domain;
using Skahal.Common;
#endregion

/// <summary>
/// Kick easter egg controller.
/// </summary>
[AddComponentMenu("Buildron/EasterEggs/KickEasterEggController")]
public class KickEasterEggController : EasterEggControllerBase 
{	
	#region Life cycle
	void Start ()
	{
		EasterEggsNames.Add ("Kick");
		EasterEggsNames.Add ("KickAll");
	}
	 	
	private void OnKick ()
	{		
		var avatar = BuildUserController.GetGameObject (RemoteControlService.GetConnectedRemoteControl ().UserName);
		Kick (avatar);
	}

	private void OnKickAll ()
	{
		var avatars = BuildUserController.GetAllGameObjects ();	
		StartCoroutine (KickAll (avatars));
	}
	
	private void Kick (GameObject avatar)
	{
		// Check for null because the connected remote control could be from a user without a current build.
		if (avatar != null) {
			avatar.GetComponent<BuildUserAnimationController> ().PlayKick ();
			BallController.CreateGameObject(avatar.transform.position);
		}
	}
	
	private IEnumerator KickAll (GameObject[] avatars)
	{
		foreach (var avatar in avatars) {
			Kick (avatar);
			yield return new WaitForSeconds(0.2f);
		}
	}
	#endregion
}