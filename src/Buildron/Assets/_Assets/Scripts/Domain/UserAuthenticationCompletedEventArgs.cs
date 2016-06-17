using UnityEngine;
using System.Collections;
using System;

namespace Buildron.Domain
{
	public class UserAuthenticationCompletedEventArgs : UserEventArgsBase
	{
		#region Constructors
		public UserAuthenticationCompletedEventArgs(User user, bool success) 
			: base (user)
		{
            Success = success;
		}
        #endregion

        #region Properties
        public bool Success { get; private set; }
        #endregion
    }
}