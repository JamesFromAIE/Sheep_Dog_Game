using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // VARIABLE FOR SINGLETON

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // SET SINGLETON TO THIS SCRIPT
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] AudioSource _runningDogSource; // AUDIO SOURCE FOR RUNNING DOGS
    [SerializeField] AudioSource _scoreSource;
    [SerializeField] AudioSource _musicSource;
    [SerializeField] AudioSource[] _sheepAudSources; // ARRAY OF BLEETING AUDIO SOURCES
    [SerializeField] List<AudioClip> _sheepClips;  // ARRAY OF SHEEP BLEET SOUNDS

    [SerializeField] bool _isMusicEnabled = true;
    public bool IsMusicEnabled { get { return _isMusicEnabled; } }

    [SerializeField] bool _isSFXEnabled = true;
    public bool IsSFXEnabled { get { return _isSFXEnabled; } }

    void Start()
    {
        _musicSource?.Play();
    }

    public void SetMusicBool(bool condition)
    {
        _isMusicEnabled = condition;
        UpdateMusicVolume();
    }

    public void SetSFXBool(bool condition)
    {
        _isSFXEnabled = condition;
        //UpdateSFXVolume();
    }


    public void UpdateMusicVolume()
    {
        if (_isMusicEnabled)
        {
            _musicSource.volume = 1;
        }
        else
        {
            _musicSource.volume = 0;
        }
    }

    public void UpdateSFXVolume()
    {
        if (_isSFXEnabled)
        {
            _runningDogSource.volume = 1;
            _scoreSource.volume = 1;
            for (int i = 0; i < _sheepAudSources.Length; i++) _sheepAudSources[i].volume = 1;
        }
        else
        {
            _runningDogSource.volume = 0;
            _scoreSource.volume = 0;
            for (int i = 0; i < _sheepAudSources.Length; i++) _sheepAudSources[i].volume = 0;
        }
    }

    public void PlayDogRun(bool isRunning)
    {
        if (!_isSFXEnabled) return;

        if (isRunning && !_runningDogSource.isPlaying) // IF CONDITION WANTS TO PLAY AND NOT CURRENTLY PLAYING...
        {
            _runningDogSource.Play(); // STOP AUDIOSOURCE
        }
        else if (!isRunning && _runningDogSource.isPlaying) // IF CONDITION DOES NOT WANT TO PLAY AND CURRENTLY PLAYING...
        {
            _runningDogSource.Stop(); // STOP AUDIOSOURCE
        }
        
    }

    public void PlaySheepBleet()
    {
        if (!_isSFXEnabled) return;

        var rand = new System.Random(); // GET INSTANCE OF RANDOM()
        var clip = _sheepClips[rand.Next(_sheepClips.Count)]; // GET RANDOM BLEET CLIP
        float pitch = UnityEngine.Random.Range(80, 120) / 100; // ALTER PITCH OF AUDIO SOURCE

        for (int i = 0; i < _sheepAudSources.Length; i++) // FOR EVERY BLEETING AUDIO SOURCE
        {
            var source = _sheepAudSources[i];
            if (!source.isPlaying) // IF SOURCE IS NOT PLAYING...
            {
                source.PlayClip(clip, pitch); // PLAY BLEET CLIP ON THIS AUDIO SOURCE
            }
        }
    }

    
}
