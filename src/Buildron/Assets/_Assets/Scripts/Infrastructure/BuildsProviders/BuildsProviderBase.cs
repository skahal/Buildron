using System;
using System.Text.RegularExpressions;
using Buildron.Domain;
using Buildron.Domain.Builds;
using Skahal.Common;
using UnityEngine;
using Buildron.Domain.Users;
using Buildron.Domain.CIServers;
using System.Collections.Generic;

namespace Buildron.Infrastructure.BuildsProvider
{
	/// <summary>
	/// Builds provider base class.
	/// </summary>
	public abstract class BuildsProviderBase : IBuildsProvider
	{
		#region Fields
		private string m_protocol;
		private string m_serverIP;
		private Regex m_findProtocolRegex = new Regex ("(http://|https://)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private List<string> m_currentUpdatedBuildIds = new List<string>();
        #endregion

        #region Constructors
        protected BuildsProviderBase (CIServer server)
		{
			Server = server;	
			PrepareProtocol ();
			Requester = Requester.Instance;
			Requester.GetFailed += delegate(object sender, RequestFailedEventArgs e)
			{
				if (e.Url.Contains (m_serverIP))
				{
					ServerDown.Raise (this);
				}
			};
		}
		#endregion

		#region Properties
		public string Name { get; protected set; }

		public AuthenticationRequirement AuthenticationRequirement { get; protected set; }

		public string AuthenticationTip { get; protected set; }

		protected Requester Requester { get; private set; }

		protected CIServer Server { get; private set; }

        protected int CurrentBuildsFoundCount { get; set; }
        #endregion

        #region IBuildsProvider implementation
        public event EventHandler<BuildUpdatedEventArgs> BuildUpdated;
		public event EventHandler BuildsRefreshed;
		public event EventHandler ServerUp;
		public event EventHandler ServerDown;
		public event EventHandler UserAuthenticationSuccessful;
		public event EventHandler UserAuthenticationFailed;

		public void RefreshAllBuilds ()
        {
            CurrentBuildsFoundCount = 0;
            m_currentUpdatedBuildIds.Clear();
            PerformRefreshAllBuilds();
        }

        protected abstract void PerformRefreshAllBuilds();

        public abstract void RunBuild (UserBase user, Build build);

		public abstract void StopBuild (UserBase user, Build build);

		public abstract void AuthenticateUser (UserBase user);
		#endregion

		#region Methods
		private void PrepareProtocol ()
		{
			var matchProtocol = m_findProtocolRegex.Match (Server.IP);
			
			if (matchProtocol.Success)
			{
				m_protocol = matchProtocol.Groups [1].Value;
				m_serverIP = m_findProtocolRegex.Replace (Server.IP, "");
			}
			else
			{ 
				m_protocol = "http://";
				m_serverIP = Server.IP;
			}
		}

		protected string GetHttpBasicAuthUrl (UserBase user, string urlEndPart, params object[] args)
		{	
			var endPart = string.Format (urlEndPart, args);
			
			if (String.IsNullOrEmpty (user.UserName) && String.IsNullOrEmpty (user.Password))
			{
				return string.Format (
					"{0}{1}/{2}",
					m_protocol,
					EscapeUrl (m_serverIP),
					EscapeUrl (endPart));	
			}
			else
			{
				string domain = String.IsNullOrEmpty (user.Domain) ? "" : user.Domain + @"\";
			
				return string.Format (
					"{0}{1}{2}:{3}@{4}/{5}", 
					m_protocol,
					WWW.EscapeURL (domain),
					WWW.EscapeURL (user.UserName), 
					WWW.EscapeURL (user.Password), 
					EscapeUrl (m_serverIP),
					EscapeUrl (endPart));	
			}
		}

		protected string EscapeUrl (string url)
		{
			return url.Replace (" ", "%20");
		}

		protected void OnBuildUpdated (BuildUpdatedEventArgs e)
		{
			OnServerUp ();
			BuildUpdated.Raise (this, e);

            var build = e.Build;

            if (!m_currentUpdatedBuildIds.Contains(build.Id))
            {
                m_currentUpdatedBuildIds.Add(build.Id);
            }

			if (m_currentUpdatedBuildIds.Count >= CurrentBuildsFoundCount)
            {
                OnBuildsRefreshed();
            }
        }

		private void OnBuildsRefreshed ()
		{
			BuildsRefreshed.Raise (this);
		}

        protected void OnServerUp ()
		{
			ServerUp.Raise (this);
		}

		protected void OnServerDown ()
		{
			ServerDown.Raise (this);
		}

		protected void OnUserAuthenticationSuccessful ()
		{
			UserAuthenticationSuccessful.Raise (this);
		}

		protected void OnUserAuthenticationFailed ()
		{
			UserAuthenticationFailed.Raise (this);
		}
		#endregion
	}
}