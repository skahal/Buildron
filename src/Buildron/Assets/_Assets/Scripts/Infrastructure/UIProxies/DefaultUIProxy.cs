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
	}
}

