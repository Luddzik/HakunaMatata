using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource _clickAudio;
    [SerializeField] private AudioSource _errorAudio;
    [SerializeField] private AudioSource _correctAudio;
    [SerializeField] private AudioSource _gameoverAudio;
    
    public void Initialize()
    {
        Debug.Log("Initializing Audio Manager");
    }
    
    public void PlayClickAudio()
    {
        _clickAudio.Play();
    }
    
    public void PlayErrorAudio()
    {
        _errorAudio.Play();
    }
    
    public void PlayCorrectAudio()
    {
        _correctAudio.Play();
    }
    
    public void PlayGameoverAudio()
    {
        _gameoverAudio.Play();
    }
}
