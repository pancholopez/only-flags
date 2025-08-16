# Only Flags - Feature Flag Manager

The [PROJECT_NAME] description goes here.

## Project Goals

- Centralize the creation and management of feature flags

## Prerequisites

1. Get the latest [dotnet SDK](https://dotnet.microsoft.com/en-us/download)

2. Install Entity Framework tools

    ```shell
    # install for the first time
    dotnet tool install --global dotnet-ef

    # or, make sure you have the latest version installed
    dotnet tool update --global dotnet-ef
    ```

## Project Setup

### Clone the Repository

```shell
git clone https://[REPOSITORY_URL].git
```

### Update Application and Test Settings

Check the `appsettings.json` in the projects within the `src` folder.

If you are running integration or acceptance tests, they have their own settings file in the `tests` project folder.

#### User Secrets

Optionally, you can use [user secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets). User secrets
will override any application settings with the same name.

User secrets are per-project. Here is an example to set the Active Directory user password for integration tests:

```bash
# assuming you start from the project root folder
cd .\[PROJECT_FOLDER]\

# add or replace a user secret one by one
dotnet user-secrets set "ActiveDirectory:Password" "pass123" --project .\tests\[PROJECT_NAME].IntegrationTests\

# add or replace all user secrets at once using secrets.json file in the root folder
# ensure that secrets.json is ignored by git to prevent accidental commits

git update-index --assume-unchanged secrets.json

# if you are using Windows
type .\secrets.json | dotnet user-secrets set --project .\tests\[PROJECT_NAME].IntegrationTests\

# or if you are using PowerShell or Linux
cat .\secrets.json | dotnet user-secrets set --project .\tests\[PROJECT_NAME].IntegrationTests\

# confirm by listing all user secrets in your project
dotnet user-secrets list --project .\tests\[PROJECT_NAME].IntegrationTests\

# if you made a mistake and want to reset all secrets
dotnet user-secrets clear --project .\tests\[PROJECT_NAME].IntegrationTests\ 
```

### Restore Dependencies, Build, and Run

```shell
# navigate to the solution folder
cd .\[PROJECT_FOLDER]\

# restore NuGet dependencies
dotnet restore

# build all projects
dotnet build

# run the main project in the src folder
dotnet run --project .\src\Server
```

## Tests

Testing ensures your code behaves as expected. We categorize tests into different types:

### Unit Tests

Verify individual components or functions in isolation.

```shell
dotnet test --filter TestCategory=Unit --logger "console;verbosity=normal"
```

### Integration Tests

Ensure different modules or services work together.

```shell
dotnet test --filter TestCategory=Integration --logger "console;verbosity=normal"
```

### Acceptance Tests (UI Tests)

Simulate user interactions to verify end-to-end functionality.

```shell
dotnet test --filter TestCategory=Acceptance --logger "console;verbosity=normal"
```

## Database

Make sure your project has the latest dependencies

```shell
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

Add, Remove or Apply DB migrations

```shell
# to add a new migration
dotnet ef migrations add initialCreate --project .\src\App.Data\

# to rollback the latest migration
dotnet ef migrations remove --project .\src\App.Data\

# to apply all pending migrations
dotnet ef database update
```

## Versions

- [Latest Stable Release](https://[REPOSITORY_URL]/commits/branch/release/CurrentStable)

## Dev Containers

This repository includes a Dev Container configuration for a ready-to-code environment using .NET 9 and SQLite.

Getting started:

1. Install Visual Studio Code and the "Dev Containers" extension (ms-vscode-remote.remote-containers), or use GitHub Codespaces.
2. Open the repository in VS Code.
3. When prompted, "Reopen in Container". VS Code will build the image and install dependencies.
4. After the container is ready, run:
   - `dotnet restore only-flags.sln` (runs automatically on first create)
   - `dotnet build only-flags.sln`
   - `dotnet test` to run tests

Notes:
- The container is based on `mcr.microsoft.com/devcontainers/dotnet:1-9.0-bookworm` and includes SQLite.
- Environment variables `DOTNET_CLI_TELEMETRY_OPTOUT` and `DOTNET_NOLOGO` are set in the container.

## How to Contribute

Follow these steps:

1. **Create a Branch**: Switch to a new branch for your changes.
2. **Make Your Changes**: Update code or documentation.
3. **Commit Your Changes**: Use `git commit` with a clear message.
4. **Push to Your Branch**: Upload changes to the remote branch.
5. **Open a Pull Request**: Submit your changes for review.
6. **Respond to Feedback**: Make updates as needed.

## License

© 2024 [YOUR_COMPANY] ([COMPANY_URL])