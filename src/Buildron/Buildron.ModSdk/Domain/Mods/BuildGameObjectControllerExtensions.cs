using System;
using System.Linq;
using UnityEngine;
using Buildron.Domain.Mods;
using System.Collections.Generic;
using Buildron.Domain.Builds;

public static class BuildGameObjectControllerExtensions
{	
    public static GameObject GetGameObject (this IBuildController[] builds, IBuild build)
    {
        var controller = builds.FirstOrDefault(b => b.Model == build);

        return controller == null ? null : controller.gameObject;
    }
}
