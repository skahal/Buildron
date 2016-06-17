using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buildron.Domain;
using UnityEngine;
using Zenject;
using Skahal;

namespace Buildron.Application
{
    /// <summary>
    /// Base class to Game Object application services.
    /// </summary>
    public abstract class GOServiceBase<TModel, TController>  
		where TController : SHController<TModel>
    {
        #region Constructors
        protected GOServiceBase (IFactory<TController> factory)
        {
            Factory = factory;
        }
        #endregion

        #region Properties
        protected IFactory<TController> Factory { get; private set; }
        #endregion

        #region Methods
        protected abstract string GetName(TModel model);

        public virtual GameObject GetGameObject(TModel model)
        {
            return GameObject.Find(GetName(model));
        }

        public virtual bool ExistsGameObject(TModel model)
        {
            return GetGameObject(model) != null;
        }

        public virtual GameObject CreateGameObject(TModel model)
        {
            var go = GetGameObject(model);

            if (go == null)
            {
                var build = Factory.Create();
                build.Model = model;
                go = build.gameObject;
                go.name = GetName(model);
            }

            return go;
        }
        #endregion
    }
}
