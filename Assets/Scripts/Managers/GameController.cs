using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Game Data")]
    [SerializeField] private SOLevelData _gameDataSO;
    [SerializeField] private SOSprites _spritesSO;

    public void Initialize()
    {
        Debug.Log("Initializing game controller");
    }
    
    public void StartGame(int level)
    {
        // Setup game 
        SetupLevel(level);
    }
    
    public LevelData GetLevelData(int level)
    {
        return _gameDataSO.GetLevelData(level);
    }

    private void SetupLevel(int level)
    {
        // Setup level
        Debug.Log("Setting up level " + level);
    }
}
