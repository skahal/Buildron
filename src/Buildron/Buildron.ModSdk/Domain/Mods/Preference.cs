using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skahal.Common;

namespace Buildron.Domain.Mods
{
    public enum PreferenceKind
    {
        String,
        Int,
        Float,
        Bool
    }

    public class Preference
    {
        public Preference(string name)
            : this(name, name)
        {            
        }

        public Preference(string name, string title)
            : this(name, title, PreferenceKind.String)
        {
        }

        public Preference(string name, string title, PreferenceKind kind)
            : this(name, title, kind, null)
        {
        }

        public Preference(string name, string title, PreferenceKind kind, object defaultValue)
        {
            Throw.AnyNull(new { name, title, kind });

            Name = name;
            Title = title;
            Kind = kind;
            DefaultValue = defaultValue;
        }

        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public PreferenceKind Kind { get; set; }
        public object DefaultValue { get; set; }
    }
}
