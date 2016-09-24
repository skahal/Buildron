using System.Linq;
using Buildron.Domain.Builds;
using Buildron.Domain.Mods;
using UnityEngine;

/// <summary>
/// Build controller extensions.
/// </summary>
public static class BuildControllerExtensions
{	
	/// <summary>
	/// Gets the game object that is holding the specified build as modeol.
	/// </summary>
	/// <returns>The game object.</returns>
	/// <param name="builds">The builds.</param>
	/// <param name="build">The build model.</param>
    public static GameObject GetGameObject (this IBuildController[] builds, IBuild build)
    {
        var controller = builds.FirstOrDefault(b => b.Model == build);

        return controller == null ? null : controller.gameObject;
    }
}
