namespace Buildron.Domain.RemoteControls
{
	/// <summary>
	/// Filter builds remote control command.
	/// </summary>
	public class FilterBuildsRemoteControlCommand : IRemoteControlCommand
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Buildron.Domain.RemoteControls.FilterBuildsRemoteControlCommand"/> class.
		/// </summary>
		public FilterBuildsRemoteControlCommand ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Buildron.Domain.RemoteControls.FilterBuildsRemoteControlCommand"/> class.
		/// </summary>
		/// <param name="keyWord">Key word.</param>
		public FilterBuildsRemoteControlCommand (string keyWord)
		{
			KeyWord = keyWord;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Buildron.Domain.RemoteControls.FilterBuildsRemoteControlCommand"/> class.
		/// </summary>
		/// <param name="successEnabled">Success enabled.</param>
		/// <param name="runningEnabled">Running enabled.</param>
		/// <param name="failedEnabled">Failed enabled.</param>
		/// <param name="queuedEnabled">Queued enabled.</param>
		public FilterBuildsRemoteControlCommand (
			bool? successEnabled,
			bool? runningEnabled,
			bool? failedEnabled,
			bool? queuedEnabled)
		{
			SuccessEnabled = successEnabled;
			RunningEnabled = runningEnabled;
			FailedEnabled = failedEnabled;
			QueuedEnabled = queuedEnabled;
		}
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the success enabled.
        /// </summary>
        /// <value>The success enabled.</value>
        public bool? SuccessEnabled { get; set; }

        /// <summary>
        /// Gets or sets the running enabled.
        /// </summary>
        /// <value>The running enabled.</value>
        public bool? RunningEnabled { get; set; }

        /// <summary>
        /// Gets or sets the failed enabled.
        /// </summary>
        /// <value>The failed enabled.</value>
        public bool? FailedEnabled { get; set; }

		/// <summary>
		/// Gets the queued enabled.
		/// </summary>
		/// <value>The queued enabled.</value>
		public bool? QueuedEnabled { get; set; }

		/// <summary>
		/// Gets or sets the key word.
		/// </summary>
		/// <value>The key word.</value>
		public string KeyWord { get; set; }
		#endregion
	}
}

