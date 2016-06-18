using System;
using Skahal.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Buildron.Domain.EasterEggs
{
	/// <summary>
	/// Easter egg service.
	/// </summary>
	public class EasterEggService
	{
		#region Fields
		private IEnumerable<IEasterEggProvider> m_easterEggProviders;
		private ISHLogStrategy m_log;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.EasterEggs.EasterEggService"/> class.
		/// </summary>
		/// <param name="easterEggProviders">Easter egg providers.</param>
		/// <param name="log">Log.</param>
		public EasterEggService(IEnumerable<IEasterEggProvider> easterEggProviders, ISHLogStrategy log)
		{
			m_easterEggProviders = easterEggProviders;
			m_log = log;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Check if the specified message is an easter egg message.
		/// </summary>
		/// <returns>
		/// True if is an easter egg message, otherwise false.
		/// </returns>
		/// <param name='message'>
		/// The message received from a Remote Control.
		/// </param>
		public bool IsEasterEggMessage (string message)
		{
			return !String.IsNullOrEmpty (message) && message.StartsWith ("/");
		}
		
		/// <summary>
		/// Receives the easter egg.
		/// </summary>
		/// <returns>
		/// The easter egg.
		/// </returns>
		/// <param name='message'>
		/// If set to <c>true</c> message.
		/// </param>
		public bool ReceiveEasterEgg (string message)
		{
			if (IsEasterEggMessage (message)) {
				var easterEggName = GetEasterEggNameFromMessage (message);
				var providers = m_easterEggProviders.Where (p => p.CanExecute (easterEggName)).ToArray ();

				if (providers.Length > 0) {
					m_log.Debug ("Easter egg received '{0}'", easterEggName);

					foreach (var p in providers) {
						m_log.Debug ("Executing easter egg provide '{0}'", p.GetType ().Name);
						p.Execute (easterEggName);
					}

					return true;
				}
			}

			return false;
		}

		private string GetEasterEggNameFromMessage (string message) 
		{
			return message.Substring (1).ToLowerInvariant ();
		}
		#endregion
	}
}