using System;
using System.Collections.Generic;
using System.Text;

namespace NewsSystem.Services.Sources
{
    public interface ISource
    {
        string BaseUrl { get; }

        IEnumerable<RemoteNews> GetLatestPublications();

        IEnumerable<RemoteNews> GetAllPublications();

        RemoteNews GetPublication(string url);
    }
}
