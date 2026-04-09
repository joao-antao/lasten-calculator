---
name: create-component-tests
description: 'Create backend component tests for a .NET endpoint, worker, or filter. Use when asked to create, generate, or scaffold component tests for backend production code. Reads production code, applies component testing conventions, generates the test file, and runs tests.'
---

# Create Backend Component Tests

## When to Use

- "Create component tests for [endpoint/worker/filter]"
- "Generate component tests for the ingestor worker"

## Procedure

### 1. Identify the Target

Determine the type:
- **Endpoint** → BFF pipeline test (HTTP through WebApplicationFactory)
- **Worker/Job** → Ingestor pipeline test (IHost with WireMock)
- **Filter** → Isolated filter test (DefaultHttpContext)

### 2. Read Production Code

Read the endpoint/worker/filter, its request/response models, validation logic, and handler.

### 3. Read and Apply Conventions

Read `.github/backend-component-testing.instructions.md` and follow every convention.

### 4. Ensure the Test Project Exists

If the `.ComponentTests` project exists, skip to step 5.

Otherwise create it:
- BFF → `tests/backend/bff/CostOfLiving.Bff.ComponentTests/`
- Workers → `tests/backend/jobs/CostOfLiving.<Name>.ComponentTests/`
- BFF packages: `xunit`, `xunit.runner.visualstudio`, `Microsoft.NET.Test.Sdk`, `coverlet.collector`, `Bogus`, `Microsoft.AspNetCore.Mvc.Testing`
- Worker packages: `xunit`, `xunit.runner.visualstudio`, `Microsoft.NET.Test.Sdk`, `coverlet.collector`, `NSubstitute`, `WireMock.Net`, `Microsoft.Extensions.DependencyInjection`, `Microsoft.AspNetCore.Mvc.Testing`
- Add `ProjectReference` to the production project
- Run `dotnet sln src/backend/CostOfLiving.sln add <path>`
- Create base infrastructure following conventions

### 5. Check Existing Infrastructure

Reuse what exists in the target test project. Do not assume all projects have the same folders:
- BFF has: `Helpers/`, `Stubs/`
- Ingestor has: `Extensions/`, `Mocks/`

Only create new helpers/stubs for uncovered dependencies.

### 6. Generate, Run, Report

Create the test file in the appropriate folder. Run with `dotnet test --filter "<TestClassName>"`. All tests must pass. If a test fails, fix the test logic or setup — never remove a test, weaken an assertion, or change an expected value to force it to pass. Summarize: file path, test count, coverage areas.
