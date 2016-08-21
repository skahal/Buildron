using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buildron.Domain.RemoteControls;

namespace Buildron.Domain.Mods
{
	/// <summary>
	/// Define an interface to a remote control proxy.
	/// </summary>
    public interface IRemoteControlProxy
    {
		/// <summary>
		/// Gets the current.
		/// </summary>
		/// <value>The current.</value>
        IRemoteControl Current { get; }

		/// <summary>
		/// Receives the command.
		/// </summary>
		/// <returns><c>true</c>, if command was received, <c>false</c> otherwise.</returns>
		/// <param name="command">The command.</param>
        bool ReceiveCommand(IRemoteControlCommand command);
    }
}
