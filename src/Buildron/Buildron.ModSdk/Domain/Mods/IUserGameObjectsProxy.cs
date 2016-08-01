namespace Buildron.Domain.Mods
{
	/// <summary>
	/// Defines an interface to user game objects proxy.
	/// </summary>
	public interface IUserGameObjectsProxy
	{
		/// <summary>
		/// Get all instances that implement IUserController interface.
		/// </summary>
		/// <remarks>
		/// This method will return all implementations of IUserController instanced from all mods, so, this useful when you want that your
		/// mod do something related to all user game objects. See a sample of this on Buildron.ClassicMods.CameraMod that use this method to know how to
		/// move the camera to show all users game objects.
		/// </remarks>
		/// <returns>All IUserController implentations intancs on Buildron right now.</returns>
		IUserController[] GetAll();
	}
}