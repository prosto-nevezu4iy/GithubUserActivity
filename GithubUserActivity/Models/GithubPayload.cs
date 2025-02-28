using GithubUserActivity.Models;

public class GithubPayload
{
    public List<GithubCommit> Commits { get; set; }
    public string Ref_Type { get; set; }
}