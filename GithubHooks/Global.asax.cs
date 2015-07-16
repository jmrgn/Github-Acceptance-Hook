using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GithubHooks
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new SnakeCasePropertyNamesContractResolver();
        }

        public class SnakeCasePropertyNamesContractResolver : DeliminatorSeparatedPropertyNamesContractResolver
        {
            public SnakeCasePropertyNamesContractResolver() : base('_') { }
        }

        public class DeliminatorSeparatedPropertyNamesContractResolver : DefaultContractResolver
        {
            private readonly string _separator;

            protected DeliminatorSeparatedPropertyNamesContractResolver(char separator)
                : base(true)
            {
                _separator = separator.ToString();
            }

            protected override string ResolvePropertyName(string propertyName)
            {
                var parts = new List<string>();
                var currentWord = new StringBuilder();

                foreach (var c in propertyName)
                {
                    if (char.IsUpper(c) && currentWord.Length > 0)
                    {
                        parts.Add(currentWord.ToString());
                        currentWord.Clear();
                    }
                    currentWord.Append(char.ToLower(c));
                }

                if (currentWord.Length > 0)
                {
                    parts.Add(currentWord.ToString());
                }

                return string.Join(_separator, parts.ToArray());
            }
        }
    }
}
