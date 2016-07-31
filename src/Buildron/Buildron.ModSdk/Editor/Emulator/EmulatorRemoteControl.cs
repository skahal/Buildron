using System;
using Buildron.Domain.Builds;
using Buildron.Domain.Users;
using Skahal.Common;
using System.Collections.Generic;
using Buildron.Domain.RemoteControls;

public class EmulatorRemoteControl : IRemoteControl
{
	#region Fields

	private static int s_rcCount;

	#endregion

	#region Constructors

	public EmulatorRemoteControl ()
	{
		var id = (++s_rcCount).ToString ();
		UserName = "RC {0}".With (id);
	}

	#endregion

	#region IRemoteControl implementation

	public bool Connected { get; set; }

	#endregion

	#region IAuthUser implementation

	public string Domain { get; set; }

	public string DomainAndUserName  { get; set; }

	public string UserName  { get; set; }

	public string Password { get; set; }

	#endregion

	#region IEntity implementation

	public long Id  { get; set; }

	public bool IsNew  { get; set; }

	#endregion


}
