using System;

namespace Buildron.Domain.Mods
{
	/// <summary>
	/// Define an interface to thread proxy.
	/// </summary>
	public interface IThreadProxy
	{
		/// <summary>
		/// Runs the action on main thread.
		/// </summary>
		/// <param name="action">Action.</param>
		void RunOnMainThread(Action action);
	}
}