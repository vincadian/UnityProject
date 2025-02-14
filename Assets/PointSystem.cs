using UnityEngine;

[CreateAssetMenu(fileName = "PointSystem", menuName = "Game/PointSystem")]
public class PointSystem : ScriptableObject
{
    [System.Serializable]
    public class Bonus
    {
        public bool isActive;
        private event System.Action<bool> _onChanged;

        public void Subscribe(System.Action<bool> handler) => _onChanged += handler;
        public void Unsubscribe(System.Action<bool> handler) => _onChanged -= handler;
        
        public void Toggle() 
        {
            isActive = !isActive;
            _onChanged?.Invoke(isActive);
        }
    }

    // Score implementation
    [SerializeField] private int _score;
    public event System.Action<int> OnScoreChanged;

    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            OnScoreChanged?.Invoke(_score);
        }
    }

    // Bonus instances
    [SerializeField] private Bonus _bonus1 = new Bonus();
    [SerializeField] private Bonus _bonus2 = new Bonus();

    public Bonus Bonus1 => _bonus1;
    public Bonus Bonus2 => _bonus2;

    // Singleton implementation
    private static PointSystem _instance;
    public static PointSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<PointSystem>("PointSystem");
                if (_instance == null)
                {
                    Debug.LogError("Create PointSystem asset via: Create > Game > PointSystem!");
                }
            }
            return _instance;
        }
    }
}