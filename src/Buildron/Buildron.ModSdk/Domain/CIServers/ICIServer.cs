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
	/// Defines an interface to the Continuous Integration Server entity.
	/// </summary>
	public interface ICIServer : IAuthUser
	{
		#region Properties
		/// <summary>
		/// Gets or sets the type of the server.
		/// </summary>
		/// <value>
		/// The type of the server.
		/// </value>
		CIServerType ServerType { get; set; }

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		string Title { get; set; }

		/// <summary>
		/// Gets or sets the IP.
		/// </summary>
		string IP { get; set; }

		/// <summary>
		/// Gets or sets the refresh seconds.
		/// </summary>
		float RefreshSeconds { get; set; }

		/// <summary>
		/// Gets or sets if FX Sounds are enabled.
		/// </summary>
		bool FxSoundsEnabled { get; set; }

		/// <summary>
		/// Gets or sets if History Totem is enabled.
		/// </summary>
		bool HistoryTotemEnabled { get; set; }

		/// <summary>
		/// Gets or sets builds totems number.
		/// </summary>
		int BuildsTotemsNumber { get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
		CIServerStatus Status { get; set; }
		#endregion
	}
}