using System;
using System.Collections.Generic;
using System.Linq;
using Buildron.Domain.EasterEggs;
using Buildron.Domain.Sorting;
using Buildron.Domain.Versions;
using NUnit.Framework;
using Rhino.Mocks;
using Skahal.Logging;
using UnityEditor;
using UnityEngine;

namespace Buildron.Domain.UnitTests.Sorting
{
	[Category ("Buildron.Domain")]
	public class SortingAlgorithmBaseTest
	{
        #region Tests
        [Test]
        public void Sort_ItemsAlreadySorted_EventArgsWasAlreadySorted()
        {
            var target = new ShellSortingAlgorithm<int>();
            var sortingBeginRaised = target.CreateAssert<SortingBeginEventArgs>("SortingBegin", 1);
            var sortingItemsSwappedRaised = target.CreateAssert<SortingItemsSwappedEventArgs<int>>("SortingItemsSwapped", 0);
            var sortingEndedRaised = target.CreateAssert<SortingEndedEventArgs>("SortingEnded", 1);

            var items = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            var result = target.Sort(items, Comparer<int>.Default);
            while (result.MoveNext())
            {
            }

            var expectedItems = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            CollectionAssert.AreEqual(expectedItems, items);            

            sortingBeginRaised.Assert();
            sortingItemsSwappedRaised.Assert();
            sortingEndedRaised.Assert();
        }
        #endregion
    }
}