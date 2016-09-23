using Buildron.Domain.Mods;
using UnityEngine;

namespace Buildron.Infrastructure.CameraProxies
{
    public class CameraControllerInfo
    {
        public MonoBehaviour Controller { get; set; }

        public CameraControllerKind Kind { get; set; }

        public bool Exclusive { get; set; }
    }
}
