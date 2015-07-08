using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Web.Http;
using GithubHooks.Configuration;
using GithubHooks.Helpers;
using GithubHooks.Models;
using Newtonsoft.Json;
using Octokit;
using Octokit.Internal;
using WebApplication1.Models;
using PullRequest= GithubHooks.Models.PullRequest;

namespace GithubHooks.Controllers
{
    public class HooksController : ApiController
    {
        private static string apiKey = ConfigurationManager.ApiCredentialsConfig.Key;
        private string baseUrl, owner, repoName, pullRequestBase, pullRequestMerge, deleteBranch;
        private static string fallDownRobot = @"![](https://cdn3.vox-cdn.com/thumbor/9Ke1QsWFXy7kfbKGW7Qt2CrorOo=/1600x0/filters:no_upscale()/cdn0.vox-cdn.com/uploads/chorus_asset/file/3769944/robotgif_2.0.gif)";
        private static string robotCelebration = @"![](http://media.giphy.com/media/C9qVnOqGo3VyU/giphy.gif)";

        [Route("hook")]
        [HttpPost]
        public IHttpActionResult ProcessHook(IssueCommentEvent commentEvent)
        {
            var index = Array.IndexOf(commentEvent.Comment.Url.LocalPath.Split('/'), "repos");
            var urlParts = commentEvent.Comment.Url.LocalPath.Split('/');
            var url = commentEvent.Comment.Url.AbsoluteUri;
            var start = url.IndexOf("/repos", 0);
            baseUrl = url.Substring(0, start);
            owner = urlParts[index + 1];
            repoName = urlParts[index + 2];
            pullRequestBase = string.Format("{0}/repos/{1}/{2}/pulls", baseUrl, owner, repoName);
            pullRequestMerge = pullRequestBase + "/{0}/merge";
            deleteBranch = string.Format("{0}/repos/{1}/{2}/git/refs/heads", baseUrl, owner, repoName) + "/{0}";

            var creds = new InMemoryCredentialStore(new Credentials("derp", apiKey));
            var headerVal = new ProductHeaderValue("GitHooks");
            var github = new GitHubClient(headerVal, creds, new Uri(baseUrl));
            var apiConnection = new ApiConnection(new Connection(headerVal, creds));

            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new LowercaseContractResolver()
            };

            if (commentEvent != null)
            {
                if (checkComment(commentEvent.Comment.Body))
                {
                    var branchName = getBranchNameFromComment(commentEvent.Comment.Body);

                    PullAndMerge(branchName, commentEvent.Issue.Number, commentEvent.Issue.Title, apiConnection, github,
                        settings);
                }
            }

            return Ok();
        }

        private IHttpActionResult PullAndMerge(string branchName, int issueNumber, string issueTitle,
            ApiConnection apiConnection, GitHubClient github, JsonSerializerSettings settings)
        {
            var pullReq = new PullRequest()
            {
                Base = "master",
                Head = branchName,
                Title = string.Format("#{0} - {1}", issueNumber, issueTitle),
                Body = "Pull Request Auto-Created by Zhenbot™"
            };

            object pullReqNumber = null;

            try
            {
                pullReqNumber = apiConnection.Post<Dictionary<string, object>>(new Uri(pullRequestBase), JsonConvert.SerializeObject(pullReq, settings)).Result["number"];
            }
            catch (Exception e)
            {
                var aggregateException = e as AggregateException;
                if (aggregateException != null)
                {
                    var apiException = aggregateException.GetBaseException() as ApiException;
                    if (apiException != null && apiException.Message.Equals("Pull Request is not mergeable"))
                    {
                        
                    }
                }

                var comment = new PostableComment()
                {
                    Body = string.Format("{0} <br/>Zhenbot™ was unable to create Pull Request for {1}. Sorry about that :person_frowning:. Exception: {2}", fallDownRobot, branchName, e)
                };

                var finalComment = github.Issue.Comment.Create(owner, repoName, issueNumber, JsonConvert.SerializeObject(comment, settings)).Result;
                return BadRequest();
            }

            MergeResult mergeResult;

            try
            {
                mergeResult = MergePullRequest(pullReqNumber, apiConnection, settings, true);
            }
            catch (Exception e)
            {
                var comment = new PostableComment()
                {
                    Body = string.Format("{0} <br/>Zhenbot™ was unable to merge Pull Request #{1} for {2}. Sorry about that :person_frowning:. Exception: {2}.", fallDownRobot, pullReqNumber, branchName, e)
                };

                var finalComment = github.Issue.Comment.Create(owner, repoName, issueNumber, JsonConvert.SerializeObject(comment, settings)).Result;
                return BadRequest();
            }

            if (mergeResult.Merged)
            {
                apiConnection.Delete(new Uri(string.Format(deleteBranch, branchName)));

                var comment = new PostableComment()
                {
                    Body = string.Format("{0} <br/>Pulled (#{1}) and deleted {2} :ok_woman:. Zhenbot™ signing off.", robotCelebration, pullReqNumber, branchName)
                };

                var finalComment = github.Issue.Comment.Create(owner, repoName, issueNumber, JsonConvert.SerializeObject(comment, settings)).Result;
            }
            else
            {
                var comment = new PostableComment()
                {
                    Body = string.Format("{0} <br/>Zhenbot™ was unable to merge Pull Request #{1} for {2}. Sorry about that :person_frowning:.", fallDownRobot, pullReqNumber, branchName)
                };

                var finalComment = github.Issue.Comment.Create(owner, repoName, issueNumber, JsonConvert.SerializeObject(comment, settings)).Result;
            }

            return Ok();
        }

        private MergeResult MergePullRequest(object pullReqNumber, ApiConnection apiConnection, JsonSerializerSettings settings, bool tryAgain)
        {
            var merge = new Merge()
            {
                CommitMessage = "Auto-merging pull request. Beep Boop."
            };

            var mergeUrl = string.Format(pullRequestMerge, pullReqNumber);

            try
            {
                return apiConnection.Put<MergeResult>(new Uri(mergeUrl), JsonConvert.SerializeObject(merge, settings)).Result;
            }
            catch (AggregateException e)
            {
                var apiException = e.GetBaseException() as ApiException;
                if (apiException != null && apiException.Message.Equals("Pull Request is not mergeable") && tryAgain)
                {
                    //naive sleep, I think the problem is with trying to merge IMMEDIATELY
                    Thread.Sleep(5000);
                    return MergePullRequest(pullReqNumber, apiConnection, settings, false);
                }
            }

            return new MergeResult()
            {
                Merged = false
            };
        }

        private string getBranchNameFromComment(string comment)
        {
            string[] split = { ":accept:" };

            return comment.Split(split, StringSplitOptions.None)[1].Trim();
        }

        private bool checkComment(string comment)
        {
            return comment.Contains(":accept:");
        }
    }
}
