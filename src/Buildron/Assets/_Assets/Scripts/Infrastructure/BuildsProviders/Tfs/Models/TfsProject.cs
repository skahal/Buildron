using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Buildron.Infrastructure.BuildsProvider.Tfs.Models
{
    [Serializable]
    public class TfsProject : TfsObject
    {
        public Guid id;

        public string name;

		public string state;

		public int revision;
	}
}
