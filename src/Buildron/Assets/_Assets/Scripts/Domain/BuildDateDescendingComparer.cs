#region Usings
using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using Buildron.Domain;
#endregion

namespace Buildron.Domain
{
	/// <summary>
	/// Build date descending comparer.
	/// </summary>
	public class BuildDateDescendingComparer : IComparer<Build>
	{
		#region IComparer[Build] implementation
		/// <summary>
		/// Compare the specified x and y.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public int Compare (Build x, Build y)
		{
			return x.Date.CompareTo (y.Date) * -1;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Buildron.Domain.BuildDateDescendingComparer"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Buildron.Domain.BuildDateDescendingComparer"/>.</returns>
		public override string ToString()
		{
			return "date";
		}
		#endregion		
	}
}