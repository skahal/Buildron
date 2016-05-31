using System;
using Skahal.Infrastructure.Framework.Domain;

namespace Buildron.Domain
{
	#region Enums
	public enum KeyWordFilterType
	{
		Contains,
		DoesNotContains
	}
	#endregion
	
	[Serializable]
	public class BuildFilter : EntityBase, IAggregateRoot
	{
		#region Fields
		private string m_keyWord;
		#endregion
		
		#region Constructors
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
		public bool FailedEnabled { get; set; }
		public bool RunningEnabled { get; set; }
		public bool SuccessEnabled { get; set; }
		public bool QueuedEnabled { get; set; }

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