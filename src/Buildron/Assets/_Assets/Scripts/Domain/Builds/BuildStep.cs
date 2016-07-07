using Buildron.Domain.Mods;

namespace Buildron.Domain.Builds
{
	/// <summary>
	/// Build step.
	/// </summary>
	public class BuildStep : IBuildStep
    {
		#region Properties
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the type of the step.
		/// </summary>
		/// <value>The type of the step.</value>
		public BuildStepType StepType { get; set; }
		#endregion
	}
}