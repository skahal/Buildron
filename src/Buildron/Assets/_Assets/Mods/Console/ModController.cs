using UnityEngine;
using System.Collections;
using Buildron.Domain.Mods;
using System.Collections.Generic;

public class ModController : MonoBehaviour {

	#region Fields
	private Rect m_windowRect = new Rect(10, 10, 400, 300);
	private List<string> m_msgs = new List<string>();
	#endregion

	public void AddMessage (string message, params object[] args)
	{
		m_msgs.Insert(0, message.With (args));

		if (m_msgs.Count > 10) {
			m_msgs.RemoveAt(10);
		}
	}

	void OnGUI()
	{
		GUILayout.Window(1, m_windowRect, HandleWindowFunction, "Console mod",  GUILayout.MinWidth(100), GUILayout.MinHeight(100));
	}

	void HandleWindowFunction (int id)
	{
		GUILayout.BeginVertical ();

		foreach (var msg in m_msgs) {
			GUILayout.Label (msg);
		}

		GUILayout.EndVertical ();
	}
}
