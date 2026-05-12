# Custom Story Fragments

To add a custom story fragment, call [GameState.AddCustomStoryFragment](../../api/SuzerainModdingKit.GameState.yml) in the `OnEvaluateStep` [event](events.md).

See [DecisionExample](https://github.com/suzerain-modding/suzerain-mod-examples/tree/main/DecisionExample), an example mod that adds a custom decision to the game.

```cs
// The following code is an edited version of DecisionExample,
// containing only the most relevant parts.

// -- Core.cs --

// Other code ..

internal sealed class Core : MelonMod
{
    public override void OnInitializeMelon()
    {
        // Listen for these events.
        Events.OnEvaluateStep += OnEvaluateStep;

        // Other code ..
    }

    public void OnEvaluateStep(object sender, EventArgs e)
    {
        // Forward to SpendBudgetDecision.
        SpendBudgetDecision.OnEvaluateStep();
    }

    // Other code ..
}

// -- SpendBudgetDecision.cs --

// Other code ..

public static class SpendBudgetDecision
{
    // Other code ..

    // The properties for this decision.
    public static readonly CustomDecisionData Data = new(
        // The unique name of the decision.
        name: "DecisionExample.MyDecision",
        // The story pack that this decision will be displayed in.
        storyPack: SuzerainStoryPackInfo.Sordland,
        // The token that this decision should be displayed on.
        assignedTokenName: SuzerainTokenName.SordlandCityHolsord,
        // The title of the decision when viewed in the decision panel.
        title: "The Budget Question",
        // The description of the decision when viewed in the decision panel.
        description: "We are faced with a choice: Spend government budget for no reason, or don't.",
        // The title of the decision when viewed under the assigned token.
        hubTitle: "The Budget Question",
        // The description of the decision when viewed under the assigned token.
        hubDescription: "Should we spend budget?");

    // Other code ..

    internal static void OnEvaluateStep()
    {
        // If we're not in Sordland on turn 1 step 2, return.
        if (!GameState.IsCurrentStoryPack(SuzerainStoryPackInfo.Sordland) ||
            GameState.CurrentTurnNum != 1 ||
            GameState.CurrentStepNum != 2)
        {
            return;
        }

        // Return if the decision has already been added.
        if (GameState.StoryFragmentExistsInCurrentStep(Data.Name))
        {
            return;
        }

        // Other code ..

        // Add the story fragment to the game. We don't care about the return value, discard it.
        _ = GameState.AddCustomStoryFragment(Data);
    }

    // Other code ..
}
```

