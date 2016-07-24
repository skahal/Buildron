using System;

namespace Buildron.Domain.Mods
{
	public interface IUserGameObjectsProxy
	{
		IUserController[] GetAll();
	}
}
