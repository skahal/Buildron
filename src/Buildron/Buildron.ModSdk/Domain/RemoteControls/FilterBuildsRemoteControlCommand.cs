using System;

namespace Buildron.Domain.RemoteControls
{
	public class FilterBuildsRemoteControlCommand : IRemoteControlCommand
	{
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

		public bool? SuccessEnabled { get; private set; }
		public bool? RunningEnabled { get; private set; }
		public bool? FailedEnabled { get; private set; }
		public bool? QueuedEnabled { get; private set; }
	}
}

