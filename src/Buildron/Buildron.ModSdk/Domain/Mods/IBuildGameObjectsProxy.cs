namespace Buildron.Domain.Mods
{
	/// <summary>
	/// Defines an interface to build game objects proxy.
	/// </summary>
	public interface IBuildGameObjectsProxy
	{
		/// <summary>
		/// Get all instances that implement IBuildController interface.
		/// </summary>
		/// <remarks>
		/// This method will return all implementations of IBuildController instanced from all mods, so, this useful when you want that your
		/// mod do something related to all builds game objects. See a sample of this on Buildron.ClassicMods.CameraMod that use this method to know how to
		/// move the camera to show all builds game objects.
		/// </remarks>
		/// <returns>All IBuildController implentations intancs on Buildron right now.</returns>
		IBuildController[] GetAll();
	}
}