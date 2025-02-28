namespace GithubUserActivity.Models;

public class GithubEvent
{
    public string Type { get; set; }
    public GithubRepo Repo { get; set; }
    public GithubPayload Payload { get; set; }
}