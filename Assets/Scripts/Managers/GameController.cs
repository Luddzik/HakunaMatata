using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Game Data")]
    [SerializeField] private SOLevelData _gameDataSO;
    [SerializeField] private SOSprites _spritesSO;

    public int NumberOfTriesLeft => _numberOfTriesLeft;
    public int Score => _score;
    public int CurrentPairCount => _currentPairCount;
    public int LevelPairCount => _levelPairCount;
    
    public int LoadedLevel => _loadedLevel;
    
    public List<Sprite> LevelSprites => _levelSprites;
    public List<int> LevelIndexes => _levelIndexes;

    private List<Sprite> _levelSprites;
    private List<int> _levelIndexes;

    private CardController _currentSelection;

    private int _loadedLevel;
    private int _currentPairCount;
    private int _levelPairCount;
    private int _numberOfTriesLeft;
    private int _combo;
    private int _score;

    public void Initialize()
    {
        Debug.Log("Initializing game controller");
    }
    
    public void StartGame(int level)
    {
        SetupLevel(level);
    }
    
    public int GetLevelCount()
    {
        return _gameDataSO.GetLevelCount();
    }
    
    public LevelData GetLevelData(int level)
    {
        if (level >= _gameDataSO.GetLevelCount())
        {
            Debug.Log("No new levels available. Returning to main menu.");
            LevelData levelData = new LevelData();
            levelData.level = -1;
            return levelData;
        }
        
        return _gameDataSO.GetLevelData(level);
    }
    
    public void CardSelected(CardController cardController)
    {
        Debug.Log("Card selected: " + cardController.CardIndex);
        
        if (_currentSelection == null)
        {
            _currentSelection = cardController;
        }
        else
        {
            if (_currentSelection.CardIndex == cardController.CardIndex)
            {
                _currentSelection.MatchFound();
                cardController.MatchFound();
                _score += (int)Mathf.Pow(2, _combo);
                _combo++;
                _currentPairCount++;
                
                DDOL.Instance.MatchingOccured(true);
            }
            else
            {
                _currentSelection.NoMatchFound();
                cardController.NoMatchFound();
                _numberOfTriesLeft--;
                _combo = 0;
                
                DDOL.Instance.MatchingOccured(false);
            }
            _currentSelection = null;
            
            DDOL.Instance.GameStateUpdate();
        }
    }

    private void SetupLevel(int level)
    {
        Debug.Log("Setting up level " + level);

        _loadedLevel = level;
        
        LevelData levelData = _gameDataSO.GetLevelData(level);
        int cardCount = levelData.rowCount * levelData.columnCount;
        _levelSprites = new List<Sprite>();
        _levelIndexes = new List<int>();
        
        _levelPairCount = cardCount / 2;

        for (int i = 0; i < _levelPairCount; i++)
        {
            _levelIndexes.Add(i);
            _levelIndexes.Add(i);
        }
        
        _levelIndexes.Shuffle();
        for (int i = 0; i < _levelIndexes.Count; i++)
        {
            _levelSprites.Add(_spritesSO.GetSpriteAt(_levelIndexes[i]));
        }
        
        _score = 0;
        _combo = 0;
        _numberOfTriesLeft = Mathf.RoundToInt(levelData.rowCount * levelData.columnCount/2) + 1;
        _currentPairCount = 0;
    }
}
