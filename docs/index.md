---
_layout: landing
---

# Suzerain Modding Kit

An API for creating Suzerain mods with MelonLoader.

**Suzerain Modding Kit is currently in beta** and should not be considered stable. Expect bugs and crashes.

## Overview

Suzerain Modding Kit provides an API for modders to extend Suzerain using C# and MelonLoader. The API currently includes support for:

- New bills, decisions, reports, and journal entries.
- Editing and creating new dialogue for conversations.
- Adding new options to decisions.
- Subscribing to game events (eg. `OnBillSigned`, `OnBillVetoed`, `OnDecisionFinished`).
- Supports both Sordland and Rizia.

## Compatibility

| Platform           | Support          |
| ------------------ | ---------------- |
| Windows            | ✅ Supported     |
| Linux (via Proton) | ✅ Supported     |
| macOS              | ❌ Not Supported |

Only the Steam version of Suzerain is supported.

## Getting Started

**How do I install mods?** See [Installing Mods](https://suzerain-modding.github.io/suzerain-modding-kit/guides/user/installing-mods.html).

**How do I develop mods?** See [Developing Mods](https://suzerain-modding.github.io/suzerain-modding-kit/guides/dev/developing-mods.html).

## Versioning

SMK uses a two-component versioning scheme: `era.release`.

- `era` increments when a planned milestone ships. This _usually_ indicates a content update with new features.
- `release` increments when any release ships. Incrementing this without incrementing `era` _usually_ indicates bug fixes, small changes, and/or compatibility with a new Suzerain version.

**Both components may contain breaking changes.** This is because Suzerain updates can release at any time and may force a breaking change in SMK.

The `release` component is the primary signal for staying up to date as it increments for every update. `era` exists to track and plan content updates.

We aim to deprecate APIs before removing them, ideally allowing at least one release before removal. However, **this is not a guarantee** as Suzerain updates may force immediate removal.

> This versioning system was adopted on June 9th, 2026.

## Contributing

Contributions are welcome. See [CONTRIBUTING.md](https://github.com/suzerain-modding/suzerain-modding-kit/blob/main/CONTRIBUTING.md) for more information.

## License

This project is licensed under the [ISC License](https://github.com/suzerain-modding/suzerain-modding-kit/blob/main/LICENSE).

## Disclaimer

Suzerain Modding Kit is an unofficial, community-made API for modding Suzerain. It is not affiliated with, endorsed by, or sponsored by Torpor Games. Suzerain is the property of Torpor Games.
