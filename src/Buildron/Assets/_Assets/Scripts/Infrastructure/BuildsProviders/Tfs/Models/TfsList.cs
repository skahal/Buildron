using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Buildron.Infrastructure.BuildsProvider.Tfs.Models
{
    [Serializable]
    public class TfsList<TItem>
	{
		public int count;

		public TItem[] value;
	}

}
