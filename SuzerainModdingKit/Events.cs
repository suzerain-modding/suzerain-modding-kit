using MelonLoader;
using SuzerainModdingKit.StoryFragments.Decision;

namespace SuzerainModdingKit;

/// <summary>
/// Suzerain lifecycle events.
/// </summary>
public static class Events
{
    // This class is organized in the following order:
    // 1. Event properties:
    //   1a. Before events. Alphabetical order.
    //   1b. After (On) events. Alphabetical order.
    // 2. EventArgs classes: Same order as properties.
    // 3. Trigger methods: Same order as properties.
    // 4. Private methods.

    /// <summary>
    /// Called before a step ends.
    /// </summary>
    public static event EventHandler BeforeStepEnd;
    /// <summary>
    /// Called when a bill is signed by the player.
    /// </summary>
    public static event EventHandler<BillEventArgs> OnBillSigned;
    /// <summary>
    /// Called when a bill is vetoed by the player.
    /// </summary>
    public static event EventHandler<BillEventArgs> OnBillVetoed;
    /// <summary>
    /// Called when the player selects an option in a decision.
    /// </summary>
    public static event EventHandler<DecisionFinishedEventArgs> OnDecisionFinished;
    /// <summary>
    /// Called when the game shows a decision.
    /// </summary>
    public static event EventHandler OnDecisionShow;
    /// <summary>
    /// Called when a step is evaluated. Note that this may be called multiple times per step.
    /// </summary>
    public static event EventHandler OnEvaluateStep;
    /// <summary>
    /// Called when the journal is initialized. Note that this may be called multiple times.
    /// </summary>
    public static event EventHandler OnJournalInitialized;
    /// <summary>
    /// Called when a turn ends.
    /// </summary>
    public static event EventHandler OnTurnEnd;

    /// <summary>
    /// Event args passed to <c cref="OnBillSigned">OnBillSigned</c> and
    /// <c cref="OnBillVetoed">OnBillVetoed</c> events.
    /// </summary>
    public class BillEventArgs : EventArgs
    {
        /// <summary>
        /// The name of the bill.
        /// </summary>
        public string BillName
        {
            get;
        }

        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        /// <param name="billName">
        /// The name of the bill.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any required arguments are null.
        /// </exception>
        public BillEventArgs(string billName)
        {
            BillName = billName ?? throw new ArgumentNullException(nameof(billName));
        }
    }

    public class DecisionFinishedEventArgs : EventArgs
    {
        public DecisionOptionInfo SelectedOptionInfo
        {
            get;
        }

        public DecisionFinishedEventArgs(DecisionOptionInfo selectedOptionInfo)
        {
            SelectedOptionInfo = selectedOptionInfo ??
                throw new ArgumentNullException(nameof(selectedOptionInfo));
        }
    }

    internal static void TriggerBeforeStepEnd()
    {
        SafeInvoke(BeforeStepEnd, nameof(BeforeStepEnd));
    }

    internal static void TriggerOnBillSigned(string billName)
    {
        SafeInvoke(OnBillSigned, nameof(OnBillSigned), new BillEventArgs(billName));
    }

    internal static void TriggerOnBillVetoed(string billName)
    {
        SafeInvoke(OnBillVetoed, nameof(OnBillVetoed), new BillEventArgs(billName));
    }

    internal static void TriggerOnDecisionFinished(DecisionOptionInfo selectedOptionInfo)
    {
        SafeInvoke(OnDecisionFinished, nameof(OnDecisionFinished),
            new DecisionFinishedEventArgs(selectedOptionInfo));
    }

    internal static void TriggerOnDecisionShow()
    {
        SafeInvoke(OnDecisionShow, nameof(OnDecisionShow));
    }

    internal static void TriggerOnEvaluateStep()
    {
        SafeInvoke(OnEvaluateStep, nameof(OnEvaluateStep));
    }

    internal static void TriggerOnJournalInitialized()
    {
        SafeInvoke(OnJournalInitialized, nameof(OnJournalInitialized));
    }

    internal static void TriggerOnTurnEnd()
    {
        SafeInvoke(OnTurnEnd, nameof(OnTurnEnd));
    }

    private static void SafeInvoke(
        Delegate eventHandler,
        string eventName,
        EventArgs eventArgs = null)
    {
        if (eventHandler == null)
        {
            return;
        }
        string name = eventName ?? "undefined";
        EventArgs args = eventArgs ?? EventArgs.Empty;

        foreach (Delegate subscriber in eventHandler.GetInvocationList())
        {
            try
            {
                _ = subscriber.DynamicInvoke([null, args]);
            }
            catch (Exception ex)
            {
                string delegateName =
                    $"{subscriber.Method.DeclaringType.FullName}.{subscriber.Method.Name}";

                Melon<Core>.Logger.Error($"Delegate '{delegateName}' " +
                    $"for event '{name}' threw an exception: {ex}");
            }
        }
    }
}
