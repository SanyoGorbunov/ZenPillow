using UnityEngine;
using System;
using Boo.Lang;

public class AudioManager : MonoBehaviour
{
    private bool isMute = false;
    public AudioSound[] sounds;
    // Start is called before the first frame update

    private string[] _backgroundSounds;
    private string _currentBackgroundSound = "Ambient";

    public static AudioManager instance = null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        var backgroundSounds = new List<string>();
        foreach (AudioSound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            if (s.isBack)
            {
                backgroundSounds.Add(s.name);
            }
        }

        _backgroundSounds = backgroundSounds.ToArray();
    }

    public string[] GetBackgroundSounds()
    {
        return _backgroundSounds;
    }

    private void Start()
    {
        Play(_currentBackgroundSound);
    }

    public void Change(string name)
    {
        AudioSound sound = Array.Find(sounds, s => s.name == _currentBackgroundSound);
        sound.source.Stop();

        _currentBackgroundSound = name;
        Play(_currentBackgroundSound);
    }

    public void Play(string name)
    {
        AudioSound sound = Array.Find(sounds, s => s.name == name);
        sound.source.Play();
    }

    public void ToggleMute()
    {
        isMute = !isMute;
        SetMute(isMute);
    }

    public static void StaticToggleMute()
    {
        instance.ToggleMute();
    }

    public void SetMute(bool isMute)
    {
        if (isMute)
        {
            Mute();
        }
        else {
            UnMute();
        }
    }

    public bool GetMute()
    {
        return isMute;
    }


    public void Mute()
    {
        foreach (AudioSound s in sounds)
        {
            s.source.volume = 0.0f;
        }
    }

    public void UnMute()
    {
        foreach (AudioSound s in sounds)
        {
            s.source.volume = s.volume;
        }
    }
    public static void StaticPlay(string name)
    {
        instance.Play(name);
    }

    public void Pause(string name)
    {
        AudioSound sound = Array.Find(sounds, s => s.name == name);
        sound.source.Pause();
    }

    public static void StaticPause(string name)
    {
        instance.Pause(name);
    }

    public void UnPause(string name)
    {
        AudioSound sound = Array.Find(sounds, s => s.name == name);
        sound.source.UnPause();
    }

    public static void StaticUnpause(string name)
    {
        instance.UnPause(name);
    }
}
