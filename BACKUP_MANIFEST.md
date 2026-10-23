# Resolver Reach project backup manifest

Snapshot date: 2026-10-23  
Current primary repository: https://github.com/PCUser2021/ResolverReach  
Repository visibility: **private**  
Product version: **Resolver Reach 2.0.0**  
Primary source history includes both Resolver Reach and Ollama GGUF Importer.

## Authoritative tracked backup

The private Git repository is the authoritative, versioned backup of source and project records. Do not make this repository public: it contains implementation source for Resolver Reach and Ollama GGUF Importer, plus historical commits and CI configuration.

- Current main at backup preparation: commit `026fda8d8e8bc6eafdbf2bda7354f3acdac6227e`.
- Resolver Reach 2.0.0 redesign merge: commit `35ff1517f7e0c0ce42fe1a8500935123d922dd30`.
- Successful Windows x64 publish/installer build: [workflow run #39928118989](https://github.com/PCUser2021/ResolverReach/actions/runs/39928118989).
- Project source and icon: `src/ResolverReach/`.
- Packaging and CI: `installer/ResolverReach.iss`, `.github/workflows/windows-build.yml`.
- Product documentation and durable working context: `README.md`, `CHANGELOG.md`, `PROJECT_CONTEXT.md`, `DEVELOPMENT_LOG.md`, `PROMPT_ARCHIVE.md`.
- Public-only draft copy (docs/assets; not source): `public-distribution/`.
- Distribution constraints and publish steps: `PUBLIC_DISTRIBUTION_PLAN.md`.

A backup branch is to be created at the final commit produced by this task: `backup/ResolverReach-2.0.0-2026-10-23`. Verify that branch after creation before relying on it.

## Verified shipped binary

- Installer file: `Resolver-Reach-2.0.0-Setup-x64.exe`.
- Size: 33,971,274 bytes.
- SHA-256: `0023ab33844ba6ffe00f572eb0ed28e395b3e46044fb392f893726f20df2bc22`.
- GitHub Actions artifact: `ResolverReach-Windows-x64`, artifact ID `12490750995`; temporary artifact expiry listed as 2026-11-06.
- Build/installer success does not prove live reachability for each DNS resolver.

## Public-only distribution kit prepared in the current working session

Local ZIP: `/mnt/data/ResolverReach-Public-Distribution-Kit.zip`  
ZIP SHA-256: `0295e351aa532b70f8e84d83054d0b9e78a9f31301a61b70008188f2c0a9030b`.

It contains only the installer, public-facing README, requested personal/home-use terms, the proposed repository description, a brand icon illustration, and a clearly labelled UI design preview. It contains **no source files**. The preview is an illustration based on the UI layout, not a screenshot captured from a running app.

## Visibility constraint and not-yet-completed external action

GitHub's own documentation says that changing a private repository to public makes the code visible to everyone and allows anyone to fork it: https://docs.github.com/en/repositories/managing-your-repositorys-settings-and-features/managing-repository-settings/setting-repository-visibility

To satisfy both privacy goals, keep this source repository private and publish the prepared kit in a separate public distribution repository (suggested name: `ResolverReach-Downloads`). In this session's available GitHub connection, no action is provided to create a repository or change repository visibility. Therefore the public repository has **not** been created and the current private repository has intentionally **not** been exposed.

## ChatGPT memory limitation

The complete ChatGPT platform-level Memory store and every historical chat cannot be exported through the connected tools. Repository-related prompts, decisions, source changes, build results, and handoff details that are available have been persisted in `PROMPT_ARCHIVE.md`, `PROJECT_CONTEXT.md`, and `DEVELOPMENT_LOG.md`. This record must not be described as a complete ChatGPT Memory export.
