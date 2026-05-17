using Il2Cpp;
using MelonLoader;
using SuzerainModdingKit.Report;
using SuzerainModdingKit.StoryFragments;
using SuzerainModdingKit.StoryPack;

namespace SuzerainModdingKit;

/// <summary>
/// Static interface for querying and modifying the current state of the game.
/// </summary>
public static class GameState
{
    private static GameFlowManager _gameFlowManager => Managers.Instance?.GameFlowManager;

    /// <summary>
    /// The current step number.
    /// </summary>
    public static int? CurrentStepNum => _gameFlowManager?.CurrentStepNo;
    /// <summary>
    /// The current step name (eg. "Sordland_Turn_2_Step_3"). Note that the format of step names
    /// may vary between story packs.
    /// </summary>
    public static string CurrentStepName => _gameFlowManager?.currentStepData?.NameInDatabase;
    /// <summary>
    /// The current turn number.
    /// </summary>
    public static int? CurrentTurnNum => _gameFlowManager?.CurrentTurnNo;
    /// <summary>
    /// The current turn name (eg. "Sordland_Turn_2"). Note that the format of step names
    /// may vary between story packs.
    /// </summary>
    public static string CurrentTurnName => _gameFlowManager?.currentTurnData?.NameInDatabase;
    /// <summary>
    /// The name of the current story pack.
    /// </summary>
    public static string CurrentStoryPackName => _gameFlowManager?.currentTurnData
        ?.TurnProperties?.AssignedStoryPack;
    /// <summary>
    /// Is the game active? The game is "active" when the current step name is not null.
    /// </summary>
    public static bool IsGameActive => CurrentStepName != null;

    /// <summary>
    /// Check if the current story pack matches the given
    /// <c cref="StoryPackInfo">StoryPackInfo</c>.
    /// </summary>
    /// <param name="storyPack">
    /// The <c cref="StoryPackInfo">StoryPackInfo</c> to test against.
    /// </param>
    /// <returns>
    /// A boolean indicating whether the current story pack matches the given
    /// <c cref="StoryPackInfo">StoryPackInfo</c>.
    /// </returns>
    public static bool IsCurrentStoryPack(StoryPackInfo storyPack)
    {
        return string.Equals(
            storyPack?.StoryPackName,
            CurrentStoryPackName,
            StringComparison.Ordinal);
    }

    /// <summary>
    /// Check if a story fragment exists in the current step.
    /// </summary>
    /// <param name="name">
    /// The name of the story fragment.
    /// </param>
    /// <returns>
    /// A boolean indicating whether the story fragment exists in the current step.
    /// </returns>
    public static bool StoryFragmentExistsInCurrentStep(string name)
    {
        return _gameFlowManager?.currentStepData?.StoryFragments?.Contains(name) ?? false;
    }

    /// <summary>
    /// Check if a report exists in the current turn.
    /// </summary>
    /// <param name="name">
    /// The name of the report.
    /// </param>
    /// <returns>
    /// A boolean indicating whether the report exists in the current turn.
    /// </returns>
    public static bool ReportExistsInCurrentTurn(string name)
    {
        Func<ReportData, bool> match = data =>
            string.Equals(data.NameInDatabase, name, StringComparison.Ordinal);
        return _gameFlowManager?.currentTurnReports?.Exists(match) ?? false;
    }

    /// <summary>
    /// Add a custom story fragment to the current step.
    /// </summary>
    /// <param name="customStoryFragmentData">
    /// The custom story fragment data.
    /// </param>
    /// <returns>
    /// A boolean indicating whether the operation succeeded or not.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the game is not active.
    /// </exception>
    public static bool AddCustomStoryFragment(CustomStoryFragmentData customStoryFragmentData)
    {
        // This method adds a custom story fragment to the current step dynamically.
        // We don't use Suzerain's registry because articy:expresso scripts
        // don't recognize custom variables.

        ArgumentNullException.ThrowIfNull(customStoryFragmentData);
        ThrowIfInactive();

        // Turn 1 Step 1 only contains the beginning scene (inauguration, coronation, etc).
        // Trying to add a story fragment during this step will throw an exception.
        if (CurrentTurnNum == 1 && CurrentStepNum == 1)
        {
            Melon<Core>.Logger.Error("Cannot add story fragments on Turn 1 Step 1. " +
                "Turn 1 Step 2 is the earliest step that supports custom story fragments.");
            return false;
        }

        if (StoryFragmentExistsInCurrentStep(customStoryFragmentData.Name))
        {
            Melon<Core>.Logger.Error(
                $"A story fragment with the name '{customStoryFragmentData.Name}' already " +
                "exists in the current step.");
            return false;
        }

        StoryFragmentData registeredData = customStoryFragmentData.RegisterInSuzerain();

        // Add it to the scene.
        _gameFlowManager.currentStepData.StoryFragments.Add(customStoryFragmentData.Name);
        _gameFlowManager.EvaluateStoryFragment(
            isEnabled: true,
            isDone: false,
            registeredData,
            customStoryFragmentData.Name);

        // Create the exclamation icon.
        bool didCreateIndicatorSuccessfully = CreateTokenIndicator(
            customStoryFragmentData.AssignedTokenName,
            TokenIndicatorPanel.TokenIndicatorType.StoryFragment);
        if (!didCreateIndicatorSuccessfully)
        {
            Melon<Core>.Logger.Warning(
                $"Added story fragment '{customStoryFragmentData.Name}' to current step with " +
                "warnings.");
        }

        return true;
    }

    /// <summary>
    /// Add a custom report to the current turn.
    /// </summary>
    /// <param name="customReportData">
    /// The custom report data.
    /// </param>
    /// <returns>
    /// A boolean indicating whether the operation succeeded or not.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the game is not active.
    /// </exception>
    public static bool AddCustomReport(CustomReportData customReportData)
    {
        ArgumentNullException.ThrowIfNull(customReportData);
        ThrowIfInactive();

        // Turn 1 Step 1 only contains the beginning scene (inauguration, coronation, etc).
        // Trying to add a story fragment during this step will throw an exception.
        if (CurrentTurnNum == 1 && CurrentStepNum == 1)
        {
            Melon<Core>.Logger.Error("Cannot add reports on Turn 1 Step 1. " +
                "Turn 1 Step 2 is the earliest step that supports custom reports.");
            return false;
        }

        if (ReportExistsInCurrentTurn(customReportData.Name))
        {
            Melon<Core>.Logger.Error(
                $"A report with the name '{customReportData.Name}' already " +
                "exists in the current turn.");
            return false;
        }

        ReportData registeredData = customReportData.RegisterInSuzerain((int)CurrentTurnNum);
        _gameFlowManager.currentTurnReports.Add(registeredData);
        _gameFlowManager.enabledNotDoneReportData.Add(registeredData);

        bool didCreateIndicatorSuccessfully = CreateTokenIndicator(
            customReportData.AssignedTokenName,
            TokenIndicatorPanel.TokenIndicatorType.StoryFragment);
        if (!didCreateIndicatorSuccessfully)
        {
            Melon<Core>.Logger.Warning(
                $"Added report '{customReportData.Name}' to current step with " +
                "warnings.");
        }

        return true;
    }

    internal static void ThrowIfInactive()
    {
        if (!IsGameActive)
        {
            throw new InvalidOperationException("The game is not active.");
        }
    }

    private static bool CreateTokenIndicator(string assignedTokenName,
        TokenIndicatorPanel.TokenIndicatorType indicatorType)
    {
        // This method uses warnings instead of errors even when it causes the method to
        // terminate because this method is called when adding a custom story fragment,
        // which is successful regardless of whether the token indicator is created or not.

        ThrowIfInactive();

        TokenIndicatorPanel panel = Panels.Instance.TokenIndicatorPanel;

        // Check if the token indicator we're trying to create already exists.
        Func<TokenIndicatorPanel.TokenIndicator, bool> match =
            ind => string.Equals(
                ind.token.tokenDataEntityName,
                assignedTokenName,
                StringComparison.Ordinal) && ind.tokenIndicatorType == indicatorType;
        bool exists = panel.tokenIndicators.Exists(match);
        if (exists)
        {
            return true;
        }

        // TryAddTokenIndicator creates the token indicator and adds it to the list.
        panel.TryAddTokenIndicator(
            assignedTokenName,
            indicatorType,
            panel.tokenIndicators);

        // Get the indicator we just created.
        TokenIndicatorPanel.TokenIndicator newIndicator = panel.tokenIndicators.Find(match);
        if (newIndicator == null)
        {
            Melon<Core>.Logger.Warning(
                $"Failed to create token indicator for token '{assignedTokenName}'.");
            return false;
        }

        // Get a template. For some reason, panel.templateTokenIndicator doesn't work,
        // so we just grab the first one from the list of instantiated token indicators.
        bool isAnyTemplateAvailable =
            panel.instantiatedTokenIndicators != null &&
            panel.instantiatedTokenIndicators.Count > 1;
        TemplateTokenIndicator indicatorTemplate = isAnyTemplateAvailable
            ? panel.instantiatedTokenIndicators[0]
            : null;
        if (indicatorTemplate == null)
        {
            Melon<Core>.Logger.Warning(
                $"Failed to create token indicator for token '{assignedTokenName}'. No template " +
                "object found.");
            return false;
        }

        // Create the game object.
        TemplateTokenIndicator indicatorObj = UnityEngine.Object.Instantiate(
            indicatorTemplate,
            indicatorTemplate.transform.parent);
        indicatorObj.Setup(newIndicator);
        panel.instantiatedTokenIndicators.Add(indicatorObj);

        return true;
    }
}
