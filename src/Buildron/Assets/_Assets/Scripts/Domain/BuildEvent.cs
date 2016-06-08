using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Buildron.Domain
{
    public sealed class BuildEvent
    {
        public BuildEvent(Build build)
        {
            Build = build;
        }

        public Build Build { get; private set; }

        public bool Canceled { get; set; }
    }
}
