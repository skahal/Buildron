using System;
using Buildron.Domain.Mods;
using UnityEngine;

namespace Buildron.Infrastructure.UIProxies
{
	public class DefaultUIProxy : IUIProxy
	{
		public DefaultUIProxy ()
		{
		}

		public Font Font { get; set; }

		public void SetStatusText (string text, float secondsTimeout = 0)
		{
			StatusBarController.SetStatusText (text, secondsTimeout);
		}
	}
}

