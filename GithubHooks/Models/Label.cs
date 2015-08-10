using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GithubHooks.Models
{
    public class Label
    {
        public string url { get; set; }
        public string name { get; set; }
        public string color { get; set; }
    }
}