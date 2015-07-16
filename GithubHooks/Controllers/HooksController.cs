using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using GithubHooks.Configuration;
using GithubHooks.Models;
using Octokit;
using Octokit.Internal;
using ProductHeaderValue = Octokit.ProductHeaderValue;

namespace GithubHooks.Controllers
{
    public class HooksController : ApiController
    {
        private static readonly string ApiKey = ConfigurationManager.ApiCredentialsConfig.Key;
        private string _baseUrl, _owner, _repoName;
        private static readonly string FallDownRobot = @"![](https://cdn3.vox-cdn.com/thumbor/9Ke1QsWFXy7kfbKGW7Qt2CrorOo=/1600x0/filters:no_upscale()/cdn0.vox-cdn.com/uploads/chorus_asset/file/3769944/robotgif_2.0.gif)";
        private static readonly string RobotCelebration = @"![](http://media.giphy.com/media/C9qVnOqGo3VyU/giphy.gif)";
        private static GitHubClient _github;
        
        [Route("hook")]
        [HttpPost]
        public IHttpActionResult ProcessHook(IssueCommentEvent commentEvent)
        {
            _baseUrl = commentEvent.Repository.Url.Scheme + "://" + commentEvent.Repository.Url.Host;
            _owner = commentEvent.Repository.Owner.Login;
            _repoName = commentEvent.Repository.Name;

            var creds = new InMemoryCredentialStore(new Credentials("derp", ApiKey));
            var headerVal = new ProductHeaderValue("GitHooks");
            _github = new GitHubClient(headerVal, creds, new Uri(_baseUrl));

            if (CheckComment(commentEvent.Comment.Body))
            {
                var branchName = GetBranchNameFromComment(commentEvent.Comment.Body);

                try
                {
                    var pullRequest = GetPullRequest(branchName, commentEvent.Issue.Number, commentEvent.Issue.Title);
                    var merge = MergePullRequest(pullRequest, commentEvent.Issue.Number, branchName, true);
                    DeleteBranch(merge, branchName, commentEvent.Issue.Number, pullRequest.Number);
                    LeaveSuccessfulMergeComment(commentEvent.Issue.Number, pullRequest.Number, branchName);
                }
                catch (RobotFallDownException e)
                {
                    return BadRequest();
                }
            }

            return Ok();
        }

        private static bool CheckComment(string comment)
        {
            return comment.Contains(":accept:");
        }

        private static string GetBranchNameFromComment(string comment)
        {
            string[] split = { ":accept:" };

            return comment.Split(split, StringSplitOptions.None)[1].Trim();
        }

        private PullRequest GetPullRequest(string branchName, int issueNumber, string issueTitle)
        {
            var existingPullRequest = FindRelatedPullRequest(branchName);
            if (existingPullRequest != null && !existingPullRequest.Merged)
            {
                return existingPullRequest;
            }

            var newPullRequest = new NewPullRequest(string.Format("#{0} - {1}", issueNumber, issueTitle), branchName, "master");
            try
            {
                var pullRequest = _github.PullRequest.Create(_owner, _repoName, newPullRequest).Result;

                return pullRequest;
            }
            catch (Exception e)
            {
                var commentBody = string.Format("{0} <br/>Zhenbot™ was unable to create Pull Request for {1}. Sorry about that :person_frowning:. Exception: {2}", FallDownRobot, branchName, e);
                LeaveIssueComment(issueNumber, commentBody);

                throw new RobotFallDownException();
            }
        }

        private PullRequestMerge MergePullRequest(PullRequest pullRequest, int issueNumber, string branchName, bool tryAgain)
        {
            var newMergePullRequest = new MergePullRequest { Message = "Merged " + pullRequest.Title };

            try
            {
                var merge = _github.PullRequest.Merge(_owner, _repoName, pullRequest.Number, newMergePullRequest).Result;

                return merge;
            }
            catch (Exception e)
            {
                var aggregateException = e as AggregateException;
                if (aggregateException != null)
                {
                    var apiException = aggregateException.GetBaseException() as ApiException;
                    if (apiException != null && apiException.Message.Equals("Pull Request is not mergeable") && tryAgain)
                    {
                        Thread.Sleep(5000);
                        return MergePullRequest(pullRequest, issueNumber, branchName, false);
                    }

                    var commentBody = string.Format("{0} <br/>Zhenbot™ was unable to merge Pull Request #{1} for {2}. Sorry about that :person_frowning:. Exception: {2}.", FallDownRobot, pullRequest.Number, branchName, e);
                    LeaveIssueComment(issueNumber, commentBody);
                }

                throw new RobotFallDownException();
            }
        }

        private void LeaveIssueComment(int issueNumber, string commentBody)
        {
            var comment = _github.Issue.Comment.Create(_owner, _repoName, issueNumber, commentBody).Result;
        }

        private PullRequest FindRelatedPullRequest(string branch)
        {
            var issues = _github.Issue.GetAllForRepository(_owner, _repoName).Result;

            foreach (var issue in issues.ToList())
            {
                if (issue.PullRequest != null)
                {
                    var actualPullRequest = _github.PullRequest.Get(_owner, _repoName, issue.Number).Result;

                    if (actualPullRequest.Head.Ref == branch)
                    {
                        return actualPullRequest;
                    }
                }
            }

            return null;
        }

        private void LeaveSuccessfulMergeComment(int issueNumber, int pullRequestNumber, string branchName)
        {
            var body = string.Format("{0} <br/>Pulled (#{1}) and deleted {2} :ok_woman:. Zhenbot™ signing off.", RobotCelebration, pullRequestNumber, branchName);
            LeaveIssueComment(issueNumber, body);
        }

        private void DeleteBranch(PullRequestMerge merge, string branchName, int issueNumber, int pullRequestNumber)
        {
            if (merge.Merged)
            {

                var task = Task.Run(async () => await _github.GitDatabase.Reference.Delete(_owner, _repoName, "heads/" + branchName));
                task.Wait();
            }
            else
            {
                var commentBody = string.Format("{0} <br/>Zhenbot™ was unable to merge Pull Request #{1} for {2}. Sorry about that :person_frowning:.", FallDownRobot, pullRequestNumber, branchName);
                LeaveIssueComment(issueNumber, commentBody);

                throw new RobotFallDownException();
            }
        }
    }
}
