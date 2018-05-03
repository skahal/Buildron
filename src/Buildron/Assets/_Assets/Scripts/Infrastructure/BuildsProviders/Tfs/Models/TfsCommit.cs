using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Buildron.Infrastructure.BuildsProvider.Tfs.Models
{
    [Serializable]
    public class TfsCommit
    {
        public string commitId;

        public string comment;

        public TfsGitUser author;

        public TfsGitUser committer;
    }
}
