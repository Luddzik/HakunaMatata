using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    [SerializeField] private GameObject _frontImage;
    [SerializeField] private GameObject _backImage;
    
    private readonly float _flipSpeed = 5f;
    
    private int _cardSpriteIndex;
    private bool _isFlipped = false;
    private IEnumerator _flipCoroutine;

    public void InitializeCard(Sprite frontFace, int cardSpriteIndex)
    {
        _cardSpriteIndex = cardSpriteIndex;
        _frontImage.GetComponent<Image>().sprite = frontFace;
    }
    
    public void FlipCard()
    {
        bool forceFace = false;
        if (_flipCoroutine != null)
        {
            forceFace = true;
            StopCoroutine(_flipCoroutine);
            _flipCoroutine = null;
        }

        _flipCoroutine = FlipCoroutine(forceFace);
        StartCoroutine(_flipCoroutine);
    }
    
    // Debug flip function
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FlipCard();
        }
    }
    // --------------------
    
    private IEnumerator FlipCoroutine(bool forceFace = false)
    {
        float scale = transform.localScale.x;
        while (scale > 0f)
        {
            scale -= (Time.deltaTime * _flipSpeed);
            if (scale < 0f)
            {
                scale = 0f;
            }
            transform.localScale = new Vector3(scale, 1, 1);
            
            yield return null;
        }

        _isFlipped = !forceFace && !_isFlipped;
        _frontImage.SetActive(!_isFlipped);
        _backImage.SetActive(_isFlipped);

        while (scale < 1f)
        {
            scale += (Time.deltaTime * _flipSpeed);
            if (scale > 1f)
            {
                scale = 1f;
            }
            transform.localScale = new Vector3(scale, 1, 1);
            
            yield return null;
        }

        _flipCoroutine = null;
    }
}
