using System;
using Buildron.Domain.Users;

namespace Buildron.Domain.RemoteControls
{
    /// <summary>
    /// Represents a Buildron RC.
    /// </summary>
    [Serializable]
	public sealed class RemoteControl : UserBase, IRemoteControl
    {
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Buildron.Domain.RemoteControls.IRemoteControl"/> is connected.
		/// </summary>
		/// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        public bool Connected { get; set; }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Buildron.Domain.RemoteControls.IRemoteControl"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Buildron.Domain.RemoteControls.IRemoteControl"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("{0} ({1})", UserName, Connected ? "connected" : "disconnected");
		}
	}
}