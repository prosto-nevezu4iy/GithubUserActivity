using GithubUserActivity.Models;
using GithubUserActivity.Services;

Console.WriteLine("Usage: github-activity <username>");
Console.WriteLine("The following event types can be used: push-event, issues-event, watch-event, create-event");
Console.WriteLine();

var cacheService = new CacheService();

while (true)
{
    string input = Console.ReadLine();

    if (string.IsNullOrEmpty(input))
    {
        Console.WriteLine("Invalid input. Try again.");
        continue;
    }

    args = input.Split(' ');
    string command = args[0].ToLower();

    if (command != "github-activity")
    {
        Console.WriteLine("Usage: github-activity <username>");
        continue;
    }

    string username = args[1];
    string eventTypeFilter = args.Length > 2 ? args[2] : null;

    try
    {
        var events = await cacheService.GetCachedOrFetchEvents(username);
        var userInfo = await cacheService.GetCachedOrFetchUserInfo(username);

        if (!string.IsNullOrEmpty(eventTypeFilter))
        {
            eventTypeFilter = eventTypeFilter.Replace("-", string.Empty);
            events = events.Where(x => x.Type.Equals(eventTypeFilter, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (events.Count == 0)
        {
            Console.WriteLine("No events found.");
            return;
        }

        foreach (var evt in events)
        {
            Console.WriteLine(FormatEvent(evt));
        }

        Console.WriteLine();
        Console.WriteLine("User Information:");
        Console.WriteLine($"-- UserId {userInfo.Id}");
        Console.WriteLine($"-- Name {userInfo.Name}");
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine($"Error fetching data: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}

string FormatEvent(GithubEvent evt)
{
    return evt.Type switch
    {
        "PushEvent" => $"Pushed {evt.Payload.Commits.Count} commits to {evt.Repo.Name}",
        "IssuesEvent" => $"Opened a new issue in {evt.Repo.Name}",
        "WatchEvent" => $"Starred {evt.Repo.Name}",
        "CreateEvent" => $"A Git {evt.Payload.Ref_Type} was created in {evt.Repo.Name}",
        _ => $"Performed {evt.Type} on {evt.Repo.Name}"
    };
}