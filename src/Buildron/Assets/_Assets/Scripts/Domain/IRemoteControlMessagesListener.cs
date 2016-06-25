using System;

namespace Buildron.Domain
{
	/// <summary>
	/// Define an interface to a listener for messages sent from remote control.
	/// </summary>
    public interface IRemoteControlMessagesListener
    {
		#region Events
		/// <summary>
		/// Occurs when build filter is updated.
		/// </summary>
		event EventHandler<BuildFilterUpdatedEventArgs> BuildFilterUpdated;

		/// <summary>
		/// Occurs when build sort is updated.
		/// </summary>
		event EventHandler<BuildSortUpdatedEventArgs> BuildSortUpdated;
		#endregion

		#region Methods
		void SendToServerAuthentication (string userName, string password);
		void SendToServerTakeScreenshot ();
		void SendToServerSortBuilds (int sortingAlgorithmType, int sortBy);
		void SendToServerShowHistory ();
		void SendToServerShowBuilds ();
		void SendToServerZoomIn ();
		void SendToServerZoomOut ();
		void SendToServerGoLeft ();
		void SendToServerGoRight ();
		void SendToServerGoUp ();
		void SendToServerGoDown ();
		void SendToServerResetCamera ();
		void ShowSuccessBuilds (bool show);
		void ShowRunningBuilds (bool show);
		void ShowFailedBuilds (bool show);
		void ShowQueuedBuilds (bool show);
		void ShowBuildsWithName (string partialName);
		void RunBuild ();
		void StopBuild ();
		#endregion
    }
}