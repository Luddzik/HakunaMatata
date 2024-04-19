using UnityEngine;

[CreateAssetMenu(fileName = "Pair Sprites", menuName = "ScriptableObjects/Sprites")]
public class SOSprites : ScriptableObject
{
    [SerializeField] private Sprite _backSprite;
    [SerializeField] private Sprite[] _sprites;
    
    public Sprite BackSprite => _backSprite;
    
    public Sprite GetSpriteAt(int index)
    {
        return _sprites[index];
    }
}
