using System;
using System.Linq;
using System.Text.Json;

using Example;

namespace ApiQuickstartExample
{
    internal class Application
    {
        private readonly ApiClient _client;

        private readonly string _criteria;

        public Application(ApiClient client, string criteria)
        {
            _client = client;
            _criteria = criteria;
        }

        public void Run(Action<string> writeLine)
        {
            var groups = GetGroups();
            var groupReport = GroupReport.Generate(groups);
            writeLine(groupReport);

            var users = GetUsers();
            var userReport = UserReport.Generate(users);
            writeLine(userReport);

            var count = CountGradebooks();
            var gradebooks = GetGradebooks(_criteria);
            var gradebookReport = GradebookReport.Generate(count, _criteria, gradebooks);
            writeLine(gradebookReport);

            var export = ExportGradebooks(_criteria);
            var downloadUrl = $"{_client.BaseUrl}/{export.DownloadUrl}";
            var lifetime = (int)(new DateTimeOffset(export.Expiry) - DateTimeOffset.UtcNow).TotalMinutes;
            writeLine($"Here is the link to download your gradebooks: {downloadUrl}");
            writeLine($"Please note the link requires authentication, and it expires in {lifetime} minutes.");
        }

        private Group[] GetGroups()
        {
            var json = _client.Get("directory/groups").Data;

            return JsonSerializer.Deserialize<Group[]>(json)
                .OrderBy(x => x.GroupName)
                .ToArray();
        }

        private User[] GetUsers()
        {
            var json = _client.Get("security/users").Data;

            return JsonSerializer.Deserialize<User[]>(json)
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .ToArray();
        }

        private int CountGradebooks()
        {
            var data = _client.Get("progress/gradebooks/count").Data;

            return int.Parse(data);
        }

        private Gradebook[] GetGradebooks(string title)
        {
            var query = string.Empty;

            if (!string.IsNullOrEmpty(title))
                query = $"?GradebookTitle={title}";

            var json = _client.Get("progress/gradebooks" + query).Data;

            return JsonSerializer.Deserialize<Gradebook[]>(json)
                .OrderBy(x => x.GradebookTitle)
                .ToArray();
        }

        private Export ExportGradebooks(string title)
        {
            var query = string.Empty;

            if (!string.IsNullOrEmpty(title))
                query = $"?GradebookTitle={title}";

            var json = _client.Get("progress/gradebooks/export" + query).Data;

            return JsonSerializer.Deserialize<Export>(json);
        }
    }
}