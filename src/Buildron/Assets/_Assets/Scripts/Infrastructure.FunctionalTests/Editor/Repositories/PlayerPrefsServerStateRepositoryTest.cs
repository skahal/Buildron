using NUnit.Framework;
using Buildron.Domain;
using System.Linq;
using Skahal.Infrastructure.Repositories;
using Buildron.Domain.Builds;

namespace Buildron.Infrastructure.FunctionalTests.Repositories
{
    [Category("Buildron.Infrastructure")]
	[Category("Unity")]
    public class PlayerPrefsServerStateRepositoryTest
    {
        [Test]
        public void Create_NewInstance_Created()
        {
			var target = new GenericPlayerPrefsRepository<ServerState>();
            target.Clear();
            
			var entity = new ServerState {
				CameraPositionZ = -10,
				BuildFilter = new BuildFilter {
					KeyWord = "test",
					FailedEnabled = true,
					QueuedEnabled = false,
					RunningEnabled = true,
					SuccessEnabled = true
				},
				HasHistory = true,
				IsShowingHistory = true,
				IsSorting = true,

			};
            target.Create(entity);

            Assert.AreNotEqual(0, entity.Id);            
            Assert.AreEqual(1, target.All().Count());
            var actual = target.All().First();
	        Assert.AreEqual(-10, actual.CameraPositionZ);
			Assert.AreEqual("test", actual.BuildFilter.KeyWord);

			Assert.IsTrue(actual.BuildFilter.FailedEnabled);
			Assert.IsFalse(actual.BuildFilter.QueuedEnabled);
			Assert.IsTrue(actual.BuildFilter.RunningEnabled);
			Assert.IsTrue(actual.HasHistory);
			Assert.IsTrue(actual.IsShowingHistory);
			Assert.IsTrue(actual.IsSorting);
        }
    }

}