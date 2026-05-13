# Custom Bills

See [Custom Story Fragments](custom-story-fragments.md) for a guide on adding custom story fragments.

The above guide explains how to add a story fragment to the game, but now we need to add functionality to our bill. For this, we can use the `OnBillSigned` and `OnBillVetoed` [events](events.md).

```cs
// -- Core.cs --

// Other code ..

internal sealed class Core : MelonMod
{
    public override void OnInitializeMelon()
    {
        // Listen for these events.
        Events.OnBillSigned += OnBillSigned;
        Events.OnBillVetoed += OnBillVetoed;
    }

    public void OnBillSigned(object sender, Events.BillEventArgs e)
    {
        // Forward to MyBill.
        MyBill.OnBillSigned(e.BillName);
    }

    public void OnBillVetoed(object sender, Events.BillEventArgs e)
    {
        // Forward to MyBill.
        MyBill.OnBillVetoed(e.BillName);
    }
}

// -- MyBill.cs --

// Other code ..

public static class MyBill
{
    public static readonly CustomBillData Data = new(
        // Other arguments ..
        name: "ExampleMod.MyBill");

    internal static void OnBillSigned(string billName)
    {
        if (!Data.Name.Equals(billName, StringComparison.Ordinal))
        {
            return;
        }

        // Do something.
    }

    internal static void OnBillVetoed(string billName)
    {
        if (!Data.Name.Equals(billName, StringComparison.Ordinal))
        {
            return;
        }

        // Do something.
    }
}
```

