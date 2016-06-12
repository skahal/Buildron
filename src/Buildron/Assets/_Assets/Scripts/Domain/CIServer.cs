namespace Buildron.Domain
{
	/// <summary>
	/// CI server type.
	/// </summary>
	public enum CIServerType { 
		Hudson = 1,
		Jenkins = 2,
		TeamCity = 3
	}

	/// <summary>
	/// CI server status.
	/// </summary>
	public enum CIServerStatus {
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
	public sealed class CIServer : UserBase
	{
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