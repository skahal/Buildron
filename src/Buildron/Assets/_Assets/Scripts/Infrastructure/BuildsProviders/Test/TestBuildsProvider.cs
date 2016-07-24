using System.Collections.Generic;
using Skahal.Common;
using UnityEngine;
using Buildron.Domain.Builds;
using Buildron.Domain.Users;

/// <summary>
/// Test builds provider.
/// </summary>
public class TestBuildsProvider : IBuildsProvider
{
	#region Fields
	private List<Build> m_builds;
	#endregion
	
	#region Constructors
	/// <summary>
	/// Initializes a new instance of the <see cref="TestBuildsProvider"/> class.
	/// </summary>
	public TestBuildsProvider ()
	{
		m_builds = new List<Build> ();
		int id = 0;
		
		System.Action<string, string> addBuild = (name, projectName) =>
		{
			id++;
			m_builds.Add (new Build () 
			{
				Id = id.ToString (),
				Configuration = new BuildConfiguration ()
				{
					Id = id.ToString (),
					Name = name,
					Project = new BuildProject () 
					{
						Name = projectName
					}
				}
			});
		};
		
		for (int i = 0; i < 5; i++) {
			addBuild ("iOS - Classic", "Ships N'Battles");
			addBuild ("iOS - HD", "Ships N'Battles");
			addBuild ("Android", "Ships N'Battles");
			addBuild ("Mac", "Ships N'Battles");
			addBuild ("Web", "Ships N'Battles");
			addBuild ("Windows Phone", "Ships N'Battles");
			addBuild ("OUYA", "Ships N'Battles");
		
			addBuild ("1.4.3", "Card-o-matic");
			addBuild ("1.?.?", "Card-o-matic");
		
			addBuild ("Mac", "Buildron");
		}
	}
	#endregion
	
	#region IBuildsProvider implementation
	public event System.EventHandler<BuildUpdatedEventArgs> BuildUpdated;
	public event System.EventHandler BuildsRefreshed;	
	public event System.EventHandler ServerUp;
	public event System.EventHandler ServerDown;
	public event System.EventHandler<UserAuthenticationCompletedEventArgs> UserAuthenticationCompleted;
	
	public string Name { get { return "Test"; }}
	public AuthenticationRequirement AuthenticationRequirement { get { return AuthenticationRequirement.Never; } }
	public string AuthenticationTip { get { return ""; }}
	
	public void RefreshAllBuilds ()
	{
		int buildsCount = m_builds.Count;
		
		for (int i = 0; i < buildsCount; i++) {
			var b = m_builds [i];
			b.Status = RandomStatus ();
			b.LastRanStep = new BuildStep () { 
				Name = "Test",
				StepType = (BuildStepType)Random.Range(0, 8)
			};
			
			b.PercentageComplete = Random.Range (0f, 1f);

			var userId = Random.Range (0, 2);
			var user = new User () 
			{
				Name = "User " + userId,
				Email = userId == 0 ? "giacomelli@gmail.com" : "giusepe@gmail.com",
			};

			user.UserName = user.Name;
			user.Builds.Add (b);

			b.TriggeredBy = user;
			b.LastChangeDescription = System.DateTime.Now.ToLongTimeString ();
			b.Date = System.DateTime.Now;
			BuildUpdated.Raise (this, new BuildUpdatedEventArgs (b));
		}
		
		BuildsRefreshed.Raise (this);
	}
	
	public void RunBuild (IAuthUser user, IBuild build)
	{
		build.Status = BuildStatus.Running;
	}
	
	public void StopBuild (IAuthUser user, IBuild build)
	{
		build.Status = BuildStatus.Canceled;
	}
	
	private BuildStatus RandomStatus ()
	{
		return (BuildStatus)Random.Range (1, (int)BuildStatus.Running);
	}
	
	public void AuthenticateUser (IAuthUser user)
	{
		if (string.IsNullOrEmpty (user.Password)) {
			UserAuthenticationCompleted.Raise (this, new UserAuthenticationCompletedEventArgs (user, false));
		} else {
			UserAuthenticationCompleted.Raise (this, new UserAuthenticationCompletedEventArgs (user, true));
		}
	}
	
	#endregion
}