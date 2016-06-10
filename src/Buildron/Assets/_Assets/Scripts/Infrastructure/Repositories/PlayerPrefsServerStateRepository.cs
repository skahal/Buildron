using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buildron.Domain;
using Skahal.Infrastructure.Repositories;

namespace Buildron.Infrastructure.Repositories
{
    public class PlayerPrefsServerStateRepository : PlayerPrefsRepositoryBase<ServerState>, IServerStateRepository
    {
    }
}
