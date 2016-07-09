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

		IBuildConfiguration Configuration { get; set; }
		DateTime Date { get; set; } 
		string Id { get; set;}
        bool IsFailed { get; }
        bool IsQueued { get; }
        bool IsRunning { get; }
        bool IsSuccess { get; }
		string LastChangeDescription { get; set;}
		IBuildStep LastRanStep { get; set;}
		float PercentageComplete { get; set;}
        BuildStatus PreviousStatus { get; }
		int Sequence { get; set;}
		BuildStatus Status { get; set;}
		IUser TriggeredBy { get; set;}

        int CompareTo(IBuild other);

		/// <summary>
		/// Clones this instance.
		/// </summary>
		/// <returns>The clone.</returns>
		object Clone();
    }
}