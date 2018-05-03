using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Buildron.Infrastructure.BuildsProvider.Tfs.Models
{
    [Serializable]
    public class TfsGitUser
    {
        public string name;

        public string email;

        public string date;

        public DateTime Date { get { return date.ToDateTime(); } }
    }
}
