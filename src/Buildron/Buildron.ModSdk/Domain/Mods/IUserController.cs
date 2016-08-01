using Buildron.Domain.Users;

namespace Buildron.Domain.Mods
{
	/// <summary>
	/// Defines an interface to game objects that act as an user game object controller.
	/// </summary>
	/// <remarks>
	/// While is not mandatory you use this interface on your user game objects, it's highly recommended that you implement it, because this allow
	/// others mods know what are the currently user game objects and react to them, like Buildron.ClassicMods.CameraMod that move the camera to show all user game objects.
	/// </remarks>
	public interface IUserController : IGameObjectController
	{
		/// <summary>
		/// Gets the model.
		/// </summary>
		/// <value>The model.</value>
		IUser Model { get;  }
	}
}
