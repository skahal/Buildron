#region Usings
using System.Collections;
using Buildron.Domain;
using Skahal.Common;
using Skahal.Effects;
using UnityEngine;
#endregion

/// <summary>
/// Matrix easter egg controller.
/// </summary>
[AddComponentMenu("Buildron/EasterEggs/MatrixEasterEggController")]
public class MatrixEasterEggController : EasterEggControllerBase
{
	#region Fields
	private bool m_actived;
	private MeshFilter[] m_currentAppliedTo;
	#endregion
	
	#region Editor properties
	public Camera TargetCamera;
	#endregion 
	
	#region Life cycle
	void Start ()
	{
		EasterEggsNames.Add("Matrix");
	}
	
	void OnMatrix ()
	{
		if (m_actived) {
			WireframeEffectService.Unapply (m_currentAppliedTo);
			m_actived = false;
		} else {
			m_actived = true;
			m_currentAppliedTo = WireframeEffectService.ApplyToAllSceneMeshFilters ((controller) => {
				controller.TargetCamera = TargetCamera;
			});
		}
	}
	#endregion
}

