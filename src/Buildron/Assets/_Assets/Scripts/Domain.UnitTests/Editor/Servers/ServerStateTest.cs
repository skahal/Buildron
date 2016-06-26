using Buildron.Domain;
using NUnit.Framework;
using UnityEngine;
using Buildron.Domain.Servers;

namespace Buildron.Domain.Server.UnitTests
{
    [Category("Buildron.Domain")]
    public class ServerStateTest
    {
        [Test]
		[Category("Unity")]
		public void GetCameraPosition_State_Position()
        {
			var target = new ServerState ();
			target.CameraPositionX = 1;
			target.CameraPositionY = 2;
			target.CameraPositionZ = 3;

			var actual = target.GetCameraPosition ();
			Assert.AreEqual (new Vector3 (1, 2, 3), actual);
        }

		[Test]
		[Category("Unity")]
		public void SetCameraPosition_Position_State()
		{
			var target = new ServerState ();
			target.SetCameraPosition (new Vector3 (1, 2, 3));

			Assert.AreEqual (1, target.CameraPositionX);
			Assert.AreEqual (2, target.CameraPositionY);
			Assert.AreEqual (3, target.CameraPositionZ);
		}
    }
}