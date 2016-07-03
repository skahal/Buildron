#region Usings
using UnityEngine;
using UnityEngine.UI;
using Skahal.Threading;


#endregion

/// <summary>
/// Status bar controller.
/// </summary>
public class StatusBarController : MonoBehaviour {
	
	#region Fields
	private static StatusBarController s_instance;
	#endregion
	
	#region Editor properties
	public Text[] StatusLabels;
	#endregion
	
	#region Constructors
	public StatusBarController ()
	{
		s_instance = this;
	}
	#endregion
	
	#region Methods
	public static void SetStatusText (string text, float secondsTimeout = 0)
	{
		for (int i = 0; i < s_instance.StatusLabels.Length; i++) {
			
			var label = s_instance.StatusLabels[i];
			label.text = text;
		
			if (secondsTimeout > 0) {
				SHCoroutine.Start (secondsTimeout, () => {
					if (label.text.Equals (text, System.StringComparison.OrdinalIgnoreCase)) {
						label.text = string.Empty;
					}
				});
			}
		}
	}
	
	public static void ClearStatusText()
	{
		SetStatusText(string.Empty);
	}
	#endregion
}

