using System.Collections.Generic;

namespace Buildron.Domain.Builds
{
    public interface IBuildConfiguration
    {
        string Id { get; set; }
        string Name { get; set; }
        IBuildProject Project { get; set; }
        IList<IBuildStep> Steps { get; set; }
    }
}