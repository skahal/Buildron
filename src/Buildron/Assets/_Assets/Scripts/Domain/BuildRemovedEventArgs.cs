using System;

namespace Buildron.Domain
{
    /// <summary>
    /// Arguments for build removed events.
    /// </summary>
    public class BuildRemovedEventArgs : BuildEventArgsBase
	{
		#region Constructors
		public BuildRemovedEventArgs (Build build)
			: base(build)
		{
		}
        #endregion
	}
}