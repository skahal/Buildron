using System;

namespace Buildron.Domain.Versions
{
	/// <summary>
	/// Update info received event arguments.
	/// </summary>
	public class UpdateInfoReceivedEventArgs : EventArgs
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Versions.UpdateInfoReceivedEventArgs"/> class.
		/// </summary>
		/// <param name="updateInfo">Update info.</param>
		public UpdateInfoReceivedEventArgs (VersionUpdateInfo updateInfo)
		{
			UpdateInfo = updateInfo;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the update info.
		/// </summary>
		/// <value>The update info.</value>
		public VersionUpdateInfo UpdateInfo { get; private set; }
		#endregion
	}
}