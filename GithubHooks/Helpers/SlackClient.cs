using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using GithubHooks.Models;
using Newtonsoft.Json;

namespace GithubHooks.Helpers
{
    public class SlackClient
    {
        private Uri uri;
        public SlackClient(string url)
        {
            this.uri = new Uri(url);
        }

        public async Task ForwardGithubEvent(PullRequestEvent pullRequestEvent)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync(uri, new StringContent(JsonConvert.SerializeObject(pullRequestEvent), System.Text.Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
            }
        }

    }
}