using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // VARIABLE FOR SINGLETON

    private void Awake() => Instance = this; // SET SINGLETON TO THIS SCRIPT

    [SerializeField] AudioSource[] _sheepAudSources; // ARRAY OF BLEETING AUDIO SOURCES
    [SerializeField] AudioSource _runningDogSource; // AUDIO SOURCE FOR RUNNING DOGS
    [SerializeField] List<AudioClip> _sheepClips;  // ARRAY OF SHEEP BLEET SOUNDS

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
    
}
