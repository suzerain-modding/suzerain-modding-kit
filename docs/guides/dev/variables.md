# Variables

Use the [Variables](../../api/SuzerainModdingKit.Variables.yml) static class to read and write game variables. You can read/write to vanilla Suzerain variables (such as `BaseGame.GovernmentBudget`) or custom variables.

If you are using custom variables, make sure you register them with `Variables.Register`. If you don't register custom variables, they will not be saved to disk.

If you want to access a vanilla Suzerain variable, but don't know the name, use the [debug overlay](debug-overlay.md) to see all variables in the game.

