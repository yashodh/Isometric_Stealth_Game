using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    private AudioSource _source;

    public void Play(AudioEnum type)
    {
        _source.clip = AudioManager.Instance.GetAudioData(type).Clip;
        _source.Play();
    }

    public void PlayOnce(AudioEnum type)
    {
        AudioClip clip = AudioManager.Instance.GetAudioData(type).Clip;
        _source.PlayOneShot(clip);
    }

    public void Stop()
    {
        _source.Stop();
    }

    public bool IsPlaying()
    {
        return _source.isPlaying;
    }
}
