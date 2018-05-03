using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Buildron.Infrastructure.BuildsProvider.Tfs.Models
{
    [Serializable]
    public class TfsBuild : TfsObject
    {
        public int id;

        public string status;

        public TfsBuildStatus Status { get { return status.ToEnum<TfsBuildStatus>(); } }

        public string result;

        public TfsBuildResult Result { get { return result.ToEnum<TfsBuildResult>(); } }

        public string queueTime;

        public DateTime QueueTime { get { return DateTime.Parse(queueTime); } }

        public string startTime;

        public DateTime StartTime { get { return DateTime.Parse(startTime); } }

        public string finishTime;

        public DateTime FinishTime { get { return DateTime.Parse(finishTime); } }

        public TfsUser requestedFor;

        public TfsUser requestedBy;

        public TfsUser lastChangedBy;

        public string sourceBranch;

        public string sourceVersion;

        public TfsRepository repository;
    }
}
