using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Buildron.Infrastructure.BuildsProvider.Tfs.Models
{
    [Serializable]
    public class TfsBuildStep
    {
        public bool enabled;

        public string displayName;
    }
}
