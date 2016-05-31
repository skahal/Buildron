
using System;

namespace Buildron.Domain.Versions
{
	public class UpdateInfoReceivedEventArgs : EventArgs
	{
		public UpdateInfoReceivedEventArgs (VersionUpdateInfo updateInfo)
		{
			UpdateInfo = updateInfo;
		}
		public VersionUpdateInfo UpdateInfo { get; private set; }
	}
}