using System;

namespace Buildron.Domain.RemoteControls
{
    /// <summary>
    /// Arguments for remote control changed events.
    /// </summary>
    public class RemoteControlChangedEventArgs : EventArgs
	{
        #region Constructors
        /// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.RemoteControls.RemoteControlChangedEventArgs"/> class.
        /// </summary>
        /// <param name="remoteControl">The remote control.</param>
        public RemoteControlChangedEventArgs(IRemoteControl remoteControl)
        {
            RemoteControl = remoteControl;
        }
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the remote control.
        /// </summary>
		public IRemoteControl RemoteControl { get; private set; }
        #endregion
    }
}