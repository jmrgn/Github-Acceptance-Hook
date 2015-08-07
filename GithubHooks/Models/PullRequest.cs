using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GithubHooks.Models
{
    /// <summary>
    /// AUTO GENERATED
    /// </summary>
    public class PullRequest
    {
        public string url { get; set; }
        public int id { get; set; }
        public string html_url { get; set; }
        public string diff_url { get; set; }
        public string patch_url { get; set; }
        public string issue_url { get; set; }
        public int number { get; set; }
        public string state { get; set; }
        public bool locked { get; set; }
        public string title { get; set; }
        public GithubUser user { get; set; }
        public string body { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public object closed_at { get; set; }
        public object merged_at { get; set; }
        public object merge_commit_sha { get; set; }
        public object assignee { get; set; }
        public object milestone { get; set; }
        public string commits_url { get; set; }
        public string review_comments_url { get; set; }
        public string review_comment_url { get; set; }
        public string comments_url { get; set; }
        public string statuses_url { get; set; }
        public Head head { get; set; }
        public Base @base { get; set; }
        public Links _links { get; set; }
        public bool merged { get; set; }
        public object mergeable { get; set; }
        public string mergeable_state { get; set; }
        public object merged_by { get; set; }
        public int comments { get; set; }
        public int review_comments { get; set; }
        public int commits { get; set; }
        public int additions { get; set; }
        public int deletions { get; set; }
        public int changed_files { get; set; }
    }

    public class Head
    {
        public string label { get; set; }
        public string @ref { get; set; }
        public string sha { get; set; }
        public GithubUser user { get; set; }
        public Repository repo { get; set; }
    }

    public class Base
    {
        public string label { get; set; }
        public string @ref { get; set; }
        public string sha { get; set; }
        public GithubUser user { get; set; }
        public Repository repo { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Html
    {
        public string href { get; set; }
    }

    public class IssueLink
    {
        public string href { get; set; }
    }

    public class CommentsLink
    {
        public string href { get; set; }
    }

    public class ReviewCommentsLink
    {
        public string href { get; set; }
    }

    public class ReviewCommentLink
    {
        public string href { get; set; }
    }

    public class CommitsLink
    {
        public string href { get; set; }
    }

    public class StatusesLink
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Self self { get; set; }
        public Html html { get; set; }
        public IssueLink issue { get; set; }
        public CommentsLink comments { get; set; }
        public ReviewCommentsLink review_comments { get; set; }
        public ReviewCommentLink review_comment { get; set; }
        public CommitsLink commits { get; set; }
        public StatusesLink statuses { get; set; }
    }

}