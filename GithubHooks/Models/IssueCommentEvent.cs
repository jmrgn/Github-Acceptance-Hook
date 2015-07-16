namespace GithubHooks.Models
{
    public class IssueCommentEvent
    {
        public string Action { get; set; }
        public Issue Issue { get; set; }
        public Comment Comment { get; set; }
        public Repository Repository { get; set; }
        public Sender Sender { get; set; }
    }
}