using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Buildron.Domain.EasterEggs;
using Rhino.Mocks;
using System.Collections.Generic;
using Skahal.Logging;
using Buildron.Domain.Sorting;
using Buildron.Domain.Versions;
using System;
using System.Linq;

namespace Buildron.Domain.UnitTests.Sorting
{
	[Category ("Buildron.Domain")]
	public class NoneSortingAlgorithmTest
	{
		#region Tests
		[Test]
		public void Sort_Items_Sorted()
		{
			var target = new NoneSortingAlgorithm<int> ();
			var sortingBeginRaised = target.CreateAssert<SortingBeginEventArgs> ("SortingBegin", 1);
			var sortingItemsSwappedRaised = target.CreateAssert<SortingItemsSwappedEventArgs<int>> ("SortingItemsSwapped", 0);
			var sortingEndedRaised = target.CreateAssert<SortingEndedEventArgs> ("SortingEnded", 1);

			var items = new List<int> (new int[] { 9, 1, 8, 2, 7, 3, 6, 4, 5 });
			var result = target.Sort (items, Comparer<int>.Default);
			while (result.MoveNext ());

			var expectedItems = new List<int> (new int[] { 9, 1, 8, 2, 7, 3, 6, 4, 5 });
			CollectionAssert.AreEqual (expectedItems, items);

			sortingBeginRaised.Assert ();
			sortingItemsSwappedRaised.Assert ();
			sortingEndedRaised.Assert ();
		}

		#endregion
	}
}