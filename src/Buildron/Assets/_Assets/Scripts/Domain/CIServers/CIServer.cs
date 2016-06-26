using System;
using Buildron.Domain.Users;

namespace Buildron.Domain.CIServers
{
	/// <summary>
	/// Continous integration server types.
	/// </summary>
	public enum CIServerType
	{
		/// <summary>
		/// Hudson: http://hudson-ci.org
		/// </summary>
		Hudson = 1,

		/// <summary>
		/// Jenkins: https://jenkins.io
		/// </summary>
		Jenkins = 2,

		/// <summary>
		/// TeamCity: https://www.jetbrains.com/teamcity/
		/// </summary>
		TeamCity = 3
	}

	/// <summary>
	/// Continuous integration server status.
	/// </summary>
	public enum CIServerStatus
	{
		/// <summary>
		/// Continous integration server is Up.
		/// </summary>
		Up,

		/// <summary>
		/// Continous integration server is down.
		/// </summary>
		Down
	}

	/// <summary>
	/// The Continuous Integration Server entity.
	/// </summary>
	[Serializable]
	public sealed class CIServer : UserBase
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