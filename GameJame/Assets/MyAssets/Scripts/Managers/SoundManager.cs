using UnityEngine;
using System.Collections.Generic;

public class SoundManager : SingletonManager<SoundManager>
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

    public List<Sound> sounds = new List<Sound>(); // ������ ���� ������
    private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>(); // ������� ������ �� �����

    public List<AudioSource> audioSources = new List<AudioSource>(); // ������ ���������� �����

    protected override void Awake()
    {
        base.Awake(); // ����� �������� ������ ��� ������ SingletonManager

        // ��������� ������� ������
        foreach (var sound in sounds)
        {
            if (!soundDictionary.ContainsKey(sound.name))
            {
                soundDictionary[sound.name] = sound;
            }
        }

        // ���� ������ ���������� ����, ��������� ���� AudioSource �� ���������
        if (audioSources.Count == 0)
        {
            AudioSource defaultSource = gameObject.AddComponent<AudioSource>();
            audioSources.Add(defaultSource);
        }
    }

    // ����� ��������������� ����� � ������� ���������
    public void PlaySound(string name, int sourceIndex = 0)
    {
        if (sourceIndex < 0 || sourceIndex >= audioSources.Count)
        {
            Debug.LogWarning($"�������� ������ ��������� �����: {sourceIndex}");
            return;
        }

        if (soundDictionary.TryGetValue(name, out Sound sound))
        {
            audioSources[sourceIndex].PlayOneShot(sound.clip, sound.volume);
        }
        else
        {
            Debug.LogWarning($"���� � ������ {name} �� ������!");
        }
    }

    // ����� ��������������� ������� ������ � ������� ���������
    public void PlayMusic(string name, int sourceIndex = 0, bool loop = true)
    {
        if (sourceIndex < 0 || sourceIndex >= audioSources.Count)
        {
            Debug.LogWarning($"�������� ������ ��������� �����: {sourceIndex}");
            return;
        }

        if (soundDictionary.TryGetValue(name, out Sound sound))
        {
            AudioSource source = audioSources[sourceIndex];
            source.clip = sound.clip;
            source.volume = sound.volume;
            source.loop = loop;
            source.Play();
        }
        else
        {
            Debug.LogWarning($"������ � ������ {name} �� �������!");
        }
    }

    // ����� ��������� ������� ������ ��� ���������� ���������
    public void StopMusic(int sourceIndex = 0)
    {
        if (sourceIndex < 0 || sourceIndex >= audioSources.Count)
        {
            Debug.LogWarning($"�������� ������ ��������� �����: {sourceIndex}");
            return;
        }

        AudioSource source = audioSources[sourceIndex];
        if (source.isPlaying)
        {
            source.Stop();
        }
    }
}
