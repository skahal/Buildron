#region Usings
using System.Collections.Generic;
#endregion

namespace Buildron.Domain.Sorting
{
	/// <summary>
	///  sorting algorithm.
	/// </summary>
	public interface ISortingAlgorithm<TItem> where TItem : System.IComparable<TItem>
	{
		string Name { get; }
		void Sort(IList<TItem> items, IComparer<TItem> equalityComparer);
	}
}