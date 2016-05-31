#region Usings
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Skahal.Tweening;


#endregion

[AddComponentMenu("Buildron/Controllers/BuildFocusedPanelController")]
public class BuildFocusedPanelController : MonoBehaviour 
{
	#region Fields
	private BuildController m_buildController;
	private bool m_isVisible;
	private Text m_text;
	#endregion
	
	#region Editor properties
	public float YHeight = 1f;
	#endregion
	
	#region Life cycle
	private void Start ()
	{
		m_buildController = transform.parent.GetComponent<BuildController> ();
		
		if (m_buildController.IsHistoryBuild) {
			GameObject.Destroy(gameObject);
		} else {	
			m_text = GetComponentInChildren<Text> ();
			Messenger.Register (gameObject, "OnBuildHidden", "OnBuildVisible");
			m_isVisible = true;
			Hide ();
		}
	}
	
	private void OnBuildHidden ()
	{
		if (BuildController.VisiblesCount == 1 && m_buildController.IsVisible) {
			Show ();
		} else {
			Hide ();
		}
	}
	
	private void OnBuildVisible()
	{
		OnBuildHidden();
	}
		
	private void Show ()
	{
		var date = m_buildController.Data.Date;
		var focusedText = m_buildController.Data.LastChangeDescription;
		
		if (!m_isVisible) {
			m_text.enabled = true;
			
			if (string.IsNullOrEmpty (focusedText)) {
				m_text.text = string.Format ("[{0:dd/MM HH:mm}]", date);
			} else {
				m_text.text = string.Format ("[{0:dd/MM HH:mm}] {1}", date, focusedText);
			}
			
			iTweenHelper.MoveTo (gameObject, 
				iT.MoveTo.islocal, true,
				iT.MoveTo.z, YHeight,
				iT.MoveTo.time, 2f);
			
			GetComponent<Renderer>().enabled = true;
			m_isVisible = true;
		}
	}
	
	private void Hide ()
	{
		if (m_isVisible) {
			if (m_buildController.IsVisible) {
				iTweenHelper.MoveTo (gameObject, 
				iT.MoveTo.islocal, true,
				iT.MoveTo.z, -0.5f,
				iT.MoveTo.time, 2f,
				iT.MoveTo.oncomplete, "HideCompleted");
			} else {
				HideCompleted();
			}
				
		}
	}
	
	private void HideCompleted ()
	{
		m_text.enabled = false;
		GetComponent<Renderer>().enabled = false;
		m_isVisible = false;
	}
	#endregion
}
