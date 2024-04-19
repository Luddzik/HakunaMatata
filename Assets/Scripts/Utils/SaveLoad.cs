using UnityEngine;

public static class SaveLoad
{
    private const string UNLOCKED_LEVEL = "HighestUnlockedLevel";
    
    public static void SaveHighestUnlockedLevel(int level)
    {
        PlayerPrefs.SetInt(UNLOCKED_LEVEL, level);
    }
    
    public static void GetHighestUnlockedLevel(out int level)
    {
        level = PlayerPrefs.GetInt(UNLOCKED_LEVEL, -1);
    }
}
