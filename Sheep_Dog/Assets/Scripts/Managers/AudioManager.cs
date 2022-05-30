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

    void Start()
    {
        _musicSource?.Play();
    }

    public void PlayDogRun(bool isRunning)
    {
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


    async Task PlayAsyncPitch(AudioSource source)
    {
        float pitch = UnityEngine.Random.Range(-10, 10) / 100; // ALTER PITCH OF AUDIO SOURCE

        source.Play();
        await Task.Delay(200);

        _scoreSource.pitch -= pitch;
    }
    
}
