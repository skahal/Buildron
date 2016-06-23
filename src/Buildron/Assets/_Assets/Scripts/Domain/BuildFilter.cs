using System;
using Skahal.Infrastructure.Framework.Domain;

namespace Buildron.Domain
{
	#region Enums
	/// <summary>
	/// Ke word filter type.
	/// </summary>
	public enum KeyWordFilterType
	{
		/// <summary>
		/// Should contains the keyword.
		/// </summary>
		Contains,

		/// <summary>
		/// Should not contains the keyword.
		/// </summary>
		DoesNotContains
	}
	#endregion

	/// <summary>
	/// Represents the build filter sent from a RC.
	/// </summary>
    [Serializable]
	public class BuildFilter : EntityBase, IAggregateRoot
	{
		#region Fields
		private string m_keyWord;
		#endregion
		
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Buildron.Domain.BuildFilter"/> class.
		/// </summary>
		public BuildFilter ()
		{
			FailedEnabled = true;
			RunningEnabled = true;
			SuccessEnabled = true;
			QueuedEnabled = true;
			KeyWord = string.Empty;
			KeyWordType = KeyWordFilterType.Contains;
		}
		#endregion
		
		#region Properties
		/// <summary>
		/// Gets or sets a value indicating whether the failed builds should be selected by the filter.
		/// </summary>
		public bool FailedEnabled { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the running builds should be selected by the filter.
		/// </summary>
		public bool RunningEnabled { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the success builds should be selected by the filter.
		/// </summary>
		public bool SuccessEnabled { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the queued builds should be selected by the filter.
		/// </summary>
		public bool QueuedEnabled { get; set; }

		/// <summary>
		/// Gets or sets the keyword.
		/// </summary>
		/// <value>The keyword.</value>
		public string KeyWord 
		{ 
			get
			{
				return m_keyWord;
			}
			
			set
			{
				if(!String.IsNullOrEmpty(value) && value.StartsWith("-"))
				{
					m_keyWord = value.Substring(1);
					KeyWordType = KeyWordFilterType.DoesNotContains;
				}
				else {
					m_keyWord = value;
					KeyWordType = KeyWordFilterType.Contains;
				}
			}
		}

		/// <summary>
		/// Gets the type of the keyword.
		/// </summary>
		/// <value>The type of the key word.</value>
		public KeyWordFilterType KeyWordType { get; private set ;}
		
		/// <summary>
		/// Gets a value indicating whether this instance is empty (has no filter).
		/// </summary>
		public bool IsEmpty {
			get {
				return string.IsNullOrEmpty (KeyWord) 
					&& FailedEnabled
					&& RunningEnabled
					&& SuccessEnabled
					&& QueuedEnabled;
			}
		}
		#endregion
	}
}