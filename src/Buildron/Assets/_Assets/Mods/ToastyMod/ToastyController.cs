using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToastyController : MonoBehaviour {

	private AudioSource m_audio;
	private Vector2 m_currentTargetPosition;
	private float m_currentSpeed;

	public float WarmupSeconds = 2f;
	public Vector3 MoveSize = new Vector3(85, 0, 0);
	public float SlideInSpeed = 0.5f;
	public float SlideOutSpeed = 1f;

	void Awake()
	{
		m_audio = GetComponentInChildren<AudioSource> ();
	}

	void Update () {
		transform.position = Vector2.Lerp (transform.position, m_currentTargetPosition, m_currentSpeed);
	}

	void OnEnable()
	{
		StartCoroutine (Slide ());
	}

	public IEnumerator Slide()
	{
		m_currentTargetPosition = transform.position;
		yield return new WaitForSeconds (WarmupSeconds);

		var slideSeconds = m_audio.clip.length;

		// Slide in.
		m_currentTargetPosition = transform.position - MoveSize;
		m_currentSpeed = SlideInSpeed;
		m_audio.Play ();
		yield return new WaitForSeconds (slideSeconds);

		// Slide out.
		m_currentTargetPosition = transform.position + MoveSize;
		m_currentSpeed = SlideOutSpeed;
		yield return new WaitForSeconds (slideSeconds);
		gameObject.SetActive(false);
	}
}
