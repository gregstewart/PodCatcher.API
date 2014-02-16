using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace PodCatcher.API.Models.Episodes
{
    public class EpisodeTableRepositoryFactory
    {
        private static IEpisodeRepository _episodeRepository = null;

        public static IEpisodeRepository Create()
        {
            if (_episodeRepository != null)
            {
                return _episodeRepository;
            }

            return new EpisodeTableRepository();
        }

        public static void SetEpisodeRepository(IEpisodeRepository episodeRepository)
        {
            _episodeRepository = episodeRepository;
        }

    }
}