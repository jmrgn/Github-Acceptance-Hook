using System;

namespace GithubHooks.Models
{
    public class Repository
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public GithubUser Owner { get; set; }
        public bool Private { get; set; }
        public Uri HtmlUrl { get; set; }
        public string Description { get; set; }
        public bool Fork { get; set; }
        public Uri Url { get; set; }
        public Uri ForksUrl { get; set; }
        public Uri KeysUrl { get; set; }
        public Uri CollaboratorsUrl { get; set; }
        public Uri TeamsUrl { get; set; }
        public Uri HooksUrl { get; set; }
        public Uri IssueEventsUrl { get; set; }
        public Uri EventsUrl { get; set; }
        public Uri AssigneesUrl { get; set; }
        public Uri BranchesUrl { get; set; }
        public Uri TagsUrl { get; set; }
        public Uri BlobsUrl { get; set; }
        public Uri GitTagsUrl { get; set; }
        public Uri GitRefsUrl { get; set; }
        public Uri TreesUrl { get; set; }
        public Uri StatusesUrl { get; set; }
        public Uri LanguagesUrl { get; set; }
        public Uri StargazersUrl { get; set; }
        public Uri ContributorsUrl { get; set; }
        public Uri SubscribersUrl { get; set; }
        public Uri SubscriptionUrl { get; set; }
        public Uri CommitsUrl { get; set; }
        public Uri GitCommitsUrl { get; set; }
        public Uri CommentsUrl { get; set; }
        public Uri IssueCommentUrl { get; set; }
        public Uri ContentsUrl { get; set; }
        public Uri CompareUrl { get; set; }
        public Uri MergesUrl { get; set; }
        public Uri ArchiveUrl { get; set; }
        public Uri DownloadsUrl { get; set; }
        public Uri IssuesUrl { get; set; }
        public Uri PullsUrl { get; set; }
        public Uri MilestonesUrl { get; set; }
        public Uri NotificationsUrl { get; set; }
        public Uri LabelsUrl { get; set; }
        public Uri ReleasesUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime PushedAt { get; set; }
        public Uri GitUrl { get; set; }
        public Uri SshUrl { get; set; }
        public Uri CloneUrl { get; set; }
        public Uri SvnUrl { get; set; }
        public Uri Homepage { get; set; }
        public int Size { get; set; }
        public int StargazersCount { get; set; }
        public int WatchersCount { get; set; }
        public string Language { get; set; }
        public bool HasIssues { get; set; }
        public bool HasDownloads { get; set; }
        public bool HasWiki { get; set; }
        public bool HasPages { get; set; }
        public int ForksCount { get; set; }
        public Uri MirrorUrl { get; set; }
        public int OpenIssuesCount { get; set; }
        public int Forks { get; set; }
        public int OpenIssues { get; set; }
        public int Watchers { get; set; }
        public string DefaultBranch { get; set; }
    }
}