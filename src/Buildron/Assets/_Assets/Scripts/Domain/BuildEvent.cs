using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Buildron.Domain
{
	/// <summary>
	/// Represents an build event used by IBuildInterceptors.
	/// </summary>
    public sealed class BuildEvent
    {
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.BuildEvent"/> class.
		/// </summary>
		/// <param name="build">Build.</param>
        public BuildEvent(Build build)
        {
            Build = build;
        }
		#endregion

		#region 

		#region Properties
		/// <summary>
		/// Gets the build.
		/// </summary>
		/// <value>The build.</value>
        public Build Build { get; private set; }

		/// <summary>
		/// Gets or sets a value indicating whether the event was canceled.
		/// </summary>
		/// <value><c>true</c> if this instance canceled; otherwise, <c>false</c>.</value>
        public bool Canceled { get; set; }
		#endregion
    }
}
