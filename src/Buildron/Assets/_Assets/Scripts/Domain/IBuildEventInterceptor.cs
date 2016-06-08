using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Buildron.Domain
{
    public interface IBuildEventInterceptor
    {
        void OnStatusChanged(BuildEvent buildEvent);
        void OnTriggeredByChanged(BuildEvent buildEvent);
    }
}
