using System;
using Buildron.Domain.Users;

namespace Buildron.Domain.Builds
{
    #region Enums    
    /// <summary>
    /// Build status.
    /// </summary>
    public enum BuildStatus
    {
        /// <summary>
        /// Build is in an unknown status.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Build has finish with success.
        /// </summary>
        Success,

        /// <summary>
        /// Build is in error status.
        /// </summary>
        Error,

        /// <summary>
        /// Build has failed
        /// </summary>
        Failed,

        /// <summary>
        /// Build has been canceled.
        /// </summary>
        Canceled,

        /// <summary>
        /// Build has been queued.
        /// </summary>
        Queued,

        /// <summary>
        /// Build is running.
        /// </summary>
        Running,

        /// <summary>
        /// Build is running unit tests.
        /// </summary>
		RunningUnitTests,

        /// <summary>
        /// Build is running functional tests.
        /// </summary>
		RunningFunctionalTests,

        /// <summary>
        /// Build is running duplicates finder.
        /// </summary>
		RunningDuplicatesFinder,

        /// <summary>
        /// Build is running code analysis.
        /// </summary>
		RunningCodeAnalysis,

        /// <summary>
        /// Build is running deploy.
        /// </summary>
		RunningDeploy
    }
    #endregion

	/// <summary>
	/// Defines an interface to a build.
	/// </summary>
	public interface IBuild : IComparable<IBuild>, ICloneable 
    {
		#region Events        
		/// <summary>
		/// Occurs when status has changed.
		/// </summary>
		event EventHandler<BuildStatusChangedEventArgs> StatusChanged;

		/// <summary>
		/// Occurs when triggered by has changed.
		/// </summary>
		event EventHandler<BuildTriggeredByChangedEventArgs> TriggeredByChanged;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the configuration.
		/// </summary>
		IBuildConfiguration Configuration { get; set; }

		/// <summary>
		/// Gets or sets the date.
		/// </summary>
		DateTime Date { get; set; } 

		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		string Id { get; set;}

		/// <summary>
		/// Gets a value indicating whether the build has failed (Canceled | Error | Failed)
		/// </summary>
        bool IsFailed { get; }
        
		/// <summary>
		/// Gets a value indicating whether the build has been queued.
		/// </summary>
		bool IsQueued { get; }

		/// <summary>
		/// Gets a value indicating whether the build is running.
		/// </summary>
        bool IsRunning { get; }

		/// <summary>
		/// Gets a value indicating whether the build is finish with success.
		/// </summary>
        bool IsSuccess { get; }

		/// <summary>
		/// Gets or sets the last change description.
		/// </summary>
		string LastChangeDescription { get; set;}

		/// <summary>
		/// Gets or sets the last ran step.
		/// </summary>
		IBuildStep LastRanStep { get; set;}

		/// <summary>
		/// Gets or sets the percentage complete.
		/// </summary>
		float PercentageComplete { get; set;}

		/// <summary>
		/// Gets the previous status.
		/// </summary>
        BuildStatus PreviousStatus { get; }

		/// <summary>
		/// Gets or sets the sequence.
		/// </summary>
		int Sequence { get; set;}

		/// <summary>
		/// Gets or sets the status.
		/// </summary>	
		BuildStatus Status { get; set;}

		/// <summary>
		/// Gets or sets the user that triggered the build.
		/// </summary>
		IUser TriggeredBy { get; set;}
		#endregion
    }
}