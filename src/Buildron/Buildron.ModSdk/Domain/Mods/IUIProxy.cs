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

		/// <summary>
		/// Sets the status text of Buildron UI.
		/// </summary>
		/// <returns>The status text.</returns>
		/// <param name="text">The text.</param>
		/// <param name="secondsTimeout">The seconds timeout to text be removed. Default is zero(0).</param>
		void SetStatusText (string text, float secondsTimeout = 0);
	}
}