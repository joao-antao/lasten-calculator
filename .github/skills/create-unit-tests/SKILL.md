---
name: create-unit-tests
description: 'Create backend unit tests for a .NET class — handlers, services, domain models, or infrastructure. Use when asked to create, generate, or scaffold unit tests for backend production code. Reads production code, applies unit testing conventions, generates the test file, and runs tests.'
---

# Create Backend Unit Tests

## When to Use

- "Create unit tests for [handler/service/model]"
- "Generate unit tests for the mortgage calculation domain"

## Procedure

### 1. Identify the Target

Determine the layer:
- **Application** → Handler/service tests (mock dependencies, test business logic)
- **Domain** → Model/value object tests (pure logic, no mocks)
- **Infrastructure** → Repository/client tests (mock external boundaries)

### 2. Read Production Code

Read the class, its constructor dependencies, public methods, business rules, and exception paths.

### 3. Read and Apply Conventions

Read `.github/backend-unit-testing.instructions.md` and follow every convention.

### 4. Ensure the Test Project Exists

Locate the project by layer:
- **Application** → `tests/backend/shared/CostOfLiving.Application.UnitTests/`
- **Domain** → `tests/backend/shared/CostOfLiving.Domain.UnitTests/`
- **Infrastructure** → `tests/backend/shared/CostOfLiving.Infrastructure.UnitTests/`

If it exists, skip to step 5.

Otherwise create it:
- Packages: `xunit`, `xunit.runner.visualstudio`, `Microsoft.NET.Test.Sdk`, `coverlet.collector`, `Bogus`
- Application/Infrastructure tests also need: `NSubstitute`
- Add `ProjectReference` to the production project
- Run `dotnet sln src/backend/CostOfLiving.sln add <path>`

### 5. Check Existing Tests

Review existing test files for related classes to match patterns, Faker setups, and assertion styles.

### 6. Generate, Run, Report

Create the test file mirroring the production code path. Run with `dotnet test --filter "<TestClassName>"`. All tests must pass. If a test fails, fix the test logic or setup — never remove a test, weaken an assertion, or change an expected value to force it to pass. Summarize: file path, test count, coverage areas.
