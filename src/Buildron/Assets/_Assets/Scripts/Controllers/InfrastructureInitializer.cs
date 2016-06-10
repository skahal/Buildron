using System.Collections;
using Buildron.Domain;
using Buildron.Domain.Notifications;
using Buildron.Domain.Versions;
using Buildron.Infrastructure.Clients;
using Buildron.Infrastructure.Repositories;
using Skahal.Infrastructure.Framework.Commons;
using UnityEngine;

public class InfrastructureInitializer : MonoBehaviour
{
	void Awake ()
	{
		DependencyService.Register<IServerStateRepository> (new PlayerPrefsServerStateRepository ());
		
		DependencyService.Register<IVersionClient> (() => {
			return new BackEndClient (); });
		
		DependencyService.Register<IVersionRepository> (() => {
			return new PlayerPrefsVersionRepository (); });
		
		DependencyService.Register<INotificationClient> (() => {
			return new BackEndClient (); });
	}
}

