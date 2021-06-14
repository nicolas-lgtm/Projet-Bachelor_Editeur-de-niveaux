using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Sound[] sounds;
    public static Sound[] _sounds;

    void Start()
    {
        _sounds = sounds;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    public static void Play(string name)
    {
        Sound s = Array.Find(_sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning(name + " not found");
            return;
        }

        s.source.Play();
    }


    [Serializable]
    public class Sound
    {
        public string name;

        public AudioClip clip;

        [Range(0f, 1f)] public float volume;
        public bool loop;

        [HideInInspector] public AudioSource source;
    }

    enum SoundName
    {
        MusicBackground,
        movingSound,
        winSound,
        loseSound
    }
}
