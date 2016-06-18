using UnityEngine;
using System.Collections;
using Buildron.Domain;
using Skahal.Common;
using System.Collections.Generic;
using System.Linq;
using Buildron.Domain.EasterEggs;

/// <summary>
/// Easter egg controller base.
/// </summary>
public abstract class EasterEggControllerBase : MonoBehaviour, IEasterEggProvider
{	
	#region Constructors
	/// <summary>
	/// Initializes a new instance of the <see cref="EasterEggControllerBase"/> class.
	/// </summary>
	protected EasterEggControllerBase()
	{
		EasterEggsNames = new List<string>();
	}
	#endregion
	
	#region Properties
	/// <summary>
	/// Gets the easter eggs names.
	/// </summary>
	/// <value>The easter eggs names.</value>
	public IList<string> EasterEggsNames { get; private set; }
	#endregion
	
	#region Methods
	/// <summary>
	/// Verify if the provider can execute the easter egg.
	/// </summary>
	/// <param name="easterEggName">The name of easter egg to execute</param>
	/// <returns><c>true</c> if this instance can execute the specified easterEggName; otherwise, <c>false</c>.</returns>
	/// <param name="easterEggName">Easter egg name.</param>
	public bool CanExecute (string easterEggName)
	{		
		return EasterEggsNames.Any (e => e.Equals (easterEggName, System.StringComparison.OrdinalIgnoreCase));
	}

	/// <summary>
	/// Execute the easter egg.
	/// </summary>
	/// <param name="easterEggName">The name of easter egg to execute</param>
	/// <returns>If an easter egg was executed.</returns>
	/// <param name="easterEggName">Easter egg name.</param>
	public bool Execute (string easterEggName)
	{		
		var availableEasterEggNames = EasterEggsNames
			.Where (e => e.Equals (easterEggName, System.StringComparison.OrdinalIgnoreCase))
			.ToArray ();
		
		foreach (var availableEasterEggName in availableEasterEggNames) {	
			if (availableEasterEggName != null) {
				SendMessage ("On" + availableEasterEggName);
			}
		}

		return availableEasterEggNames.Length > 0;
	}
	#endregion
}