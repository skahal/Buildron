using Buildron.Domain;
using NUnit.Framework;
using UnityEngine;
using Buildron.Domain.Servers;
using System.Reflection;
using System.Runtime.Serialization;
using Skahal.Serialization;
using Buildron.Domain.Builds;
using Buildron.Domain.Sorting;

namespace Buildron.Domain.UnitTests.Server
{
    [Category("Buildron.Domain")]
    public class ServerStateTest
    {
        [Test]
		[Category("Unity")]
		public void Constructor_SerializationInfo_PropertiesLoaed()
        {
			var target = new ServerState ();
			target.CameraPosition = new Vector3 (1.1f, 2.2f, 3.3f);
			target.BuildFilter = new BuildFilter {
				KeyWord = "teste"
			};
			target.BuildSortBy = SortBy.Text;
			target.BuildSortingAlgorithmType = SortingAlgorithmType.Selection;

			var bytes = SHSerializer.SerializeToBytes (target);

			var actual = SHSerializer.DeserializeFromBytes<ServerState> (bytes);
			Assert.AreEqual (1.1f, actual.CameraPosition.x);
			Assert.AreEqual (2.2f, actual.CameraPosition.y);
			Assert.AreEqual (3.3f, actual.CameraPosition.z);

			Assert.AreEqual ("teste", actual.BuildFilter.KeyWord);
			Assert.AreEqual (SortBy.Text, actual.BuildSortBy);
			Assert.AreEqual (SortingAlgorithmType.Selection, actual.BuildSortingAlgorithmType);
		}
    }
}