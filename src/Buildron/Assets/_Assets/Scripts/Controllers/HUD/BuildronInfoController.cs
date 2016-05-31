#region Usings
using UnityEngine;
using System.Collections;
#endregion

/// <summary>
/// Buildron info controller.
/// </summary>
[AddComponentMenu("Buildron/HUD/BuildronInfoController")]
public class BuildronInfoController : MonoBehaviour 
{
	/// <summary>
	/// Goes to site Buildron's site.
	/// </summary>
	public void GoToSite()
	{
		Application.OpenURL("http://skahal.com/buildron?from=Buildron");	
	}
}