using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Buildron.Infrastructure.BuildsProvider.Tfs.Models
{
    public static class Conversion
    {
        public static TEnum ToEnum<TEnum>(this string value)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }

        public static DateTime ToDateTime(this string value)
        {
            return DateTime.Parse(value);
        }

        public static bool ContainsGitCommits(this TfsBuild tfsBuild)
        {
            return
                tfsBuild.repository != null &&
                tfsBuild.sourceVersion != null &&
                "TfsGit".Equals(tfsBuild.repository.type, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
