using Buildron.Domain;
using NUnit.Framework;
using Rhino.Mocks;
using Skahal.Logging;
using Skahal.Infrastructure.Framework.Repositories;
using System.Linq;
using Buildron.Domain.Sorting;

namespace Buildron.Domain.Servers.UnitTests
{
    [Category("Buildron.Domain")]
    public class ServerServiceTest
    {
		#region Fields
		private IRepository<ServerState> repository;
		private ISHLogStrategy log;
		#endregion

		#region Initialize
		[SetUp]
		public void InitializeTest()
		{
			repository = MockRepository.GenerateMock<IRepository<ServerState>> ();
			log = MockRepository.GenerateMock<ISHLogStrategy> ();
		}
		#endregion

		#region Tests
		[Test]
		public void SaveState_New_Created()
		{
			var state = new ServerState ();
			repository.Expect(r => r.All()).Return((new ServerState[0]).AsQueryable());
			repository.Expect (r => r.Create (state)).Return (state).WhenCalled (m =>
			{
				state.Id = 1;
			});
			var target = new ServerService (repository, log);

			target.SaveState (state);
			Assert.AreEqual (1, state.Id);

			var actual = target.GetState ();
			Assert.AreEqual (state.Id, actual.Id);

			repository.VerifyAllExpectations ();
		}

		[Test]
		public void SaveState_Existing_Updated()
		{
			var state = new ServerState  { Id = 1 };
			repository.Expect(r => r.All()).Return((new ServerState[] { state }).AsQueryable());
			repository.Expect (r => r.Modify (state));
			var target = new ServerService (repository, log);

			target.SaveState (state);
			Assert.AreEqual (1, state.Id);

			var actual = target.GetState ();
			Assert.AreEqual (state.Id, actual.Id);

			repository.VerifyAllExpectations ();
		}

		[Test]
		public void GetState_ThereIsNoState_New()
		{
			repository.Expect(r => r.All()).Return((new ServerState[0]).AsQueryable());
			var target = new ServerService (repository, log);

			var actual = target.GetState ();
			Assert.AreEqual (0, actual.Id);

			repository.VerifyAllExpectations ();
		}

		[Test]
		public void GetState_ThereIsState_Existing()
		{
			var state = new ServerState () { Id = 1 };
			repository.Expect(r => r.All()).Return((new ServerState[] { state }).AsQueryable());
			var target = new ServerService (repository, log);

			var actual = target.GetState ();
			Assert.AreEqual (1, actual.Id);

			repository.VerifyAllExpectations ();
		}
		#endregion
    }
}