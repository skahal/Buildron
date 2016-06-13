using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buildron.Domain;
using UnityEngine;
using UnityEngine.UI;

namespace Buildron.Infrastructure.UserAvatarProviders
{
    /// <summary>
    /// Uses http://dummyimage.com/ to generante a acronym user avatar.
    /// </summary>
    public class AcronymUserAvatarProvider : WebUserAvatarProviderBase
    {
        protected override string BuildImageUrl(User user)
        {
            var acronym = user.Name.ToAcronym();

            return "http://dummyimage.com/256/000/fff&text={0}".With(acronym);
        }
    }
}
