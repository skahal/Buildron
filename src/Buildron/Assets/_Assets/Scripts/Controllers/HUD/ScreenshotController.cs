#region Usings
using UnityEngine;
using System.Collections;
#endregion

/// <summary>
/// Screenshot controller.
/// </summary>
public class ScreenshotController : MonoBehaviour
{
	private void Start ()
	{
		Messenger.Register (gameObject, "OnScreenshotRequested");
	}
	
	private void OnScreenshotRequested ()
	{
		SHScreenshotHelper.TakeScreenshot ((Texture2D texture) => 
		{
			Messenger.Send("OnScreenshotTaken", texture);	
		});
	}
}