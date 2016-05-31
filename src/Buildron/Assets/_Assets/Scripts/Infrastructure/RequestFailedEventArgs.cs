#region Usings
using System;
using System.Collections;
#endregion

namespace Buildron.Infrastructure
{
	/// <summary>
	/// Request failed event arguments.
	/// </summary>
	public class RequestFailedEventArgs : EventArgs
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Infrastructure.RequestFailedEventArgs"/> class.
		/// </summary>
		/// <param name='url'>
		/// URL.
		/// </param>
		public RequestFailedEventArgs (string url)
		{
			Url = url;
		}
		#endregion
		
		#region Properties
		/// <summary>
		/// Gets the URL.
		/// </summary>
		/// <value>
		/// The URL.
		/// </value>
		public string Url { get; private set; }
		#endregion
	}
}