using Buildron.Domain.Builds;
using Buildron.Domain.Users;
using Skahal.Common;
using System.Collections.Generic;
using UnityEngine;

public class EmulatorUserConfig : MonoBehaviour
{
	public static EmulatorUserConfig Instance { get; private set; }

	public Texture2D[] UserPhotos;

	void Start()
	{
		Instance = this;
	}

	public Texture2D GetRandomUserPhoto()
	{
		if (UserPhotos.Length == 0) {
			return null;
		}

		return UserPhotos [Random.Range (0, UserPhotos.Length)];
	}
}
