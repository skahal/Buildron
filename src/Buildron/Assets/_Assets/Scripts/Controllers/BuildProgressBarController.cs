using UnityEngine;
using System.Collections;

public class BuildProgressBarController : MonoBehaviour {

	public void UpdateValue(float value)
	{
		GetComponent<Renderer>().material.SetFloat ("_Cutoff", 1f - value);
	}
}
