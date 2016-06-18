using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Buildron.Domain.EasterEggs;
using Rhino.Mocks;
using System.Collections.Generic;
using Skahal.Logging;

namespace Buildron.Domain.UnitTests
{
	[Category ("Buildron.Domain")]
	public class EasterEggServiceTest
	{
		#region Fields
		private EasterEggService m_target;
		private IEasterEggProvider m_p1;
		private IEasterEggProvider m_p2;
		#endregion

		#region Initialize
		[SetUp]
		public void InitializeTest()
		{
			var providers = new List<IEasterEggProvider> ();
			m_p1 = MockRepository.GenerateMock<IEasterEggProvider> ();
			m_p1.Expect (p => p.CanExecute ("e1")).Return (true);
			m_p1.Expect (p => p.CanExecute ("e2")).Return (false);
			m_p1.Expect (p => p.CanExecute ("e3")).Return (true);

			m_p2 = MockRepository.GenerateMock<IEasterEggProvider> ();
			m_p2.Expect (p => p.CanExecute ("e1")).Return (false);
			m_p2.Expect (p => p.CanExecute ("e2")).Return (true);
			m_p2.Expect (p => p.CanExecute ("e3")).Return (true);
			providers.Add (m_p1);
			providers.Add (m_p2);

			var log = MockRepository.GenerateMock<ISHLogStrategy> ();
			m_target = new EasterEggService (providers, log);
		}
		#endregion

		#region Tests
		[Test]
		public void IsEasterEggMessage_NoStartsWithDash_False ()
		{
			Assert.IsFalse (m_target.IsEasterEggMessage ("e1/"));
			Assert.IsFalse (m_target.IsEasterEggMessage ("e1"));
		}

		[Test]
		public void IsEasterEggMessage_StartsWithDashAndProviderCanExecute_True()
		{
			Assert.IsTrue (m_target.IsEasterEggMessage ("/e1"));
			Assert.IsTrue (m_target.IsEasterEggMessage ("/e2"));
			Assert.IsTrue (m_target.IsEasterEggMessage ("/e3"));
		}

		[Test]
		public void ReceiveEasterEgg_IsNotEasterEggMessage_false()
		{
			m_p1.Expect (p => p.Execute ("/e1")).Return (true);
			m_p2.Expect (p => p.Execute ("/e1")).Return (false);

			m_p1.Expect (p => p.Execute ("/e2")).Return (false);
			m_p2.Expect (p => p.Execute ("/e2")).Return (true);

			m_p1.Expect (p => p.Execute ("/e3")).Return (true);
			m_p2.Expect (p => p.Execute ("/e3")).Return (true);

			Assert.IsTrue (m_target.ReceiveEasterEgg ("/e1"));
			Assert.IsTrue (m_target.ReceiveEasterEgg ("/e2"));
			Assert.IsTrue (m_target.ReceiveEasterEgg ("/e3"));
		}
		#endregion
	}
}