using System;
using Zenject;
using UnityEngine;
using Skahal.Logging;

namespace Buildron.Infrastructure.IoC
{
	/// <summary>
	/// DiContainer extension methods.
	/// </summary>
	public static class DiContainerExtensions
	{
		/// <summary>
		/// Binds the controller.
		/// </summary>
		/// <returns>The controller.</returns>
		/// <param name="container">Container.</param>
		/// <param name="createGameObject">If set to <c>true</c> create game object.</param>
		/// <typeparam name="TController">The 1st type parameter.</typeparam>
		public static ConditionBinder BindController<TController>(this DiContainer container, bool createGameObject = false)
			where TController : MonoBehaviour
		{
			var name = typeof(TController).Name;
			SHLog.Debug ("Binding controller '{0}'", name);

			var go = createGameObject 
				? new GameObject (name)
				: GameObject.Find (name);

			if (go == null) {
				throw new InvalidOperationException("Could not find a GameObject with name '{0}'".With(name));
			}

			var controller = createGameObject 
				? go.AddComponent<TController> ()
				: go.GetComponent<TController> ();

			container.Inject (controller);

			var initializable = controller as IInitializable;

			if (initializable != null) {
				SHLog.Debug ("Controller '{0}' binding IInitializable", name);
				container.Bind<IInitializable> ().FromInstance (initializable);
			}

			SHLog.Debug ("Controller '{0}' binding TController", name);
			var binder = container.Bind<TController> ().FromInstance (controller);

			SHLog.Debug ("Controller '{0}' bind done.", name);

			return binder;
		}

        public static ConditionBinder BindInitializableService<TServiceInterface, TServiceImplementation>(this DiContainer container)            
            where TServiceImplementation : TServiceInterface, IInitializable
        {
            var name = typeof(TServiceInterface).Name;

            SHLog.Debug("Binding service '{0}'", name);
            var binder = container.Bind<TServiceInterface>().To<TServiceImplementation>().AsSingle();

            SHLog.Debug("Service '{0}' binding IInitializable", name);
            container.Bind<IInitializable>().To<TServiceImplementation>().AsSingle();
            
            SHLog.Debug("Service '{0}' bind done.", name);

            return binder;
        }
    }
}

