using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Buildron.Domain.Mods
{
	[Flags]
    public enum CameraControllerKind
    {
        None = 0,
		Position = 1,
        Rotation = 2,
        Background = 4,
        Effect = 8
    }

    public interface ICameraProxy
    {
		TController RegisterController<TController>(CameraControllerKind kind, bool exclusive)
                where TController : MonoBehaviour;

        void UnregisterController<TController>()
            where TController : MonoBehaviour;
    }
}
