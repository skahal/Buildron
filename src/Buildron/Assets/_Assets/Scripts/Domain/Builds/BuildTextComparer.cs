using System.Collections.Generic;

namespace Buildron.Domain.Builds
{
	/// <summary>
	/// Build text comparer.
	/// </summary>
	public class BuildTextComparer : IComparer<IBuild>
	{
		#region Methods
		/// <summary>
		/// Compare the specified x and y.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public int Compare (IBuild x, IBuild y)
		{
			return x.CompareTo (y);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Buildron.Domain.Builds.BuildTextComparer"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Buildron.Domain.Builds.BuildTextComparer"/>.</returns>
		public override string ToString ()
		{
			return "text";
		}
		#endregion		
	}
}