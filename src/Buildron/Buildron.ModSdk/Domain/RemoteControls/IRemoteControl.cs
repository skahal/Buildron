using Buildron.Domain.Users;

namespace Buildron.Domain.RemoteControls
{
    /// <summary>
    /// Defines an interface for a Buildron RC.
    /// </summary>
    public interface IRemoteControl : IAuthUser
    {
        /// <summary>
		/// Gets or sets a value indicating whether this <see cref="Buildron.Domain.RemoteControls.IRemoteControl"/> is connected.
		/// </summary>
		/// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        bool Connected { get; set; }
    }
}