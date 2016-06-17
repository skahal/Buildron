using System;

namespace Buildron.Domain
{
    /// <summary>
    /// Arguments for remote control changed events.
    /// </summary>
    public class RemoteControlChangedEventArgs : EventArgs
	{
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Buildron.Domain.RemoteControlChangedEventArgs"/> class.
        /// </summary>
        /// <param name="remoteControl">The remote control.</param>
        public RemoteControlChangedEventArgs(RemoteControl remoteControl)
        {
            RemoteControl = remoteControl;
        }
        #endregion

        #region Properties        
        /// <summary>
        /// Gets the remote control.
        /// </summary>
		public RemoteControl RemoteControl { get; private set; }
        #endregion
    }
}