using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buildron.Domain;
using UnityEngine;
using UnityEngine.UI;

namespace Buildron.Infrastructure.BuildUserAvatarProviders
{
    /// <summary>
    /// Uses http://dummyimage.com/ to generante a acronym build user avatar.
    /// </summary>
    public class AcronymBuildUserAvatarProvider : WebBuildUserAvatarProviderBase
    {
        protected override string BuildImageUrl(BuildUser user)
        {
            var acronym = user.Name.ToAcronym();

            return "http://dummyimage.com/256/000/fff&text={0}".With(acronym);
        }
    }
}
