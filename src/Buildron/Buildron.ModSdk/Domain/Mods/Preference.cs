using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skahal.Common;

namespace Buildron.Domain.Mods
{
	/// <summary>
	/// The kind of preference that a mod can expose to an user.
	/// </summary>
    public enum PreferenceKind
    {
		/// <summary>
		/// String value.
		/// </summary>
        String,

		/// <summary>
		/// Integer value.
		/// </summary>
        Int,

		/// <summary>
		/// Float value.
		/// </summary>
        Float,

		/// <summary>
		/// Boolean value.
		/// </summary>
        Bool
    }

	/// <summary>
	/// Represents a preference that a mod can expose to an user.
	/// </summary>
	public class Preference
    {
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Buildron.Domain.Mods.Preference"/> class.
		/// </summary>
		/// <param name="name">The preference name.</param>
        public Preference(string name)
            : this(name, name)
        {            
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Buildron.Domain.Mods.Preference"/> class.
		/// </summary>
		/// <param name="name">The preference name.</param>
		/// <param name="title">The preference title. This value will be shown to the user.</param>
		public Preference(string name, string title)
            : this(name, title, PreferenceKind.String)
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Buildron.Domain.Mods.Preference"/> class.
		/// </summary>
		/// <param name="name">The preference name.</param>
		/// <param name="title">The preference title. This value will be shown to the user.</param>
		/// <param name="kind">The preference kind.</param>
		public Preference(string name, string title, PreferenceKind kind)
            : this(name, title, kind, null)
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Buildron.Domain.Mods.Preference"/> class.
		/// </summary>
		/// <param name="name">The preference name.</param>
		/// <param name="title">The preference title. This value will be shown to the user.</param>
		/// <param name="kind">The preference kind.</param>
		/// <param name="defaultValue">The default value.</param>
		public Preference(string name, string title, PreferenceKind kind, object defaultValue)
        {
            Throw.AnyNull(new { name, title, kind });

            Name = name;
            Title = title;
            Kind = kind;
            DefaultValue = defaultValue;
        }
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
        public string Name { get; set; }

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>The title.</value>
        public string Title { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>The description.</value>
        public string Description { get; set; }

		/// <summary>
		/// Gets or sets the kind.
		/// </summary>
		/// <value>The kind.</value>
        public PreferenceKind Kind { get; set; }

		/// <summary>
		/// Gets or sets the default value.
		/// </summary>
		/// <value>The default value.</value>
        public object DefaultValue { get; set; }
		#endregion
    }
}
