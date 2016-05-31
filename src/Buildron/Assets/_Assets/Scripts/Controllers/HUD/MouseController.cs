using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {

	#region Fields
	private Vector3 m_lastMousePosition;
	#endregion

	#region Methods
	void Start () {
		StartCoroutine(ShowHideMouse());
	}

	void Update () {
		if(Input.mousePosition != m_lastMousePosition)
		{
			Cursor.visible = true;
		}

		m_lastMousePosition = Input.mousePosition;
	}

	IEnumerator ShowHideMouse()
	{
		while(true)
		{
			if(Input.mousePosition == m_lastMousePosition)
			{
				Cursor.visible = false;
			}		

			yield return new WaitForSeconds(15);
		}
	}
	#endregion
}