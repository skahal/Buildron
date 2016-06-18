using System;
using System.Collections.Generic;

namespace Buildron.Domain.EasterEggs
{
	/// <summary>
	/// Defines an easter egg provider interface.
	/// </summary>
	public interface IEasterEggProvider
	{
		/// <summary>
		/// Verify if the provider can execute the easter egg.
		/// </summary>
		/// <param name="easterEggName">The name of easter egg to execute</param>
		bool CanExecute(string easterEggName);

		/// <summary>
		/// Execute the easter egg.
		/// </summary>
		/// <param name="easterEggName">The name of easter egg to execute</param>
		/// <returns>If an easter egg was executed.</returns>
		bool Execute(string easterEggName);
	}
}
