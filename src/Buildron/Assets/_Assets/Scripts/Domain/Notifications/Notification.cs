namespace Buildron.Domain.Notifications
{
	/// <summary>
	/// Represents a notification sent to a Buildron's client.
	/// </summary>
	public sealed class Notification
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Notifications.Notification"/> class.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="text">Text.</param>
		public Notification (string name, string text)
		{
			Name = name;
			Text = text;
		}
		#endregion
		
		#region Properties
		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; private set; }

		/// <summary>
		/// Gets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text { get; private set; }
		#endregion
	}
}