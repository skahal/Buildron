using UnityEngine;
using System.Collections;
using Buildron.Domain.CIServers;

public class EmulatorCIServer : MonoBehaviour, ICIServer
{
	#region Fields
	public bool m_historyTotemEnabled;
	public int m_buildsTotemsNumber;
	#endregion

	#region Properties
	public static EmulatorCIServer Instance { get; private set; }
	#endregion

	#region Methods
	void Awake()
	{
		Instance = this;
	}
	#endregion

	#region ICIServer implementation
	public CIServerType ServerType { get; set; }

	public string Title  { get; set; }

	public string IP  { get; set; }

	public float RefreshSeconds  { get; set; }

	public bool FxSoundsEnabled  { get; set; }

	public bool HistoryTotemEnabled 
	{ 
		get 
		{ 
			return m_historyTotemEnabled; 
		} 
		set 
		{ 
			m_historyTotemEnabled = value;
		} 
	}

	public int BuildsTotemsNumber  
	{
		get 
		{ 
			return m_buildsTotemsNumber; 
		} 
		set 
		{ 
			m_buildsTotemsNumber = value;
		}
	}

	public CIServerStatus Status  { get; set; }

	#endregion

	#region IAuthUser implementation

	public string Domain { get; set; }

	public string DomainAndUserName { get; private set; }

	public string UserName  { get; set; }

	public string Password { get; set; }

	#endregion

	#region IEntity implementation

	public long Id  { get; set; }

	public bool IsNew {
		get {
			return Id == 0;
		}
	}

	#endregion
}
