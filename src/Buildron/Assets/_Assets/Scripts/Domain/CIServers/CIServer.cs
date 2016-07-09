using System;
using Buildron.Domain.Users;

namespace Buildron.Domain.CIServers
{
	/// <summary>
	/// The Continuous Integration Server entity.
	/// </summary>
	[Serializable]
	public sealed class CIServer : UserBase, ICIServer
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.CIServers.CIServer"/> class.
		/// </summary>
		public CIServer ()
		{
			ServerType = CIServerType.TeamCity;
			Title = "Buildron";
			IP = string.Empty;
			UserName = string.Empty;
			Domain = string.Empty;
			Password = string.Empty;
			RefreshSeconds = 10;
			FxSoundsEnabled = true;
			HistoryTotemEnabled = true;
			BuildsTotemsNumber = 2;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the type of the server.
		/// </summary>
		/// <value>
		/// The type of the server.
		/// </value>
		public CIServerType ServerType { get; set; }

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the IP.
		/// </summary>
		public string IP { get; set; }

		/// <summary>
		/// Gets or sets the refresh seconds.
		/// </summary>
		public float RefreshSeconds { get; set; }

		/// <summary>
		/// Gets or sets if FX Sounds are enabled.
		/// </summary>
		public bool FxSoundsEnabled { get; set; }

		/// <summary>
		/// Gets or sets if History Totem is enabled.
		/// </summary>
		public bool HistoryTotemEnabled { get; set; }

		/// <summary>
		/// Gets or sets builds totems number.
		/// </summary>
		public int BuildsTotemsNumber { get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
		public CIServerStatus Status { get; set; }
		#endregion
	}
}