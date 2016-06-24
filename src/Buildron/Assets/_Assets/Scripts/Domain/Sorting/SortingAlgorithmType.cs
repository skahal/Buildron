namespace Buildron.Domain.Sorting
{
	/// <summary>
	/// Sorting algorithm types.
	/// </summary>
	public enum SortingAlgorithmType 
	{
        /// <summary>
        /// No sorting at all.
        /// </summary>
        None = 0,

		/// <summary>
		/// Insertion sort.
		/// </summary>
		Insertion,

        /// <summary>
        /// Selection sort.
        /// </summary>
        Selection,

        /// <summary>
        /// Shell sort.
        /// </summary>
        Shell,

        /// <summary>
        /// Bubble sort.
        /// </summary>
        Bubble
    }

    /// <summary>
    /// The kinds of sort by.
    /// </summary>
    public enum SortBy
	{
        /// <summary>
        /// Sort by text.
        /// </summary>
        Text,

        /// <summary>
        /// Sort by date.
        /// </summary>
        Date,

        /// <summary>
        /// Sort by relevant status.
        /// </summary>
        RelevantStatus
    }
}