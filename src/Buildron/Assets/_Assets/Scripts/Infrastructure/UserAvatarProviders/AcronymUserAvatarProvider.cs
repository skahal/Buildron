using System;
using Buildron.Domain.Users;

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
			var backgroundColor = (user.Name.GetHashCode () & 0x00FFFFFF).ToString ().ToUpperInvariant ();

			return "http://dummyimage.com/256/{0}/fff&text={1}".With(backgroundColor, acronym);
        }
    }
}
