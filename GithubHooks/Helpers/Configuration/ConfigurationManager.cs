﻿namespace GithubHooks.Configuration
{
    /// <summary>
    /// A control panel specific configuration manager.
    /// All custom configuration sections should be accessed here.
    /// </summary>
    public static class ConfigurationManager
    {

        public static ApiCredentialsConfiguration ApiCredentialsConfig
        {
            get
            {
                return GetConfiguration<ApiCredentialsConfiguration>("ApiCredentialsConfiguration");
            }
        }

        public static SlackConfiguration SlackConfiguration
        {
            get
            {
                return GetConfiguration<SlackConfiguration>("SlackConfiguration");
            }
        }


        private static TConfiguration GetConfiguration<TConfiguration>(string name)
            where TConfiguration : ObjectConfigurationSection
        {
            var section = System.Configuration.ConfigurationManager.GetSection(name);
            var result = section as TConfiguration;

            return result;
        }
    }
}