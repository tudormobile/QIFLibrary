# Contributing to QIFLibrary

Thank you for contributing to QIFLibrary. This document explains how to contribute, what to expect when you open issues or pull requests, and the standards we follow for code, tests, and documentation.

## Table of Contents

- Getting started
- Code style and formatting
- Branching and pull requests
- Commit messages
- Tests
- Documentation
- Reviews and merging
- Continuous integration
- Reporting security issues

## Getting started

1. Fork the repository and clone your fork.
2. Create a feature branch off `main` for your work:

   ```bash
   git checkout main
   git pull origin main
   git checkout -b feat/short-description
   ```

3. Make small, focused commits. Rebase interactively to clean up commits before opening a PR.

## Code style and formatting

This repository targets .NET 9 and follows a strict style enforced by the repository `.editorconfig`. Always ensure your code matches these rules before opening a PR. Key points:

- Use spaces for indentation; C# files use 4 spaces per indent.
- Place `using` directives outside namespaces.
- Prefer explicit braces and block-scoped namespaces.
- Expression-bodied members are not preferred for methods/constructors/operators.

Before submitting, run the formatter in Visual Studio 2022 using:

- __Edit > Advanced > Format Document__ (or the keyboard shortcut)

Or use `dotnet format` locally to apply .editorconfig rules:

```bash
dotnet tool install -g dotnet-format --version 6.* || true
dotnet format
```

## Branching and pull requests

- Base new work on `main` (the main branch).
- One feature/fix per branch.
- Keep PRs small and focused.

When opening a PR:

- Target branch: `main`.
- Provide a clear title and description describing the problem, the change, and any migration notes.
- Reference related issues using `#<issue-number>`.

## Commit messages

Follow this simple convention:

- Include a short summary (<= 50 chars) and a longer description if needed.

Example:

```
Add QIFRecordBuilder.Parse method

Support for parsing monetary fields with culture-invariant rules. Adds unit tests.
```

## Tests

- Unit tests live in the `src/*Tests` projects.
- All code changes must include unit tests that cover new behavior and edge cases.
- Run tests locally with:

```bash
dotnet test
```

- Aim for deterministic tests; avoid network or time-dependent flakiness.

## Documentation

The `docs/` folder contains repository documentation. Most of it is generated, but the introduction/overview is not.

Guidelines for docs updates:

- Keep docs in Markdown.
- Ensure complete code comments (generated into documentation).
- If you add public API surface, update any reference docs and samples.
- If you add public API surface, check the introduction/overview docs for appropriate changes.

If you're changing formatting or coding standards, update `.editorconfig` and notify reviewers. Major documentation or design decisions should be added to the `docs/` folder and linked from `README.md`.

## Reviews and merging

- Assign reviewers via GitHub.
- Address review comments promptly; push follow-up commits.
- Squash or rebase to keep history readable when merging if requested by reviewers.
- Only owners may merge into `main` when CI passes.

## Continuous integration

This repository uses GitHub Actions for CI. PRs must pass build, test, and style checks before they can be merged.

## Reporting security issues

Do not open public issues for security-sensitive things. Contact me directly.

---

Thank you for helping improve QIFLibrary. If any part of this guide needs clarification, open an issue titled "Docs: CONTRIBUTING update" with suggested changes.