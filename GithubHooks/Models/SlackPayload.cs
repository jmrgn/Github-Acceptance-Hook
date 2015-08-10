using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GithubHooks.Models
{
    public class SlackPayload
    {
        private readonly string Message = "The Following issue is marked as Ready for Review:\n{0}\n<{1}>";

        public SlackPayload(string title, string url)
        {
            text = string.Format(Message, title, url);
        }
        public string text { get; set; }
    }
}