
namespace Buildron.Domain.Notifications
{
	public sealed class Notification
	{
		#region Constructors
		public Notification (string name, string text)
		{
			Name = name;
			Text = text;
		}
		#endregion
		
		#region Properties
		public string Name { get; private set; }
		public string Text { get; private set; }
		#endregion
	}
}