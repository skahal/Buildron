using Buildron.Domain.Sorting;
using NUnit.Framework;
using Buildron.Domain.Builds;

namespace Buildron.Domain.UnitTests.Builds
{
    [Category("Buildron.Domain")]
    public class BuildComparerFactoryTest
    {     
        [Test]
        public void Create_SortByDate_DateComparer()
        {
            Assert.IsInstanceOf<BuildDateDescendingComparer>(BuildComparerFactory.Create(SortBy.Date));
        }

        [Test]
        public void Create_SortByText_TextComparer()
        {
            Assert.IsInstanceOf<BuildTextComparer>(BuildComparerFactory.Create(SortBy.Text));
        }

        [Test]
        public void Create_SortByRelevantStatus_RelevantStatusComparer()
        {
            Assert.IsInstanceOf<BuildMostRelevantStatusComparer>(BuildComparerFactory.Create(SortBy.RelevantStatus));
        }
    }
}