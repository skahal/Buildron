using System;

namespace Buildron.Domain
{
    /// <summary>
    /// Represents a Buildron RC.
    /// </summary>
    [Serializable]
	public sealed class RemoteControl : UserBase
	{
        public bool Connected { get; set; }
	}
}