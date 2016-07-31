using System;
using Buildron.Domain.Users;

namespace Buildron.Domain.Mods
{
	public interface IUserController : IGameObjectController
	{
		IUser Model { get;  }
	}
}
