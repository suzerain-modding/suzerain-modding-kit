# Lua in the Dialogue System

Suzerain uses Dialogue System for conversations and variables. You can embed the [Lua](https://www.lua.org/) scripting language in conversation nodes using the `luaScript` and `luaCondition` arguments as described in [Editing Conversations](editing-conversations.md#conditions-scripts-and-sequences).

You can access variables from Lua using the `Variable` table.

```lua
-- luaCondition: Check if government budget is greater than 2.
Variable["BaseGame.GovernmentBudget"] > 2

-- luaScript: Set SomethingHappened to true when this node plays.
Variable["ExampleMod.SomethingHappened"] = true
```

