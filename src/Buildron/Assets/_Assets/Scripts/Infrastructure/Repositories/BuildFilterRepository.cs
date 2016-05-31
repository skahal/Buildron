#region Usings
using UnityEngine;
using System.Xml;
using System;
using Skahal.Logging;
using System.Collections.Generic;
using Buildron.Domain;
using Skahal.Infrastructure.Repositories;

#endregion

namespace Buildron.Infrastructure.Repositories
{
	public class BuildFilterRepository : PlayerPrefsRepositoryBase<BuildFilter>, IBuildFilterRepository
	{

	}
}