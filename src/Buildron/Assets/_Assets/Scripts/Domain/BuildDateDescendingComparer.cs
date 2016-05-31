#region Usings
using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using Buildron.Domain;
#endregion

namespace Buidron.Domain
{
	public class BuildDateDescendingComparer : IComparer<Build>
	{
		#region IComparer[Build] implementation
		public int Compare (Build x, Build y)
		{
			return x.Date.CompareTo (y.Date) * -1;
		}
		
		public override string ToString()
		{
			return "date";
		}
		#endregion
		
	}
}