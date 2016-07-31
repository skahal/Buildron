using NUnit.Framework;
using System.Collections.Generic;
using Buildron.Domain.Sorting;
using System;
using System.Linq;

namespace Buildron.Domain.UnitTests.Sorting
{
	[Category ("Buildron.Domain")]
	public class SortingAlgorithmFactoryTest
	{
		#region Tests
		[Test]
		[Category ("Unity")]
		public void CreateRandomSortingAlgorithm_NoArgs_Random ()
		{
			var results = new List<ISortingAlgorithm<int>>();

			for (int i = 0; i < 100; i++)
			{
				results.Add(SortingAlgorithmFactory.CreateRandomSortingAlgorithm<int> ());
			}

			Assert.AreEqual (
				Enum.GetNames (typeof(SortingAlgorithmType)).Length, 
				results.GroupBy (r => r.GetType ()).Count ());
		}

		[Test]
		public void CreateSortingAlgorithm_Type_Instance ()
		{
			var bubble = SortingAlgorithmFactory.CreateSortingAlgorithm<int> (SortingAlgorithmType.Bubble);
			Assert.IsTrue (bubble is BubbleSortingAlgorithm<int>);

			var insertion = SortingAlgorithmFactory.CreateSortingAlgorithm<int> (SortingAlgorithmType.Insertion);
			Assert.IsTrue (insertion is InsertionSortingAlgorithm<int>);

			var none = SortingAlgorithmFactory.CreateSortingAlgorithm<int> (SortingAlgorithmType.None);
			Assert.IsTrue (none is NoneSortingAlgorithm<int>);

			var selection = SortingAlgorithmFactory.CreateSortingAlgorithm<int> (SortingAlgorithmType.Selection);
			Assert.IsTrue (selection is SelectionSortingAlgorithm<int>);

			var shell = SortingAlgorithmFactory.CreateSortingAlgorithm<int> (SortingAlgorithmType.Shell);
			Assert.IsTrue (shell is ShellSortingAlgorithm<int>);
		}

		[Test]
		public void GetAlgorithmType_Instance_Type ()
		{
			var type = SortingAlgorithmFactory.GetAlgorithmType (new BubbleSortingAlgorithm<int>());
			Assert.AreEqual (type, SortingAlgorithmType.Bubble);

			type = SortingAlgorithmFactory.GetAlgorithmType (new InsertionSortingAlgorithm<int>());
			Assert.AreEqual (type, SortingAlgorithmType.Insertion);

			type = SortingAlgorithmFactory.GetAlgorithmType (new NoneSortingAlgorithm<int>());
			Assert.AreEqual (type, SortingAlgorithmType.None);

			type = SortingAlgorithmFactory.GetAlgorithmType (new SelectionSortingAlgorithm<int>());
			Assert.AreEqual (type, SortingAlgorithmType.Selection);

			type = SortingAlgorithmFactory.GetAlgorithmType (new ShellSortingAlgorithm<int>());
			Assert.AreEqual (type, SortingAlgorithmType.Shell);
		}
		#endregion
	}
}