using System;
using Buildron.Domain.Users;

namespace Buildron.Domain.RemoteControls
{
    /// <summary>
    /// Represents a Buildron RC.
    /// </summary>
    [Serializable]
	public sealed class RemoteControl : UserBase
	{
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Buildron.Domain.RemoteControls.RemoteControl"/> is connected.
		/// </summary>
		/// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        public bool Connected { get; set; }
	}
}