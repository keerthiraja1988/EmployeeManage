using DomainModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CrossCutting.Caching
{
    public sealed class Caching
    {
        //Private Constructor.  
        private Caching()
        {
        }
        private static Caching _instance = null;
        public static Caching Instance => _instance ?? (_instance = new Caching());

        List<ApplicationConfigModel> _applicationConfigs = new List<ApplicationConfigModel>();

        public void AddApplicationConfigs(List<ApplicationConfigModel> applicationConfigs)
        {
            _applicationConfigs = applicationConfigs;
        }

        public List<ApplicationConfigModel> GetAllApplicationConfigs()
        {
            return _applicationConfigs;
        }

        public string GetApplicationConfigs(string key)
        {
            return _applicationConfigs
                   .FirstOrDefault(w => w.Key == key)
                    ?.Value
                   ;
        }
    }
}
