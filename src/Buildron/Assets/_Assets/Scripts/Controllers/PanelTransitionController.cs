using System;
using UnityEngine;

public class PanelTransitionController : MonoBehaviour
{
	public PanelTransitionController ()
	{
		Instance = this;
	}

	public static PanelTransitionController Instance { get; private set; }
	public GameObject AboutPanel;
	public GameObject ConfigPanel;
	public GameObject MainPanel;

	private void Start ()
	{
		ShowConfigPanel ();	
	}

	public void ShowAboutPanel()
	{
		HideAll ();
		AboutPanel.SetActive (true);
	}

	public void ShowConfigPanel()
	{
		HideAll ();
		ConfigPanel.SetActive (true);
	}

	public void ShowMainPanel()
	{
		HideAll ();
		MainPanel.SetActive (true);
	}

	private void HideAll()
	{
		AboutPanel.SetActive (false);
		ConfigPanel.SetActive (false);
		MainPanel.SetActive (false);
	}
}