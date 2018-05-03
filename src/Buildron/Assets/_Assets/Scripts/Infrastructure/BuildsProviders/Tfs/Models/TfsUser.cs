using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Buildron.Infrastructure.BuildsProvider.Tfs.Models
{
    [Serializable]
    public class TfsUser : TfsObject
    {
        public Guid id;

        public string displayName;

        public string uniqueName;

        public string imageUrl;
    }
}
