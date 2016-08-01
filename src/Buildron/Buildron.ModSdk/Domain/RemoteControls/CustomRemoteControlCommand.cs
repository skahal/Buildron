namespace Buildron.Domain.RemoteControls
{
	/// <summary>
	/// Custom remote control command.
	/// </summary>
	public class CustomRemoteControlCommand : IRemoteControlCommand
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Buildron.Domain.RemoteControls.CustomRemoteControlCommand"/> class.
		/// </summary>
		/// <param name="name">The custom command name.</param>
		public CustomRemoteControlCommand (string name)
		{
			Name = name;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; private set; }
		#endregion
	}
}

