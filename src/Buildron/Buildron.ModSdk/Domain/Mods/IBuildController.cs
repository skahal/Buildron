using Buildron.Domain.Builds;

namespace Buildron.Domain.Mods
{
	/// <summary>
	/// Defines an interface to game objects that act as an build game object controller.
	/// </summary>
	/// <remarks>
	/// While is not mandatory you use this interface on your build game objects, it's highly recommended that you implement it, because this allow
	/// others mods know what are the currently build game objects and react to them, like Buildron.ClassicMods.CameraMod that move the camera to show all build game objects.
	/// </remarks>
	public interface IBuildController : IGameObjectController
	{
		/// <summary>
		/// Gets the model.
		/// </summary>
		/// <value>The model.</value>
		IBuild Model { get;  }
	}
}