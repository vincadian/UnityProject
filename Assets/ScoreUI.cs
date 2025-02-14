using UnityEngine;
using UnityEngine.UIElements;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    private Label _scoreLabel;

    private void Start()
    {
        if (_uiDocument == null)
        {
            Debug.LogError("Assign UIDocument in Inspector!");
            return;
        }

        VisualElement root = _uiDocument.rootVisualElement;
        _scoreLabel = root.Q<Label>("score-text");

        if (PointSystem.Instance == null)
        {
            Debug.LogError("PointSystem.Instance is null!");
            return;
        }

        // Subscribe to score changes
        PointSystem.Instance.OnScoreChanged += UpdateScoreUI;
        UpdateScoreUI(PointSystem.Instance.Score);
    }

    private void UpdateScoreUI(int newScore)
    {
        _scoreLabel.text = $"Score: {newScore}";
    }

    private void OnDestroy()
    {
        if (PointSystem.Instance != null)
        {
            PointSystem.Instance.OnScoreChanged -= UpdateScoreUI;
        }
    }
}