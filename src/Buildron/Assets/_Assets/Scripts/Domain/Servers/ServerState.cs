using System;
using Buildron.Domain.Builds;
using Buildron.Domain.Sorting;
using Skahal.Infrastructure.Framework.Domain;
using UnityEngine;
using System.Runtime.Serialization;

namespace Buildron.Domain.Servers
{
	/// <summary>
	/// Represents the current Buildron server state.
	/// </summary>
	[Serializable]
	public sealed class ServerState : EntityBase, IAggregateRoot, ISerializable
    {
		#region Fields
		[NonSerialized]
		private Vector3 m_cameraPosition;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.Servers.ServerState"/> class.
		/// </summary>
		public ServerState ()
		{
			BuildFilter = new BuildFilter ();
		}

		protected ServerState(SerializationInfo info, StreamingContext context)
		{
			Id = info.GetInt32 ("Id");
			var cameraPositionX = info.GetSingle("CameraPositionX");
			var cameraPositionY= info.GetSingle("CameraPositionY");
			var cameraPositionZ = info.GetSingle("CameraPositionZ");
			m_cameraPosition = new Vector3 (cameraPositionX, cameraPositionY, cameraPositionZ);

	
			BuildFilter = (BuildFilter) info.GetValue ("BuildFilter", typeof(BuildFilter));
			BuildSortBy = (SortBy) info.GetValue ("BuildSortBy", typeof(SortBy));
			BuildSortingAlgorithmType = (SortingAlgorithmType) info.GetValue ("BuildSortingAlgorithmType", typeof(SortingAlgorithmType));
		}
		#endregion
		
		#region Properties
		/// <summary>
		/// Gets or sets the build filter.
		/// </summary>
		/// <value>The build filter.</value>
		public BuildFilter BuildFilter { get; set; }

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
		/// Gets or sets the camera position.
		/// </summary>
		/// <value>The camera position.</value>
		public Vector3 CameraPosition 
		{
			get
			{
				return m_cameraPosition;
			}

			set
			{
				m_cameraPosition = value;
			}
		}

		#region Non stored properties
		/// <summary>
		/// Gets or sets a value indicating whether this instance is sorting.
		/// </summary>
		/// <value><c>true</c> if this instance is sorting; otherwise, <c>false</c>.</value>
		public bool IsSorting { get; set; }

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
		#endregion
        #endregion

        #region Methods
		/// <summary>
		/// Gets the object data.
		/// </summary>
		/// <param name="info">Info.</param>
		/// <param name="context">Context.</param>
		public void GetObjectData (SerializationInfo info, StreamingContext context)
		{
			info.AddValue ("Id", Id);
			info.AddValue ("CameraPositionX", m_cameraPosition.x);
			info.AddValue ("CameraPositionY", m_cameraPosition.y);
			info.AddValue ("CameraPositionZ", m_cameraPosition.z);

			info.AddValue ("BuildFilter", BuildFilter);
			info.AddValue ("BuildSortBy", BuildSortBy);
			info.AddValue ("BuildSortingAlgorithmType", BuildSortingAlgorithmType);
		}
        #endregion
    }
}