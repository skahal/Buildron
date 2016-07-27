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
        private Camera m_camera;
        private List<CameraControllerInfo> m_infos = new List<CameraControllerInfo>();
        private List<CameraControllerInfo> m_disabledInfos = new List<CameraControllerInfo>();

        public ModCameraProxy(Camera camera)
        {
            m_camera = camera;
        }

        public TController RegisterController<TController>(CameraControllerKind kind, bool exclusive)
               where TController : MonoBehaviour
        {
            var info = GetDisabledInfo<TController>() ?? new CameraControllerInfo();
            info.Kind = kind;
            info.Exclusive = exclusive;

            DisableControllers(info);          

            var controller = info.Controller as TController ?? m_camera.gameObject.AddComponent<TController>();
            controller.enabled = true;
            info.Controller = controller;                
            m_infos.Add(info);

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

            info.Controller.enabled = false;
            m_infos.Remove(info);
            m_disabledInfos.Add(info);

            if (m_infos.Count > 0)
            {
                m_infos.Last().Controller.enabled = true;
            }
        }

        private CameraControllerInfo GetInfo<TController>() 
            where TController : MonoBehaviour
        {
            var controllerType = typeof(TController);

            return m_infos.FirstOrDefault(c => c.Controller.GetType() == controllerType);
        }

        private CameraControllerInfo GetDisabledInfo<TController>()
            where TController : MonoBehaviour
        {
            var controllerType = typeof(TController);

            var info = m_disabledInfos.FirstOrDefault(c => c.Controller.GetType() == controllerType);

            if(info == null)
            {
                return GetInfo<TController>();
            }

            m_disabledInfos.Remove(info);

            return info;
        }

        private void DisableControllers(CameraControllerInfo info)
        {
            foreach(var c in m_infos)
            {
                if (info.Exclusive || c.Exclusive)
                {
                    if ((info.Kind & c.Kind) != 0)
                    {
                        c.Controller.enabled = false;
                    }
                }                
            }
        }
    }
}
