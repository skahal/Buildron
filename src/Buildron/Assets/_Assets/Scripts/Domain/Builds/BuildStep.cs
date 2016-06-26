namespace Buildron.Domain.Builds
{
	#region Enums
	/// <summary>
	/// Build step type.
	/// </summary>
	public enum BuildStepType
	{
		/// <summary>
		/// None.
		/// </summary>
		None = 0,

		/// <summary>
		/// Build step is compilation.
		/// </summary>
		Compilation = 1,

		/// <summary>
		/// Build step is unit test.
		/// </summary>
		UnitTest = 2,

		/// <summary>
		/// Build step is code analysis.
		/// </summary>
		CodeAnalysis = 3,

		/// <summary>
		/// Build step is duplication finder.
		/// </summary>
		CodeDuplicationFinder = 4,

		/// <summary>
		/// Build step is deploy.
		/// </summary>
		Deploy = 5,

		/// <summary>
		/// Build step is statistics.
		/// </summary>
		Statistics = 6,

		/// <summary>
		/// Build step is package publishing.
		/// </summary>
		PackagePublishing = 7
	}
	#endregion

	/// <summary>
	/// Build step.
	/// </summary>
	public class BuildStep
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