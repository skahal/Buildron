using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Buildron.Domain.EasterEgss;

namespace Buildron.Domain.UnitTests
{
	[Category ("Buildron.Domain")]
	public class EasterEggServiceTest
	{
		[Test]
		public void IsEasterEggMessage_NoStartsWithDash_False ()
		{
			var target = new EasterEggService ();
			Assert.IsFalse (target.IsEasterEggMessage ("teste/"));
		}

		[Test]
		public void IsEasterEggMessage_StartsWithDash_True()
		{
			var target = new EasterEggService ();
			Assert.IsTrue (target.IsEasterEggMessage ("/teste"));
		}
	}
}