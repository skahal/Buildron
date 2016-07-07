using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skahal.Logging;
using Buildron.Domain.Builds;
using Buildron.Domain.CIServers;
using Buildron.Domain.RemoteControls;
using Buildron.Domain.Users;

namespace Buildron.Domain.Mods
{
    public interface IModLoader
    {
		void Initialize ();
    }
}
