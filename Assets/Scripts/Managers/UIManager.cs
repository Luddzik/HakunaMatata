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
        GameOver
    }
    
    [Header("UI Group Elements")]
    [SerializeField] private GameObject _mainMenuUI;
    [SerializeField] private GameObject _gameUI;
    [SerializeField] private GameObject _gameOverUI;
    
    [Header("In-Game UI Elements")]
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _noOfTriesText;

    [Header("Prefabs")] 
    [SerializeField] private GameObject _horizontalLayoutPrefab;
    [SerializeField] private GameObject _cardPrefab;

    [Header("Scene References")] 
    [SerializeField] private RectTransform _verticalLayoutTransform;
    
    private UIState _currentState;
    private List<GameObject> _cardsReference;

    public void Initialize()
    {
        Debug.Log("Initializing UI Manager");
        
        _cardsReference = new List<GameObject>();
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

    public void SetupPlayGrid(int rowCount, int columnCount, List<Sprite> sprites, List<int> indexes)
    {
        ClearLevel();
        
        int index = 0;
        for (int i = 0; i < rowCount; i++)
        {
            GameObject horizontalLayout = Instantiate(_horizontalLayoutPrefab, _verticalLayoutTransform);
            for (int j = 0; j < columnCount; j++)
            {
                GameObject card = Instantiate(_cardPrefab, horizontalLayout.transform);
                CardController cardController = card.GetComponent<CardController>();
                Button cardButton = card.GetComponent<Button>();
                
                cardController.InitializeCard(sprites[index], indexes[index]);
                cardButton.onClick.AddListener(() =>
                {
                    cardController.FlipCard(forceFace: true);
                    CardSelected(cardController);
                });
                index++;
            }
            _cardsReference.Add(horizontalLayout);
        }
    }

    public void UpdateGameStateUI(int score, int noOfTries)
    {
        _scoreText.text = "Score: " + score;
        _noOfTriesText.text = "Tries Left: " + noOfTries;
    }

    private void ClearLevel()
    {
        foreach (GameObject card in _cardsReference)
        {
            Destroy(card);
        }
        _cardsReference.Clear();
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
    }
}
