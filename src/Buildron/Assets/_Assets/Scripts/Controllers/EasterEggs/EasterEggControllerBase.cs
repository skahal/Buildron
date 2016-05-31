#region Usings
using UnityEngine;
using System.Collections;
using Buildron.Domain;
using Skahal.Common;
using System.Collections.Generic;
using System.Linq;
#endregion

public abstract class EasterEggControllerBase : MonoBehaviour 
{	
	#region Constructors
	public EasterEggControllerBase()
	{
		EasterEggsNames = new List<string>();
	}
	#endregion
	
	#region Properties
	public IList<string> EasterEggsNames { get; private set; }
	#endregion
	
	#region Methods
	private void Awake ()
	{
		Messenger.Register (gameObject, "EasterEggReceived");
	}
	
	private void EasterEggReceived (string easterEgg)
	{		
		var easterEggeName = EasterEggsNames.FirstOrDefault (e => e.Equals (easterEgg, System.StringComparison.OrdinalIgnoreCase));
		
		if (easterEggeName != null)
		{
			SendMessage("On" + easterEggeName);
		}
	}
	#endregion
}