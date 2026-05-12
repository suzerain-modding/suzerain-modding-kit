# Events

## Suzerain Lifecycle Events

Use the [Events](../../api/SuzerainModdingKit.Events.yml) static class to register listeners for Suzerain lifecycle events.

```cs
// Other code ..

internal sealed class Core : MelonMod
{
    public override void OnInitializeMelon()
    {
        // Other code ..

        // You can subscribe to an event at any time.
        // We want these events to be added as soon as our mod initializes,
        // so we do it in OnInitializeMelon.
        Events.OnEvaluateStep += OnEvaluateStep;
        Events.OnDecisionFinished += OnDecisionFinished;
        // Use -= to remove an event listener:
        // Events.OnDecisionFinished -= OnDecisionFinished;

        // Other code ..
    }

    // All event listeners must take (object, EventArgs).
    // The first argument (sender) will always be null.
    // The second argument (e) depends on the event you are subscribing to.
    // In this case, it is an empty EventArgs object.
    public void OnEvaluateStep(object sender, EventArgs e)
    {
        // Do something.
    }

    // Some events, such as this one, pass a different subtype of EventArgs.
    // See the reference documentation for more details.
    public void OnDecisionFinished(object sender, Events.DecisionFinishedEventArgs e)
    {
        // Do something.
    }
}
```

## MelonLoader Events

See [Melon Callbacks](https://melonwiki.xyz/#/modders/quickstart?id=melon-callbacks).

