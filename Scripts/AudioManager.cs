using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private List<AudioData> _data = new List<AudioData>();

    public static AudioManager Instance;

    public void Init()
    {
        Instance = this;
    }

    public void PlayAudioAt(AudioEnum type, Vector3 pos)
    {
        AudioData data =_data.Find(x => x.AudioType == type);

        if (data != null)
            AudioSource.PlayClipAtPoint(data.Clip, pos);
    }

    public AudioData GetAudioData(AudioEnum type)
    {
        return _data.Find(x => x.AudioType == type);
    }
}

[Serializable]
public class AudioData
{
    public AudioEnum AudioType;
    public AudioClip Clip;
}

public enum AudioEnum
{
    // player
    Breath,
    Whistle,
    Hurt,

    // Enemy
    Gunshot,

    // Generic
    Explosion,
    Punch,
}
