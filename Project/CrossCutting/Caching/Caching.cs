using DomainModel;
using System;
using System.Collections.Generic;
using System.Text;

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

        public List<ApplicationConfigModel> GetApplicationConfigs()
        {
            return ApplicationConfigs;
        }
    }
}
