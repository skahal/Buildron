using System;
using Buildron.Domain.Mods;

public class SimulatorFileSystemProxy : IFileSystemProxy
	{
		public SimulatorFileSystemProxy ()
		{
		}

	#region IFileSystemProxy implementation

	public string[] GetFiles (string path, string searchPattern, bool recursive)
	{
		throw new NotImplementedException ();
	}

	public string[] GetDirectories (string path, string searchPattern, bool recursive)
	{
		throw new NotImplementedException ();
	}

	#endregion
	}

