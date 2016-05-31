#region Usings
#endregion

namespace Buildron.Domain
{
	#region Enums
	public enum BuildStepType
	{
		None = 0,
		Compilation = 1,
		UnitTest = 2,
		CodeAnalysis = 3,
		CodeDuplicationFinder = 4,
		Deploy = 5,
		Statistics = 6,
		PackagePublishing = 7
	}
	#endregion
	
	public class BuildStep
	{
		#region Properties
		public string Name { get; set; }
		public BuildStepType StepType { get; set; }
		#endregion
	}
}