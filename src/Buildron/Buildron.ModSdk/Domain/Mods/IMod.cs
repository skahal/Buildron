namespace Buildron.Domain.Mods
{
    /// <summary>
    /// Defines an interface for a Buildron's mod.
    /// </summary>
    public interface IMod	
	{    
		/// <summary>
		/// Initialize the mod with the context.
		/// </summary>
		/// <param name="context">The mod context.</param>
		void Initialize(IModContext context);

        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }
	}
}
