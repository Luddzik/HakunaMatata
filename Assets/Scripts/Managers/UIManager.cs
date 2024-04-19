using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public enum UIState
    {
        MainMenu,
        Game,
        GameOver,
        LevelComplete
    }
    
    [Header("UI Group Elements")]
    [SerializeField] private GameObject _mainMenuUI;
    [SerializeField] private GameObject _gameUI;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameObject _levelCompleteUI;
    
    [Header("In-Game UI Elements")]
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _noOfTriesText;

    [Header("Prefabs")] 
    [SerializeField] private GameObject _playLevelPrefab;
    [Space]
    [SerializeField] private GameObject _horizontalLayoutPrefab;
    [SerializeField] private GameObject _cardPrefab;

    [Header("Scene References")] 
    [SerializeField] private RectTransform _levelHolderTransform;
    [SerializeField] private RectTransform _verticalLayoutTransform;
    
    [Header("Object Pools")]
    [SerializeField] private int _initialHorizontalPoolSize = 6;
    [SerializeField] private int _initialCardPoolSize = 36;

    private UIState _currentState;
    
    private List<GameObject> _levelsReference;
    
    private List<GameObject> _horizontalLayoutsReference;
    private List<GameObject> _cardsReference;
    
    private List<GameObject> _horizontalLayoutPool;
    private List<GameObject> _cardPool;

    public void Initialize()
    {
        Debug.Log("Initializing UI Manager");
        
        _horizontalLayoutsReference = new List<GameObject>();
        _cardsReference = new List<GameObject>();
        
        InitializeLevelMenu();
        InitializeObjectPools();
        SetUIState(UIState.MainMenu);
    }
    
    public void StartGame(int level)
    {
        DDOL.Instance.StartLevel(level);
        SetUIState(UIState.Game);
    }
    
    public void BackButton()
    {
        SetUIState(UIState.MainMenu);
    }

    public void GameOver()
    {
        SetUIState(UIState.GameOver);
    }
    
    public void LevelComplete()
    {
        SetUIState(UIState.LevelComplete);
    }
    
    public void RestartLevel()
    {
        DDOL.Instance.RestartLevel();
        SetUIState(UIState.Game);
    }

    public void NextLevel()
    {
        DDOL.Instance.LoadNextLevel();
        SetUIState(UIState.Game);
    }

    public void SetupPlayGrid(int rowCount, int columnCount, List<Sprite> sprites, List<int> indexes)
    {
        ClearLevel();
        
        int index = 0;
        for (int i = 0; i < rowCount; i++)
        {
            GameObject horizontalLayout = GetPooledObject(_horizontalLayoutPool, _horizontalLayoutPrefab, _verticalLayoutTransform);
            for (int j = 0; j < columnCount; j++)
            {
                GameObject card = GetPooledObject(_cardPool, _cardPrefab, horizontalLayout.transform);
                CardController cardController = card.GetComponent<CardController>();
                Button cardButton = card.GetComponent<Button>();
                
                cardController.InitializeCard(sprites[index], indexes[index]);
                cardButton.onClick.AddListener(() =>
                {
                    cardController.FlipCard(forceFace: true);
                    CardSelected(cardController);
                });
                
                _cardsReference.Add(card);
                index++;
            }
            
            _horizontalLayoutsReference.Add(horizontalLayout);
        }
    }

    public void UpdateGameStateUI(int score, int noOfTries)
    {
        _scoreText.text = "Score: " + score;
        _noOfTriesText.text = "Tries Left: " + noOfTries;
    }

    private void InitializeLevelMenu()
    {
        _levelsReference = new List<GameObject>();
        int maxLevel = DDOL.Instance.GetLevelCount();
        int highestUnlocked = DDOL.Instance.HighestUnlockedLevel;
        
        for (int i = 0; i < maxLevel; i++)
        {
            GameObject level = Instantiate(_playLevelPrefab, _levelHolderTransform);
            Button levelButton = level.GetComponent<Button>();
            levelButton.interactable = i <= (highestUnlocked + 1);
            int levelIndex = i;
            levelButton.onClick.AddListener(() =>
            {
                StartGame(levelIndex);
            });
            
            level.GetComponentInChildren<TMP_Text>().text = "Level " + (i + 1);
            _levelsReference.Add(level);
        }
    }

    private void UpdateLevelMenu()
    {
        int maxLevel = DDOL.Instance.GetLevelCount();
        int highestUnlocked = DDOL.Instance.HighestUnlockedLevel;
        
        for (int i = 0; i < maxLevel; i++)
        {
            Button levelButton = _levelsReference[i].GetComponent<Button>();
            levelButton.interactable = i <= (highestUnlocked + 1);
        }
    }
    
    private void InitializeObjectPools()
    {
        _horizontalLayoutPool = new List<GameObject>();
        _cardPool = new List<GameObject>();

        for (int i = 0; i < _initialHorizontalPoolSize; i++)
        {
            GameObject horizontalLayout = Instantiate(_horizontalLayoutPrefab);
            horizontalLayout.SetActive(false);
            _horizontalLayoutPool.Add(horizontalLayout);
        }
        
        for (int i = 0; i < _initialCardPoolSize; i++)
        {
            GameObject card = Instantiate(_cardPrefab);
            card.SetActive(false);
            _cardPool.Add(card);
        }
    }
    
    private GameObject GetPooledObject(List<GameObject> pool, GameObject prefab, Transform parent)
    {
        GameObject obj;
        if (pool.Count > 0)
        {
            obj = pool[0];
            pool.RemoveAt(0);
        }
        else
        {
            obj = Instantiate(prefab);
        }

        obj.transform.SetParent(parent);
        obj.SetActive(true);
        return obj;
    }

    private void ReturnToPool(List<GameObject> pool, GameObject obj)
    {
        obj.SetActive(false);
        pool.Add(obj);
    }

    private void ClearLevel()
    {
        foreach (GameObject card in _cardsReference)
        {
            Button cardButton = card.GetComponent<Button>();
            cardButton.onClick.RemoveAllListeners();
            CardController cardController = card.GetComponent<CardController>();
            cardController.ClearCard();
            ReturnToPool(_cardPool, card);
        }
        foreach (GameObject horizontal in _horizontalLayoutsReference)
        {
            ReturnToPool(_horizontalLayoutPool, horizontal);
        }
        
        _cardsReference.Clear();
        _horizontalLayoutsReference.Clear();
    }

    private void CardSelected(CardController card)
    {
        if (card.IsMatched) return;
        
        DDOL.Instance.CardSelected(card);
    }

    private void SetUIState(UIState state)
    {
        _currentState = state;
        
        _mainMenuUI.SetActive(_currentState == UIState.MainMenu);
        _gameUI.SetActive(_currentState == UIState.Game);
        _gameOverUI.SetActive(_currentState == UIState.GameOver);
        _levelCompleteUI.SetActive(_currentState == UIState.LevelComplete);

        if (state == UIState.MainMenu)
        {
            UpdateLevelMenu();
        }
    }
}
