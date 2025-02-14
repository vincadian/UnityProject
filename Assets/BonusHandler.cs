using UnityEngine;
using UnityEngine.UIElements;

public class BonusHandler : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private PointSystem _pointSystem;

    private Button[] _buttons = new Button[2];
    private Label[] _labels = new Label[2];
    private System.Action<bool>[] _handlers = new System.Action<bool>[2];
    private VisualElement _mainPanel;

    private void Start()
    {
        VisualElement root = _uiDocument.rootVisualElement;

        // Get reference to main UI container
        _mainPanel = root.Q<VisualElement>("main-panel");
        if (_mainPanel == null)
        {
            Debug.LogError("Main panel element not found!");
            return;
        }

        // Hide UI initially
        _mainPanel.style.display = DisplayStyle.None;

        // Initialize Bonuses
        _buttons[0] = root.Q<Button>("bonus1-btn");
        _labels[0] = root.Q<Label>("bonus1-status");
        SetupBonus(0);

        _buttons[1] = root.Q<Button>("bonus2-btn");
        _labels[1] = root.Q<Label>("bonus2-status");
        SetupBonus(1);

        // Subscribe to score changes
        _pointSystem.OnScoreChanged += CheckScoreThreshold;
    }

    private void CheckScoreThreshold(int newScore)
    {
        if (newScore >= 10)
        {
            ShowBonusUI();
            // Unsubscribe so it only shows once
            _pointSystem.OnScoreChanged -= CheckScoreThreshold;
        }
    }

    private void ShowBonusUI()
    {
        _mainPanel.style.display = DisplayStyle.Flex;
    }

    private void SetupBonus(int index)
    {
        _handlers[index] = (state) => UpdateBonusDisplay(index, state);
        
        PointSystem.Bonus bonus = index == 0 ? 
            _pointSystem.Bonus1 : 
            _pointSystem.Bonus2;

        bonus.Subscribe(_handlers[index]);
        UpdateBonusDisplay(index, bonus.isActive);
        
        _buttons[index].clicked += () => 
        {
            bonus.Toggle();
            HideBonusUI();
        };
    }

    private void UpdateBonusDisplay(int index, bool isActive)
    {
        _buttons[index].text = $"Bonus {index+1}: {(isActive ? "ON" : "OFF")}";
        _labels[index].text = $"Status: {(isActive ? "Active" : "Inactive")}";
    }

    private void HideBonusUI()
    {
        _mainPanel.style.display = DisplayStyle.None;
    }

    private void OnDestroy()
    {
        _pointSystem.OnScoreChanged -= CheckScoreThreshold;
        
        for(int i = 0; i < 2; i++)
        {
            if (_handlers[i] != null)
            {
                PointSystem.Bonus bonus = i == 0 ? 
                    _pointSystem.Bonus1 : 
                    _pointSystem.Bonus2;
                bonus.Unsubscribe(_handlers[i]);
            }
        }
    }
}