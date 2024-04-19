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

    private void SetUIState(UIState state)
    {
        _currentState = state;
        
        _mainMenuUI.SetActive(_currentState == UIState.MainMenu);
        _gameUI.SetActive(_currentState == UIState.Game);
        _gameOverUI.SetActive(_currentState == UIState.GameOver);
    }
}
