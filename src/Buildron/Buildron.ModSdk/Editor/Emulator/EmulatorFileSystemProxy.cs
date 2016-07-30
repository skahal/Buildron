using System;
using Buildron.Domain.Mods;

public class EmulatorFileSystemProxy : IFileSystemProxy
	{
		public EmulatorFileSystemProxy ()
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

