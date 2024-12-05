# Trimble App Xchange Connector

This repository provides a guide for developers building connectors on the Trimble App Xchange platform. For a broader introduction to the platform, including pre-development considerations, view our [App Xchange Help Documentation](#).

## SDK Documentation Overview
The SDK provides tools and libraries to simplify building connectors using .NET. This guide walks you through prerequisites, installation, and the recommended file structure for your connector.

---

## Getting Started

### Prerequisites

1. **HTTP API:** Your software should expose an HTTP API. While other setups are possible, the SDK is optimized for HTTP endpoints.
2. **Test Data Access:** Use test data during development to avoid affecting live customer environments.
3. **C# Skills:** Intermediate or advanced knowledge of C# is required.
4. **.NET SDK:** Ensure .NET 7 or .NET 8 is installed. [Download .NET here](https://dotnet.microsoft.com/download).
5. **API Familiarity:** Understand your API resources and how to make requests to its endpoints.
6. **Defined Use Case:** Start with a clear integration use case to guide the connector's development.

### Main Components

- **SDK Library NuGet Package:** Framework for building and running connectors.
- **CLI Tool:** Interactive tool for managing the Xchange platform and your local codebase.

---

## Installing the CLI

The recommended way to use the CLI is to install it as a global .NET tool:

```bash
dotnet tool install Trimble.Xchange.Connector.CLI --global

## Updating the CLI Tool

```markdown
## Updating the CLI Tool

To update the tool, run:

```bash
dotnet tool update Trimble.Xchange.Connector.CLI --global
```

Verify installation by listing global .NET tools:

```bash
dotnet tool list --global
```

You should see `Trimble.Xchange.Connector.CLI` in the list. The `xchange` command is now available in any directory.

---

## Connector Project Structure

Connectors follow a specific structure when initialized with the CLI using `xchange connector new`. Below is the recommended structure:

```
| -> Connector{ConnectorName}/
  | -> Connector/
    | -> App/
        | -> ...
    | -> Client/
    | -> Connections/
    | -> Connector.csproj
    | -> settings.json
    | -> Program.cs
    | -> ...
  | -> Connector.sln
```

### Important Notes:
- **Do not rename or move** the following files:
  - `Connector.sln`
  - `Connector/`
  - `Connector.csproj`

---

## Reusing Existing C# Code

### Copy Over Code
For small reusable components, duplicate the necessary classes into your connector project. This approach keeps the connector lightweight and avoids including unnecessary business logic.

### Project Reference
For larger components:

1. Add your existing project to `Connector.sln`.
2. Reference it in `Connector.csproj`.

#### Example:
```
| -> {CompanyName}SourceCode
    | -> {CompanyName}.sln
    | -> Connector.sln
    | -> Connector{ConnectorName}/
        | -> App/
        | -> Client/
        | -> Connections/
        | -> Connector.csproj
    | -> {CompanyName}DataModels/
        | -> {CompanyName}DataModels.csproj
```

Keep connectors minimal to reduce bloat and streamline review processes.

---

## Submission Guidelines

### Binaries and NuGet Packages
- **Prohibited:** Binaries (e.g., DLLs) are not accepted due to security concerns.
- **NuGet Packages:** Approved on a case-by-case basis. If additional tooling is required, contact Product Enablement at [xchange_build@trimble.com](mailto:xchange_build@trimble.com).

### Dry-Run for Review
Use the `--dry-run` option to inspect the code submitted to Xchange:

```bash
xchange code submit --dry-run
```

---

By following these guidelines, you can efficiently build, test, and deploy connectors on the Trimble App Xchange platform. For further support, reach out to [xchange_build@trimble.com](mailto:xchange_build@trimble.com).
