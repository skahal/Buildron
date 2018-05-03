using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Buildron.Infrastructure.BuildsProvider.Tfs.Models
{
    [Serializable]
    public class TfsDefinition : TfsObject
    {
        public int id;

        public string name;

        public TfsProject project;

        public string quality;

        public int revision;

        public TfsUser authoredBy;

        public string createdDate;

        public DateTime CreatedDate { get { return DateTime.Parse(createdDate); } }

        public TfsBuildStep[] build;
    }
}
