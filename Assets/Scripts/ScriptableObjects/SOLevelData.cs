using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "ScriptableObjects/Level Data")]
public class SOLevelData : ScriptableObject
{
    [SerializeField] private LevelData[] _gameData;

    public int GetLevelCount()
    {
        return _gameData.Length;
    }
    
    public LevelData GetLevelData(int level)
    {
        return _gameData[level];
    }
}

[Serializable]
public struct LevelData
{
    public int level;
    public int rowCount;
    public int columnCount;
}
