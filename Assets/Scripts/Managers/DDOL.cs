using UnityEngine;

public class DDOL : MonoBehaviour
{
    private static DDOL _instance;
    public static DDOL Instance => _instance;

    [Header("Managers")] 
    [SerializeField] private GameController _gameController;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private AudioManager _audioManager;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        _gameController.Initialize();
        _uiManager.Initialize();
        _audioManager.Initialize();
    }
    
    public void StartLevel(int level)
    {
        LevelData levelData = _gameController.GetLevelData(level);
        _gameController.StartGame(level);
        _uiManager.SetupPlayGrid(levelData.rowCount, levelData.columnCount, _gameController.LevelSprites, _gameController.LevelIndexes);
        
        GameStateUpdate();
    }

    public void CardSelected(CardController card)
    {
        _gameController.CardSelected(card);
    }

    public void GameStateUpdate()
    {
        _uiManager.UpdateGameStateUI(_gameController.Score, _gameController.NumberOfTriesLeft);
        
        if (_gameController.NumberOfTriesLeft <= 0)
        {
            _uiManager.GameOver();
        }
    }
}
