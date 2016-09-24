using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buildron.Domain.Mods;
using UnityEngine;

namespace Buildron.Infrastructure.CameraProxies
{
    public class ModCameraProxy : ICameraProxy
    {
        private static List<CameraControllerInfo> s_infos = new List<CameraControllerInfo>();
        private static List<CameraControllerInfo> s_disabledInfos = new List<CameraControllerInfo>();
		private ModInfo m_modInfo;

        public ModCameraProxy(ModInfo modInfo, Camera camera)
        {
			m_modInfo = modInfo;
			MainCamera = camera;
        }

		public Camera MainCamera { get; private set; }

		public static void Reset() {
			s_infos = new List<CameraControllerInfo>();
			s_disabledInfos = new List<CameraControllerInfo>();	
		}

        public TController RegisterController<TController>(CameraControllerKind kind, bool exclusive)
               where TController : MonoBehaviour
        {
            var info = GetDisabledInfo<TController>() ?? new CameraControllerInfo();
            info.Kind = kind;
            info.Exclusive = exclusive;

            DisableControllers(info);          

			var controller = info.Controller as TController ?? CreateController<TController> (m_modInfo);
			controller.gameObject.SetActive(true);
            info.Controller = controller;                
            s_infos.Add(info);

            return controller;

        }       

        public void UnregisterController<TController>()
            where TController : MonoBehaviour
        {
            var info = GetInfo<TController>();

            if(info == null)
            {
                return;
            }

			info.Controller.gameObject.SetActive(false);
            s_infos.Remove(info);
            s_disabledInfos.Add(info);

            if (s_infos.Count > 0)
            {
				s_infos.Last().Controller.gameObject.SetActive(true);
            }
        }

		TController CreateController<TController> (ModInfo info)
			where TController : MonoBehaviour
		{
			var go = new GameObject ("{0}_{1}".With(info.Name, typeof(TController).Name));
			go.transform.parent = MainCamera.gameObject.transform;
			return go.AddComponent<TController>();
		}


        private CameraControllerInfo GetInfo<TController>() 
            where TController : MonoBehaviour
        {
            var controllerType = typeof(TController);

            return s_infos.FirstOrDefault(c => c.Controller.GetType() == controllerType);
        }

        private CameraControllerInfo GetDisabledInfo<TController>()
            where TController : MonoBehaviour
        {
            var controllerType = typeof(TController);

            var info = s_disabledInfos.FirstOrDefault(c => c.Controller.GetType() == controllerType);

            if(info == null)
            {
                return GetInfo<TController>();
            }

            s_disabledInfos.Remove(info);

            return info;
        }

        private void DisableControllers(CameraControllerInfo info)
        {
            foreach(var c in s_infos)
            {
                if (info.Exclusive || c.Exclusive)
                {
                    if ((info.Kind & c.Kind) != 0)
                    {
						c.Controller.gameObject.SetActive(false);
                    }
                }                
            }
        }
    }
}
