using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemAutoDestroy : MonoBehaviour
{
    private ParticleSystem _ps;
    private GameManager _gmScript;
    public void Start()
    {
        _ps = GetComponent<ParticleSystem>();
        _gmScript = GameManager.Instance;

        AudioSource aud = GetComponent<AudioSource>();

        if (!AudioManager.Instance.IsSFXEnabled) return;

        aud?.Play();
    }

    public void Update()
    {
        if (_ps && !_ps.IsAlive() || IsLevelOver())
        {
            Destroy(gameObject);
        }
    }

    bool IsLevelOver()
    {
        if (_gmScript.State == GameState.Playing || _gmScript.State == GameState.Paused) return false;
        else return true;
    }
}
