using UnityEngine;

public class UIManager : MonoBehaviour
{
    public enum UIState
    {
        MainMenu,
        Game,
        GameOver
    }
    
    [Header("UI Elements")]
    [SerializeField] private GameObject _mainMenuUI;
    [SerializeField] private GameObject _gameUI;
    [SerializeField] private GameObject _gameOverUI;

    [Header("Prefabs")] 
    [SerializeField] private GameObject _horizontalLayoutPrefab;
    [SerializeField] private GameObject _cardPrefab;

    [Header("Scene References")] 
    [SerializeField] private RectTransform _verticalLayoutTransform;
    
    private UIState _currentState;

    public void Initialize()
    {
        Debug.Log("Initializing UI Manager");
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

    public void SetupPlayGrid(int rowCount, int columnCount)
    {
        for (int i = 0; i < rowCount; i++)
        {
            GameObject horizontalLayout = Instantiate(_horizontalLayoutPrefab, _verticalLayoutTransform);
            for (int j = 0; j < columnCount; j++)
            {
                GameObject card = Instantiate(_cardPrefab, horizontalLayout.transform);
            }
        }
    }

    private void SetUIState(UIState state)
    {
        _currentState = state;
        
        _mainMenuUI.SetActive(_currentState == UIState.MainMenu);
        _gameUI.SetActive(_currentState == UIState.Game);
        _gameOverUI.SetActive(_currentState == UIState.GameOver);
    }
}
