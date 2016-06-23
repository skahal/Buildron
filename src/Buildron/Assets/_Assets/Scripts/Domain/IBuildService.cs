using System;
using System.Collections.Generic;
using Buildron.Domain.Sorting;

namespace Buildron.Domain
{
    public interface IBuildService
    {
        int BuildsCount { get; }
        bool Initialized { get; }
        string ServerName { get; }

        event EventHandler<BuildFoundEventArgs> BuildFound;
        event EventHandler<BuildRemovedEventArgs> BuildRemoved;
        event EventHandler<BuildsRefreshedEventArgs> BuildsRefreshed;
        event EventHandler<BuildUpdatedEventArgs> BuildUpdated;
        event EventHandler<CIServerStatusChangedEventArgs> CIServerStatusChanged;

        void AuthenticateUser(UserBase user);
        IComparer<Build> GetComparer(SortBy sortBy);
        Build GetMostRelevantBuildForUser(User user);
        void Initialize(IBuildsProvider buildsProvider);
        void RefreshAllBuilds();
        void Reset();
        void RunBuild(RemoteControl remoteControl, string buildId);
        void StopBuild(RemoteControl remoteControl, string buildId);
    }
}