#region Usings
using Buildron.Domain;
using UnityEngine;
using UnityEngine.UI;


#endregion

[AddComponentMenu("Buildron/Controllers/BuildUserController")]
[RequireComponent(typeof(BuildUserAnimationController))]
public class BuildUserController : MonoBehaviour {
	
	#region Fields
	private static Object s_buildUserPrefab = Resources.Load ("BuildUserPrefab");
	private Vector3 m_targetPosition;
	private bool m_canWalk;
	private bool m_canAnimate;
	private BuildUser m_data;
	private Vector3 m_spawnPosition;
	private bool m_alreadyAwake;
	private GameObject m_body;
	private bool m_photoAlreadySet;
	private BuildUserAnimationController m_animationController;
	private BuildStatus? m_currentStatus;
	#endregion
	
	#region Editor properties
	public Material VisibleMaterial;
	public Material InvisibleMaterial;
	#endregion
	
	#region Properties
	public BuildUser Data {
		get {
			return m_data;
		}
		
		set {
			m_data = value;
			m_animationController.Data = value;
			UpdateFromData ();
		}
	}
	#endregion
	
	#region Life cycle
	private void UpdateFromData ()
	{
		if (m_alreadyAwake && m_data != null) {
			UpdateUserPhoto ();
			m_canWalk = true;
			m_canAnimate = true;
			
			m_targetPosition = new Vector3 (m_spawnPosition.x, m_spawnPosition.y, m_spawnPosition.z + 8f);
		}
	}
	
	private void Awake ()
	{
		m_animationController = gameObject.GetComponent<BuildUserAnimationController> ();
		m_body = transform.FindChild ("rootJoint").gameObject;
		MarkAsVisible ();
		Messenger.Register (gameObject,
			"OnCameraZoomIn",
			"OnCameraZoomOut");
		
		BuildService.BuildUpdated += delegate(object sender, BuildUpdatedEventArgs e) {
			UpdateFromData ();
		};
	}
	
	private void Start ()
	{
		m_spawnPosition = transform.position;
		m_alreadyAwake = true;
		UpdateFromData ();
	}
	
	private void UpdateUserPhoto ()
	{
		if (!m_photoAlreadySet) {
			BuildUserService.GetUserPhoto (m_data, (photo) => {
				m_photoAlreadySet = true;
				var photoHolder = transform.FindChild ("Canvas/Photo").GetComponent<Image> ();
				photoHolder.enabled = true;
				photoHolder.sprite = photo.ToSprite();
			});
		}
	}
	
	private void Update ()
	{
		if (m_canWalk) {
			if (Vector3.Distance (transform.position, m_targetPosition) > 2) {
			
				if (!GetComponent<Animation>().isPlaying) {
					GetComponent<Animation>().CrossFade ("walk");	
				}
			
				transform.position = Vector3.Lerp (transform.position, m_targetPosition, Time.deltaTime * 0.4f);
			} else if (m_canAnimate) {
				m_animationController.Play ();	
				m_canWalk = false;
			}
		}
	}
	
	private void MarkAsVisible ()
	{
		m_body.GetComponent<Renderer>().material = VisibleMaterial;	
	}
	
	private void MarkAsInvisible ()
	{
		m_body.GetComponent<Renderer>().material = InvisibleMaterial;
	}
	
	private void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.tag.Equals ("BuildUser")) {
			m_canWalk = false;
			GetComponent<Animation>().Play ("idle");	
		}
	}
	
	private void OnCameraZoomIn ()
	{
		MarkAsInvisible();
	}
	
	private void OnCameraZoomOut()
	{
		MarkAsVisible();
	}
	#endregion
	
	#region Methods
	
	public static bool ExistsGameObject (BuildUser buildUser)
	{
		return GameObject.Find (buildUser.UserName) != null;
	}
	
	public static GameObject GetGameObject (BuildUser buildUser)
	{
		return GetGameObject(buildUser.UserName);
	}
	
	public static GameObject GetGameObject (string userName)
	{
		return GameObject.Find (userName.ToLowerInvariant ());
	}
	
	public static GameObject[] GetAllGameObjects ()
	{
		return GameObject.FindGameObjectsWithTag("BuildUser");
	}
	
	public static GameObject CreateGameObject (BuildUser buildUser)
	{
		var go = GameObject.Find (buildUser.UserName);
			
		if (go == null) {
			go = (GameObject)GameObject.Instantiate (s_buildUserPrefab);
			go.name = buildUser.UserName.ToLowerInvariant();
			var script = go.GetComponent<BuildUserController> ();
			script.Data = buildUser;
		}
		
		return go;
	}
	#endregion
}
