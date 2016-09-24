using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Buildron.Domain.Mods
{
	/// <summary>
	/// Camera controller kind.
	/// </summary>
	[Flags]
    public enum CameraControllerKind
    {
		/// <summary>
		/// None.
		/// </summary>
        None = 0,

		/// <summary>
		/// Controller controls the camera position.
		/// </summary>
		Position = 1,

		/// <summary>
		/// Controller controls the camera rotation.
		/// </summary>
		Rotation = 2,

		/// <summary>
		/// Controller controls the camera background.
		/// </summary>
		Background = 4,

		/// <summary>
		/// Controller controls the camera effects.
		/// </summary>
		Effect = 8
    }

	/// <summary>
	/// Defines an interface to a camera proxy.
	/// </summary>
	/// <remarks>
	/// The main responsibility of this interface is to control access of the various mods to the Buildron's main camera 
	/// and avoid more than one mod tries to change exclusive values, like camera position, in the same time.
	/// </remarks>
    public interface ICameraProxy
    {
		#region Properties
		/// <summary>
		/// Gets the main camera.
		/// </summary>
		/// <value>The main camera.</value>
		Camera MainCamera { get; }
		#endregion

		/// <summary>
		/// Registers the controller by specifying the kind and if the controller should register as an exclusive controller to camera kind.
		/// The previous register controller will be disabled if any exclusive control was requested by CameraControllerKind.
		/// </summary>
		/// <remarks>
		/// CameraControllerKind is a flag enum, so you can combine them to register a camera controller to more than one kind.
		/// </remarks>
		/// <example>
		/// <para>
		/// Mod 1 calls
		/// RegisterController&lt;Mod1CameraController&gt;(CameraControllerKind.Position | CameraControllerKind.Rotation, true);
		/// </para>
		/// <para>
		/// Mod 2 calls
		/// RegisterController&lt;Mod2CameraController&gt;(CameraControllerKind.Position | CameraControllerKind.Background, false);
		/// The Mod1CameraController will be disabled, because it needs exclusive control of position.
		/// </para>
		/// <para>
		/// Mod 3 calls
		/// RegisterController&lt;Mod3CameraController&gt;(CameraControllerKind.Background, false);
		/// The Mod2CameraController will NOT be disabled, because the Mod 2 and Mod 3 controls camera background, but they do not need exclusive control.
		/// </para>
		/// </example>
		/// <returns>The controller.</returns>
		/// <param name="kind">Kind.</param>
		/// <param name="exclusive">Exclusive.</param>
		/// <typeparam name="TController">The 1st type parameter.</typeparam>
		TController RegisterController<TController>(CameraControllerKind kind, bool exclusive)
                where TController : MonoBehaviour;

		/// <summary>
		/// Unregisters the controller and enable again the previous one registered.
		/// </summary>
		/// <returns>The controller.</returns>
		/// <typeparam name="TController">The 1st type parameter.</typeparam>

        void UnregisterController<TController>()
            where TController : MonoBehaviour;
    }
}
