using UnityEngine;
using System.Collections;

public class BuildProgressBarController : MonoBehaviour {

	private Renderer m_renderer;

	private void Awake()
	{
		m_renderer = GetComponent<Renderer> ();
	}

	public void UpdateValue(float value)
	{
		m_renderer.material.SetFloat ("_Cutoff", 1f - value);
	}

	public void Hide()
	{
		gameObject.SetActive (false);
		m_renderer.enabled = false;
		enabled = false;
	}

	public void Show()
	{
		gameObject.SetActive (true);
		m_renderer.enabled = true;
		enabled = true;
	}
}
