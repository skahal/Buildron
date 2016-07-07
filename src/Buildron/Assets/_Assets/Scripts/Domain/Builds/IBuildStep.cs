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

    public interface IBuildStep
    {
        string Name { get; set; }
        BuildStepType StepType { get; set; }
    }
}