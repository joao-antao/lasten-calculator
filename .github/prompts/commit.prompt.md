---
name: commit
description: Generate a professional conventional commit message based on staged changes.
---

You are an expert at writing clean, professional Git commit messages following the [Conventional Commits](https://www.conventionalcommits.org/) specification. Add extra detail in a single-paragraph body when needed, and use separate footer lines when applicable. Do not embellish messages.

## Task

Analyze the staged changes and produce a well-structured commit message that:
- Accurately describes **what** changed and **why**
- Enables clean history tracking, changelogs, and semantic versioning

## Conventional Commits Format

```
<type>(<scope>): <short summary>

[optional body]

[optional footer(s)]
```

### Types

| Type       | When to use                                                         |
|------------|---------------------------------------------------------------------|
| `feat`     | A new feature                                                       |
| `fix`      | A bug fix                                                           |
| `docs`     | Documentation changes only                                          |
| `style`    | Formatting, missing semicolons, etc. — no logic change              |
| `refactor` | Code restructuring without adding features or fixing bugs           |
| `perf`     | Performance improvements                                            |
| `test`     | Adding or updating tests                                            |
| `build`    | Changes to build system or external dependencies                    |
| `ci`       | CI/CD configuration changes                                         |
| `chore`    | Maintenance tasks (e.g., updating `.gitignore`, tool configs)       |
| `revert`   | Reverts a previous commit                                           |

### Rules

1. **Subject line**: imperative mood, lowercase, no trailing period, max 72 characters.
2. **Scope**: optional, lowercase, refers to the module/component affected (e.g., `auth`, `api`, `ui`).
3. **Body**: explain the *motivation* and *context*, not the implementation details. Wrap at 72 characters.
4. **Breaking changes**: add `BREAKING CHANGE:` in the footer, or append `!` after the type/scope. Example: `feat(api)!: remove deprecated endpoint`.
5. **Footer**: reference issues or PRs using `Closes #<id>`, `Fixes #<id>`, or `Refs #<id>`.
6. **Branch protection**: never commit or push directly to `main` or `master`. Always work on a feature branch and open a Pull Request for review.

## Examples

**Simple fix:**
```
fix(auth): prevent token expiry on page refresh
```

**Feature with scope and body:**
```
feat(cart): add quantity selector to product cards

Users can now adjust item quantity directly from the product listing
without navigating to the cart, reducing checkout friction.

Closes #142
```

**Breaking change:**
```
feat(api)!: replace REST endpoints with GraphQL

BREAKING CHANGE: All /api/v1/* REST endpoints have been removed.
Consumers must migrate to the new GraphQL endpoint at /graphql.

Refs #98
```

**Chore with no body needed:**
```
chore(deps): upgrade eslint to v9
```

## Instructions

1. **Check your current branch** with `git branch --show-current`. If you are on `main` or `master`, stop and switch to a feature branch first: `git checkout -b <type>/<short-description>`.
2. Stage your changes with `git add .`.
3. Run `git diff --staged` to inspect the staged changes.
4. Identify the most appropriate **type** and **scope**.
5. Write a concise, imperative subject line.
6. Add a body if the change needs context or motivation.
7. Add footer references to issues/PRs when applicable.
8. Flag breaking changes explicitly.
9. Output **only** the final commit message — no extra commentary.
10. Commit using `git commit -m "<subject>" -m "<body>"`, or open the editor with `git commit` to paste the full multi-line message.
11. Push to your feature branch with `git push origin <branch-name>` and open a Pull Request — **never push directly to `main` or `master`**.
