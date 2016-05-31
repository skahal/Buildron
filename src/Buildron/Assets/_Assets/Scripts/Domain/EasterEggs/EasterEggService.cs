#region Usings
using System;
using Skahal.Logging;
#endregion

namespace Buildron.Domain.EasterEgss
{
	/// <summary>
	/// Easter egg service.
	/// </summary>
	public static class EasterEggService
	{
		#region Methods
		/// <summary>
		/// Check if the specified message is a Easter Egg message.
		/// </summary>
		/// <returns>
		/// True if is an Easter Egg message, otherwise false.
		/// </returns>
		/// <param name='message'>
		/// The message received from a Remote Control.
		/// </param>
		public static bool IsEasterEggMessage (string message)
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
		public static bool ReceiveEasterEgg (string message)
		{
			var isEasterEgg = IsEasterEggMessage (message);
			
			if (isEasterEgg) {
				SHLog.Debug ("Easter Egg received:" + message);
				Messenger.Send("EasterEggReceived", message.Substring(1).ToLowerInvariant());
			} 
			
			return isEasterEgg;
		}
		#endregion
	}
}