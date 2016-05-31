#region Usings
using UnityEngine;
using System.Collections;
#endregion

public class AboutPanelController : MonoBehaviour
{
	#region Editor properties
	public GameObject ConfigPanel;
	#endregion

	#region Methods
	public void ShowAbout ()
	{
		ConfigPanel.SetActive (false);
		gameObject.SetActive (true);
	}
	
	public void Back ()
	{
		gameObject.SetActive (false);
		ConfigPanel.SetActive (true);
	}
	#endregion
}