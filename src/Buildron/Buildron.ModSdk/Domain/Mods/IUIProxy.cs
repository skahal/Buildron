using System;
using UnityEngine;

namespace Buildron.Domain.Mods
{
	/// <summary>
	/// Define an interface to UI proxy.
	/// </summary>
	public interface IUIProxy
	{
		/// <summary>
		/// Gets or sets the font.
		/// </summary>
		/// <value>The font.</value>
		Font Font { get; set; }

		void SetStatusText (string text, float secondsTimeout = 0);
	}
}