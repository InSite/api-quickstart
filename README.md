# Shift API Quickstart

This repository includes the source code for a C# .NET console application that demonstrates how to interact with the API using an HTTP client. 

This example shows how to authenticate, make API calls, and process responses for groups, user accounts, and gradebooks. The API provides much more, of course. The source code here is intended as an example only, to help you get started working with the API.

> Please note this quickstart applies only to version 2 of the API for Shift iQ.

You can explore the API documentation here: [Shift API version 2](https://docs.shiftiq.com/developers/api-v2/introduction)

## Table of Contents

- [Overview](#overview)
- [Prerequisites](#prerequisites)
- [Quick Start](#quick-start)
- [Configuration](#configuration)
- [Project Structure](#project-structure)
- [Usage](#usage)
- [API Client Features](#api-client-features)
- [Data Models](#data-models)
- [Examples](#examples)
- [Building and Running](#building-and-running)
- [Contributing](#contributing)
- [License](#license)

## Overview

This quickstart demonstrates:
- **Bearer token authentication** with a client secret
- **HTTP client implementation** with proper disposal patterns
- **JSON serialization/deserialization** using System.Text.Json
- **Configuration management** with appsettings.json
- **API response handling** with custom response objects
- **Simple data processing and reporting** from API responses

The application fetches data from three main endpoints:
- `/directory/groups` - Contact groups from the Directory API
- `/security/users` - User accounts from the Security API
- `/progress/gradebooks` - Gradebooks from the Progress API

## Prerequisites

- **.NET 9.0 SDK** or later
- **Visual Studio 2022** or **VS Code** with C# extension
- **API credentials** (client secret and base address URL)

## Quick Start

1. **Clone the repository**
   ```bash
   git clone https://github.com/insite/api-quickstart.git
   cd api-quickstart
   ```

2. **Configure your API credentials**
   ```bash
   # Edit appsettings.json with your actual values
   ```

3. **Build and run**
   ```bash
   dotnet build
   dotnet run
   ```

## Configuration

### appsettings.json

Update the configuration file with your API credentials:

```json
{
  "ApiQuickstart": {
    "Secret": "YOUR CLIENT SECRET",
    "BaseUrl": "YOUR API BASE URL",
    "UserAgent": "ApiQuickstartExample/1.0 (YOUR SYSTEM INFO)",
    "TimeoutSeconds": 30
  }
}
```

### Environment Variables

Alternatively, you can set configuration options using environment variables:
- `ApiQuickstart__Secret`
- `ApiQuickstart__BaseUrl`
- `ApiQuickstart__UserAgent`
- `ApiQuickstart__TimeoutSeconds`

### Configuration Properties

| Property | Description | Required |
|----------|-------------|----------|
| `Secret` | Your API client secret for authentication | Yes |
| `BaseUrl` | The base URL of your API instance | Yes |
| `UserAgent` | User agent string for API requests | No (default: auto detect) |
| `TimeoutSeconds` | Request timeout in seconds | No (default: 30) |

## Project Structure

```
├── ApiClient.cs           # Simple HTTP client implementation (for demo purposes only)
├── ApiResponse.cs         # Response wrapper with JSON deserialization
├── Application.cs         # Main application logic
├── ApplicationSettings.cs # Configuration model
├── Program.cs             # Entry point and configuration loading
├── appsettings.json       # Configuration file
├── Example.csproj         # Project file
├── Models/
│   ├── Gradebook.cs # Gradebook data model
│   ├── Group.cs     # Group data model
│   └── User.cs      # User data model
└── Reports/
    ├── GradebookReport.cs # Gradebook summary generator
    ├── GroupReport.cs     # Group listing generator
    └── UserReport.cs      # User summary generator
```

## Usage

### Basic API Client Usage

```csharp
using var client = new ApiClient(clientSecret, baseUrl, userAgent);
client.SetTimeout(TimeSpan.FromSeconds(30));

// Make a GET request
var response = client.Get("platform/status");
if (response.Success)
{
    Console.WriteLine(response.Data);
}
```

### Async Operations

```csharp
// Async GET request
var response = await client.GetAsync("directory/groups");
if (response.Success)
{
    var groups = response.GetJsonData<Group[]>();
    // Process groups...
}
```

### JSON Deserialization

```csharp
// Deserialize response data
var response = client.Get("directory/groups");
var departments = response.GetJsonData<Group[]>();

// Or manual deserialization
var groups = JsonSerializer.Deserialize<Group[]>(response.Data);
```

## API Client Features

### Authentication
- **Bearer token authentication** using client secret
- **Automatic header management** for all requests

> **Cookie token authentication** is also available for more specialized (and more advanced) integration scenarios. An example is provided in the src/web folder of this repository.

### Request Handling
- **GET requests** with query parameters
- **Custom headers** support
- **URL encoding** for parameters
- **Timeout configuration**

### Response Processing
- **HTTP status code** handling
- **Response headers** extraction
- **JSON deserialization** helpers
- **Error handling** with detailed messages

### Resource Management
- **IDisposable implementation** for proper cleanup
- **HttpClient disposal** following best practices

## Examples

### Fetching and Processing Groups

```csharp
var response = client.Get("directory/groups");
var groups = JsonSerializer.Deserialize<Group[]>(response.Data)
    .OrderBy(x => x.Name)
    .ToArray();

Console.WriteLine($"Found {groups.Length} groups:");
foreach (var group in groups)
{
    Console.WriteLine($"  - {group.Name}");
}
```

### Filtering Gradebooks

```csharp
var response = client.Get("progress/gradebooks");
var gradebooks = JsonSerializer.Deserialize<Gradebook[]>(response.Data);

// Find gradebooks with names that contain "Assess"
var filtered = gradebooks
    .Where(x => x.GradebookTitle.Contains("assess", StringComparison.OrdinalIgnoreCase))
    .OrderBy(x => x.GradebookTitle)
    .ToArray();
```

## Building and Running

### Using .NET CLI

```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run

# Run with specific configuration
dotnet run --configuration Release
```

### Using Visual Studio

1. Open `Example.csproj` in Visual Studio
2. Set your API credentials in `appsettings.json`
3. Press F5 to build and run

## Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature-name`
3. Make your changes
4. Test your changes: `dotnet test`
5. Commit your changes: `git commit -am 'Add feature'`
6. Push to the branch: `git push origin feature-name`
7. Submit a pull request

## License

This project is licensed under the Unlicense - see the [LICENSE](LICENSE) file for details.

---

**Note**: This is a demo application. Remember to replace the placeholder values in `appsettings.json` with your actual setup values before running.
