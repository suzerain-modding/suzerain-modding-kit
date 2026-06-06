using System.Globalization;
using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppPixelCrushers.DialogueSystem;
using MelonLoader;
using UnityEngine;

namespace SuzerainModdingKit;

internal static class DebugOverlay
{
    public const int OverlayWidthDefault = 250;
    public const int OverlayWidthVarsList = 700;

    private static bool _show;
    private static VariableSearchOverlay _varsList;

    public static bool IsShowing()
    {
        return _show;
    }

    public static void SetVisibility(bool value)
    {
        _show = value;
        _varsList = null;

        // Show/hide the debug console panel. This is not strictly required, so we can ignore
        // if null.
        if (value)
        {
            Panels.Instance?.DebugConsolePanel?.Show();
        }
        else
        {
            Panels.Instance?.DebugConsolePanel?.Hide();
        }
    }

    public static void Update()
    {
        if (!IsShowing())
        {
            return;
        }

        string stepName = GameState.CurrentStepName;

        int overlayWidth = _varsList == null ? OverlayWidthDefault : OverlayWidthVarsList;
        int overlayX = Screen.width - overlayWidth - 10;
        GUILayout.BeginArea(new Rect(overlayX, 10, overlayWidth, overlayWidth));
        GUILayout.BeginVertical(GUI.skin.box);

        GUILayout.Label("Suzerain Modding Kit Debug Overlay");
        GUILayout.Label(stepName ?? "GameFlowManager not loaded!");

        if (_varsList == null)
        {
            if (GUILayout.Button("Hide (Ctrl+D)"))
            {
                SetVisibility(value: false);
            }
            // Only show these buttons if the game is loaded.
            if (stepName != null)
            {
                if (GUILayout.Button("Next Step"))
                {
                    Managers.Instance.GameFlowManager.EndStep();
                    string newStepName = GameState.CurrentStepName;
                    Melon<Core>.Logger.Msg($"Debug overlay: Skipped step '{stepName}'. " +
                        $"New step: '{newStepName}'.");
                }
                if (GUILayout.Button("Next Turn"))
                {
                    Melon<Core>.Logger.Msg("Debug overlay: Showing next turn button.");
                    Managers.Instance.GameFlowManager.ShowEndTurnButton();
                }
                if (GUILayout.Button("Variables"))
                {
                    _varsList = new();
                }
            }
        }
        else
        {
            // Auto hide the vars list if stepName is null (meaning the game is not loaded).
            if (GUILayout.Button("Back") || stepName == null)
            {
                _varsList = null;
            }
            else
            {
                _varsList.Update();
            }
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    public sealed class VariableSearchOverlay
    {
        public const float ItemHeight = 25f;
        public const float ListHeight = 200f;
        public const int ElementPaddingRight = 35;

        private string _searchQuery = "";
        private Vector2 _scrollPos;
        private List<string> _filteredItems;
        private readonly GUIStyle _buttonStyle = new(GUI.skin.button)
        {
            fontSize = 11,
            alignment = TextAnchor.MiddleLeft,
        };
        private VariableEditorOverlay _editor;

        private void ComputeFilteredItems()
        {
            Il2CppStringArray items = DialogueLua.GetAllVariables();

            IEnumerable<string> filtered = string.IsNullOrWhiteSpace(_searchQuery) ? items :
                items.Where(s => s.Contains(_searchQuery, StringComparison.OrdinalIgnoreCase));

            _filteredItems = [.. filtered];
        }

        private void DrawList()
        {
            if (_filteredItems == null)
            {
                throw new InvalidOperationException(
                    "Cannot draw variables list while '_filteredItems' is null");
            }

            int itemCount = _filteredItems.Count;
            float totalHeight = itemCount * ItemHeight;

            _scrollPos = GUILayout.BeginScrollView(_scrollPos, GUILayout.Height(ListHeight));

            int firstVisible = Mathf.Max(0, Mathf.FloorToInt(_scrollPos.y / ItemHeight));
            int lastVisible = Mathf.Min(itemCount - 1,
                                   Mathf.CeilToInt((_scrollPos.y + ListHeight) / ItemHeight));

            // Render only visible items using absolute Rect positioning inside the scroll view.
            for (int i = firstVisible; i <= lastVisible; i++)
            {
                Rect rect = new(0,
                    i * ItemHeight,
                    OverlayWidthVarsList - ElementPaddingRight,
                    ItemHeight);
                string varName = _filteredItems[i];
                if (GUI.Button(rect, varName, _buttonStyle))
                {
                    _editor = new(varName);
                }
            }

            // Reserve the full scroll area height without GUILayout.Space
            // (GUILayout.Space is stripped by Il2Cpp in Suzerain, so we can't use it).
            GUILayout.Label("", GUILayout.Height(totalHeight), GUILayout.Width(0));

            GUILayout.EndScrollView();
        }

        public void Update()
        {
            GUILayout.Label("Variables");

            if (_editor != null)
            {
                if (GUILayout.Button("Back to Search"))
                {
                    _editor = null;
                }
                else
                {
                    _editor.Update();
                }
                return;
            }

            GUILayout.Label("Search:");
            string newSearchQuery = GUILayout.TextField(_searchQuery,
                GUILayout.Width(OverlayWidthVarsList - ElementPaddingRight));

            // Update the filtered items if the search query has changed.
            if (_filteredItems == null ||
                !newSearchQuery.Equals(_searchQuery, StringComparison.OrdinalIgnoreCase))
            {
                ComputeFilteredItems();
            }
            _searchQuery = newSearchQuery;

            DrawList();
        }
    }

    public sealed class VariableEditorOverlay
    {
        public readonly string VariableName;
        private string _resultMessage;
        private string _newValue = "";

        public VariableEditorOverlay(string variableName)
        {
            VariableName = variableName ?? throw new ArgumentNullException(nameof(variableName));
        }

        public void Update()
        {
            GUILayout.Label($"Variable: {VariableName}");
            GUILayout.Label("Value | " +
                $"Bool: {Variables.GetBool(VariableName)} | " +
                $"Float: {Variables.GetFloat(VariableName)
                    .ToString(CultureInfo.InvariantCulture)} | " +
                $"String: {Variables.GetString(VariableName)} |");

            if (!string.IsNullOrEmpty(_resultMessage))
            {
                GUILayout.Label($"Result: {_resultMessage}");
            }

            _newValue = GUILayout.TextField(_newValue,
                GUILayout.Width(OverlayWidthVarsList - VariableSearchOverlay.ElementPaddingRight));

            if (GUILayout.Button("Set Bool"))
            {
                if (bool.TryParse(_newValue, out bool boolValue))
                {
                    _ = Variables.Set(VariableName, boolValue);
                    _resultMessage = $"'{VariableName}' set to {boolValue}.";
                }
                else
                {
                    _resultMessage = $"Error: '{_newValue}' is not a valid bool.";
                }
            }
            if (GUILayout.Button("Set Float"))
            {
                if (float.TryParse(_newValue, NumberStyles.Float, CultureInfo.InvariantCulture,
                    out float floatValue))
                {
                    _ = Variables.Set(VariableName, floatValue);
                    _resultMessage = $"'{VariableName}' set to {floatValue
                        .ToString(CultureInfo.InvariantCulture)}.";
                }
                else
                {
                    _resultMessage = $"Error: '{_newValue}' is not a valid float.";
                }
            }
            if (GUILayout.Button("Set String"))
            {
                _ = Variables.Set(VariableName, _newValue);
                _resultMessage = $"'{VariableName}' set to '{_newValue}'.";
            }
        }
    }
}
