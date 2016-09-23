using System;
using Skahal.Logging;
using UnityEngine;

namespace Buildron.Domain.Mods
{
	public class LogCameraProxy : ICameraProxy
	{
		private ICameraProxy m_underlying;
		private ISHLogStrategy m_log;

		public LogCameraProxy(ICameraProxy underlying, ISHLogStrategy log)
		{
			m_underlying = underlying;
			m_log = log;
		}

		#region ICameraProxy implementation
		public Camera MainCamera { get { return m_underlying.MainCamera; } }

		public TController RegisterController<TController> (CameraControllerKind kind, bool exclusive) where TController : UnityEngine.MonoBehaviour
		{
			m_log.Debug (
				"Registering camera controller '{0}', kind '{1}' and exclusive {2}...", 
				typeof(TController).Name, kind, exclusive);
			
			return m_underlying.RegisterController<TController>(kind, exclusive);
		}

		public void UnregisterController<TController> () where TController : UnityEngine.MonoBehaviour
		{
			m_log.Debug (
				"Unregistering camera controller '{0}'...", 
				typeof(TController).Name);

			m_underlying.UnregisterController<TController> ();
		}

		#endregion
	}
}

