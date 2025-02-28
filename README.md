# Github User Activity CLI

A simple command-line interface (CLI) tool that fetches and displays a GitHub user's recent activity and user information using the GitHub API.

## Features

- Fetch recent activity of a GitHub user.
- Fetch information about GitHub user.
- Filter activity by event type.
- Caches events data for 10 minutes using MemoryCache to reduce API calls.
- Handles API errors gracefully.

## Installation

1. Clone the repository or download the source code.
2. Navigate to the project directory in your terminal.
3. Run the application with the appropriate commands below.

## Usage

1. Fetch all recent activity

```bash
github-activity <username>
```

2. Fetch recent activity filtered by event type:

```bash
github-activity <username> <eventType>
```

## Supported Event Types

- push-event
- issues-event
- watch-event
- create-event

## Project url

https://roadmap.sh/projects/github-user-activity