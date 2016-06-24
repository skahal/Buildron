#region Usings
using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using Buildron.Domain;
#endregion

namespace Buildron.Domain
{
	public class BuildTextComparer : IComparer<Build>
	{
		#region IComparer[Build] implementation
		public int Compare (Build x, Build y)
		{
			return x.CompareTo (y);
		}
		
		public override string ToString ()
		{
			return "text";
		}
		#endregion		
	}
}