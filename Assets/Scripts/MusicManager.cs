using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    [SerializeField] private AudioSource _backgroundMusicPlayer;
    [SerializeField] private AudioSource _SFXMusicPlayer;
    [SerializeField] private AudioClip _beethoven;
    [SerializeField] private AudioClip _click;
    [SerializeField] private AudioClip _heartFlourish;

    private void Awake()
    {
        Instance = this;
        _backgroundMusicPlayer.clip = _beethoven;
    }

    private void Start()
    {
        _backgroundMusicPlayer.Play(); // It should play on awake anyway, but it's not working
    }
    
    public void PlayClick()
    {
        _SFXMusicPlayer.PlayOneShot(_click);
    }

    public void PlayHeartFlourish()
    {
        _SFXMusicPlayer.PlayOneShot(_heartFlourish);
    }
}
