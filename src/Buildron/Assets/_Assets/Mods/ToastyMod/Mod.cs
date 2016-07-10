using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Buildron.Domain.Mods;
using Buildron.Domain.Builds;

namespace ToastyMod
{
	public class Mod : IMod 
	{
		#region IMod implementation
		public void Initialize (IModContext context)
		{
			var holder = GameObject.Instantiate(context.AssetsLoader.Load ("ToastyHolderPrefab") as Object) as GameObject;

			context.BuildStatusChanged += (sender, e) => {
				if (e.Build.Status == BuildStatus.Success)
				{
					holder.SetActive(true);
				}
			};
		}

		public string Name {
			get {
				return "Toasty";
			}
		}
		#endregion
	}
}
