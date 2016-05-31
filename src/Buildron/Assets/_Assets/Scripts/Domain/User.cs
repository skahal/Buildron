#region Usings
using System;
#endregion

namespace Buildron.Domain
{
	/// <summary>
	/// Defines a abstract user entity.
	/// </summary>
	public abstract class User
	{
		#region Fields
		private string m_userName;
		#endregion
		
		#region Constructors
		protected User ()
		{
			m_userName = String.Empty;
			Password = String.Empty;
		}
		#endregion
		
		#region Properties
		/// <summary>
		/// Gets or sets the domain.
		/// </summary>
		public string Domain { get; set; }
		
		/// <summary>
		/// Gets the name of the domain and user.
		/// </summary>
		public string DomainAndUserName {
			get {
				return String.IsNullOrEmpty (Domain) ? UserName : Domain + @"\" + UserName;
			}
		}
		
		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		public string UserName {
			get {
				return m_userName;
			}
			
			set {
				SetUserName (value);
			}
		}
		
		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		public string Password { get; set; }
		#endregion
		
		#region Private methods
		private void SetUserName (string inputUserName)
		{
			if (inputUserName.Contains (@"\")) {
				var parts = inputUserName.Split ('\\');
				Domain = parts [0];
				m_userName = parts [1];
			} else {
				Domain = string.Empty;
				m_userName = inputUserName;
			}
		}
		#endregion
	}
}