using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buildron.Domain.Mods;
using Skahal.Logging;
using UnityEngine;

namespace Buildron.Mods.Console
{
    public class Mod : IMod
    {
        public string Name
        {
            get
            {
				return "Console";
            }
        }

        public void Initialize(ModContext context)
        {
			var go = new GameObject("ConsoleController");
			var c = go.AddComponent<ModController> ();

			context.BuildFound += (sender, e) => {
				 c.AddMessage("Build found: {0}", e.Build);	
			};

			context.BuildRemoved += (sender, e) => {
				c.AddMessage("Build removed: {0}", e.Build);	
			};

			context.BuildsRefreshed += (sender, e) => {
				c.AddMessage("Build refreshed: {0} builds found, {1} builds removed, {2} builds status changed", e.BuildsFound.Count, e.BuildsRemoved.Count, e.BuildsStatusChanged.Count);	
			};

			context.BuildStatusChanged += (sender, e) => {
				c.AddMessage("Build status changed: {0}", e.Build);	
			};

			context.BuildTriggeredByChanged += (sender, e) => {
				c.AddMessage("Build triggered by changed: {0}/{1}", e.Build, e.Build.TriggeredBy);	
			};

			context.BuildUpdated += (sender, e) => {
				c.AddMessage("Build updated: {0}", e.Build);	
			};

			context.CIServerStatusChanged += (sender, e) => {
				c.AddMessage("CI server status changed: {0}", e.Server.Status);	
			};

			context.RemoteControlChanged += (sender, e) => {
				c.AddMessage("RC changed: {0}", e.RemoteControl);	
			};

			context.UserAuthenticationCompleted += (sender, e) => {
				c.AddMessage("User authentication completed: {0}:{2}", e.User, e.Success ? "success" : "failed");	
			};

			context.UserFound += (sender, e) => {
				c.AddMessage("User found: {0}", e.User);	
			};

			context.UserRemoved += (sender, e) => {
				c.AddMessage("User remoed: {0}", e.User);	
			};

			context.UserTriggeredBuild += (sender, e) => {
				c.AddMessage("User triggered build: {0}/{1}", e.User, e.Build);	
			};

			context.UserUpdated += (sender, e) => {
				c.AddMessage("User updated: {0}", e.User);	
			};
        }
    }
}
