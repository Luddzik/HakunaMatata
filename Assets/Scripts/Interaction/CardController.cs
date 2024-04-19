using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    [SerializeField] private GameObject _frontImage;
    [SerializeField] private GameObject _backImage;

    public bool IsMatched => _isMatched;
    public int CardIndex => _cardSpriteIndex;

    private readonly float _flipSpeed = 5f;
    
    private int _cardSpriteIndex;
    private bool _isFlipped = true;
    private bool _isMatched = false;
    
    private IEnumerator _flipCoroutine;
    private IEnumerator _resetCoroutine;

    public void InitializeCard(Sprite frontFace, int cardSpriteIndex)
    {
        _cardSpriteIndex = cardSpriteIndex;
        _frontImage.GetComponent<Image>().sprite = frontFace;
    }
    
    public void FlipCard(bool forceFace = false, bool reset = false)
    {
        if (_isMatched) return;
        
        if (_resetCoroutine != null)
        {
            StopCoroutine(_resetCoroutine);
            _resetCoroutine = null;
        }
        
        if (_flipCoroutine != null)
        {
            StopCoroutine(_flipCoroutine);
            _flipCoroutine = null;
        }

        _flipCoroutine = FlipCoroutine(forceFace, reset);
        StartCoroutine(_flipCoroutine);
    }

    public void MatchFound()
    {
        _isMatched = true;
    }

    public void NoMatchFound()
    {
        if (_resetCoroutine != null)
        {
            StopCoroutine(_resetCoroutine);
            _resetCoroutine = null;
        }

        _resetCoroutine = ResetCard();
        StartCoroutine(_resetCoroutine);
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
    
    private IEnumerator ResetCard()
    {
        yield return new WaitForSeconds(1f);
        FlipCard(reset: true);
        _resetCoroutine = null;
    }

    private IEnumerator FlipCoroutine(bool forceFace = false, bool reset = false)
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

        _isFlipped = (!forceFace && !_isFlipped) || reset;
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
