using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private void Awake() => Instance = this;

    [SerializeField] AudioSource[] _sheepAudSources;
    [SerializeField] AudioSource _runningDogSource;
    [SerializeField] List<AudioClip> _sheepClips;
    
    public void PlayDogRun(bool isRunning)
    {
        if (isRunning && !_runningDogSource.isPlaying)
        {
            _runningDogSource.Play();
        }
        else if (!isRunning && _runningDogSource.isPlaying)
        {
            _runningDogSource.Stop();
        }
        
    }

    public void PlaySheepBleet()
    {
        var rand = new System.Random();
        var clip = _sheepClips[rand.Next(_sheepClips.Count)];
        float pitch = UnityEngine.Random.Range(80, 120) / 100;

        for (int i = 0; i < _sheepAudSources.Length; i++)
        {
            var source = _sheepAudSources[i];
            if (!source.isPlaying)
            {
                source.PlayClip(clip, pitch);
            }
        }
    }
    
}
