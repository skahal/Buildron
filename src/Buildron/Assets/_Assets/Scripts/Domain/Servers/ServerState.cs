using System;
using Buildron.Domain.Builds;
using Buildron.Domain.Sorting;
using Skahal.Infrastructure.Framework.Domain;
using UnityEngine;

namespace Buildron.Domain.Servers
{
	/// <summary>
	/// Represents the current Buildron server state.
	/// </summary>
	[Serializable]
	public sealed class ServerState : EntityBase, IAggregateRoot
    {
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Servers.ServerState"/> class.
		/// </summary>
		public ServerState ()
		{
			BuildFilter = new BuildFilter ();
            CameraPositionX = 0.0f;
            CameraPositionY = 3.6f;
            CameraPositionZ = -19.1f;
		}
		#endregion
		
		#region Properties
		/// <summary>
		/// Gets or sets the build filter.
		/// </summary>
		/// <value>The build filter.</value>
		public BuildFilter BuildFilter { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is sorting.
		/// </summary>
		/// <value><c>true</c> if this instance is sorting; otherwise, <c>false</c>.</value>
		public bool IsSorting { get; set; }

		/// <summary>
		/// Gets or sets the build sort by.
		/// </summary>
		/// <value>The build sort by.</value>
		public SortBy BuildSortBy { get; set; }

		/// <summary>
		/// Gets or sets the type of the build sorting algorithm.
		/// </summary>
		/// <value>The type of the build sorting algorithm.</value>
		public SortingAlgorithmType BuildSortingAlgorithmType { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance has history.
		/// </summary>
		/// <value><c>true</c> if this instance has history; otherwise, <c>false</c>.</value>
		public bool HasHistory { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is showing history.
		/// </summary>
		/// <value><c>true</c> if this instance is showing history; otherwise, <c>false</c>.</value>
		public bool IsShowingHistory { get; set; }

        /// <summary>
		/// Gets or sets the camera position X.
		/// </summary>   
		/// <value>The camera position X.</value>
		public float CameraPositionX { get; set; }

        /// <summary>
		/// Gets or sets the camera position Y.
		/// </summary>
		/// <value>The camera position Y.</value>
		public float CameraPositionY { get; set; }

        /// <summary>
        /// Gets or sets the camera position Z.
        /// </summary>
        /// <value>The camera position Z.</value>
        public float CameraPositionZ { get; set; }
        #endregion

        #region Methods        
        /// <summary>
        /// Gets the camera position.
        /// </summary>
        /// <remarks>
        /// Vector3 is not serializable.
        /// </remarks>
        /// <returns>The camera position.</returns>
        public Vector3 GetCameraPosition()
        {
            return new Vector3(CameraPositionX, CameraPositionY, CameraPositionZ);
        }

        /// <summary>
        /// Sets the camera position.
        /// </summary>
        /// <remarks>
        /// Vector3 is not serializable.
        /// </remarks>
        /// <param name="position">The position.</param>
        public void SetCameraPosition(Vector3 position)
        {
            CameraPositionX = position.x;
            CameraPositionY = position.y;
            CameraPositionZ = position.z;
        }
        #endregion
    }
}