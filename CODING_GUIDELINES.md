# Coding Guidelines

Coding guidelines for contributing to Suzerain Modding Kit.

## Formatting

- Lines should not exceed 100 characters in length. If you're using Visual Studio, you can use the [Editor Guidelines](https://marketplace.visualstudio.com/items?itemName=PaulHarrington.EditorGuidelinesPreview) extension (NOTE: This extension includes telemetry that cannot be disabled. Use [this no telemetry fork](https://github.com/adrianiainlam/EditorGuidelinesNoTelemetry) if you don't want telemetry) to set a guideline at 100 characters.

## General Naming Conventions

- Private properties: `_underscorePrefixedCamelCase`.
- Local variables and parameters: `camelCase`.
- Everything else: `PascalCase`.
- Prefix interfaces with `I`.
- Treat acronyms as whole words unless they are two letters in length (eg. `SmkInfo`, not `SMKInfo` and `ArticyID`, not `ArticyId`).

## Variables

- Avoid using `var`, use explicit type names instead.

## Type Design

- Apply the most restrictive access modifiers for a feature to work.
    - Members within an `internal` class may use `public` instead of `internal`. They will be `internal` regardless of whether `public`/`internal` is specified because the class is `internal`.
- `internal` classes that are not inherited from should always be `sealed`.
- `public` classes should not be `sealed` unless necessary.
- Avoid exposing mutable public fields.

## File and Namespace Structure

- There should be one type per file, and the type should have the same name as the file.
    - **Except for patches.** A file containing patches may have multiple patch classes, and the name of the file may be different.
- Namespaces should match folder hierarchy.

## Patches

- Patch classes should be the patched class name, the patched method name, and the name of the patch (in `PascalCase`) or `Patch`, separated by underscores. For example: `ClassName_MethodName_Patch`, `ClassName_MethodName_PatchThatDoesSomethingSpecific`.
- Multiple patch classes may be contained in one file.
- Patch files should generally live next to their target (eg. `Events.cs` and `EventsPatches.cs`, `Conversation\*.cs` files and `Conversation\Patches.cs`).
- Prefer using `Postfix` over `Prefix`.

## Error Handling

- **Internal error handling:** Errors which are the result of a failure of Suzerain Modding Kit should be handled. An error or warning message should be logged instead of throwing an exception.
    - If the error is in an API method, the method should return a flag indicating to the caller that the method failed (usually `false` or `null`).
- **User error handling:** Errors which are caused by the consumers of the API should throw (eg. throw `ArgumentNullExceptions` and other user errors).
- **Avoiding corrupted state:** Avoid creating corrupted state (eg. halfway registering a custom story fragment) when an error occurs. Validate before performing the action so the method doesn't terminate halfway through the action. If something can't be validated beforehand, use default values or roll back the state before terminating.

## Diagnostic Messages

- Diagnostic messages should be clear and grammatically correct (start with a capital letter, end with period, etc). Definite entities (variable names, IDs, etc) should be surrounded in single quotes (eg. "The delegate 'SomeDelegateName' ...").
- **Logging Severity:**
    - **Error:** Use when something causes the method to terminate. The method should return a flag indicating that the method failed (see Error Handling).
    - **Warning:** Use when something goes wrong, but the method still continues.
    - **Info (Msg):** Use to log something noteworthy that isn't an error or warning.
