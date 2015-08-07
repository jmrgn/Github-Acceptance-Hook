using System;

namespace GithubHooks.Configuration
{
    [Serializable]
    public class SlackConfiguration : ObjectConfigurationSection
    {
        public string StatusWebhookUrl { get; set; }
    }
}