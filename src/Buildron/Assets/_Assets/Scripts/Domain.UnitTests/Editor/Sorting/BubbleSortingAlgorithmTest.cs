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
	public class BubbleSortingAlgorithmTest
	{
		#region Tests
		[Test]
		public void Sort_Items_Sorted()
		{
			var target = new BubbleSortingAlgorithm<int> ();
			var sortingBeginRaised = target.CreateAssert<EventArgs> ("SortingBegin", 1);
			var sortingItemsSwappedRaised = target.CreateAssert<SortingItemsSwappedEventArgs<int>> ("SortingItemsSwapped", 20);
			var sortingEndedRaised = target.CreateAssert<EventArgs> ("SortingEnded", 1);

			var items = new List<int> (new int[] { 9, 1, 8, 2, 7, 3, 6, 4, 5 });
			var result = target.Sort (items, Comparer<int>.Default);
			while (result.MoveNext ())
				;

			var expectedItems = new List<int> (new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
			CollectionAssert.AreEqual (expectedItems, items);

			sortingBeginRaised.Assert ();
			sortingItemsSwappedRaised.Assert ();
			sortingEndedRaised.Assert ();
		}
		#endregion
	}
}