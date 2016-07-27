using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Buildron.Domain.Mods
{
    public interface IPreferenceProxy : IDataProxy
    {
        void Register(params Preference[] preferences);
    }
}
