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
        private static Caching instance = null;
        public static Caching Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Caching();
                }
                return instance;
            }
        }

        public double ValueOne { get; set; }

        List<ApplicationConfigModel> ApplicationConfigs = new List<ApplicationConfigModel>();

        public void AddApplicationConfigs(List<ApplicationConfigModel> applicationConfigs)
        {
            ApplicationConfigs = applicationConfigs;
        }

        public List<ApplicationConfigModel> GetAllApplicationConfigs()
        {
            return ApplicationConfigs;
        }

        public string GetApplicationConfigs(string Key)
        {
            return ApplicationConfigs
                   .Where(w => w.Key == Key)
                   .FirstOrDefault()
                   .Value
                   ;
        }
    }
}
