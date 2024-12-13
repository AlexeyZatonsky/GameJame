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

    public List<Sound> sounds = new List<Sound>(); // Список всех звуков
    private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>(); // Быстрый доступ по имени

    public List<AudioSource> audioSources = new List<AudioSource>(); // Список источников звука

    protected override void Awake()
    {
        base.Awake(); // Вызов базового метода для работы SingletonManager

        // Заполняем словарь звуков
        foreach (var sound in sounds)
        {
            if (!soundDictionary.ContainsKey(sound.name))
            {
                soundDictionary[sound.name] = sound;
            }
        }

        // Если список источников пуст, добавляем один AudioSource по умолчанию
        if (audioSources.Count == 0)
        {
            AudioSource defaultSource = gameObject.AddComponent<AudioSource>();
            audioSources.Add(defaultSource);
        }
    }

    // Метод воспроизведения звука с выбором источника
    public void PlaySound(string name, int sourceIndex = 0)
    {
        if (sourceIndex < 0 || sourceIndex >= audioSources.Count)
        {
            Debug.LogWarning($"Неверный индекс источника звука: {sourceIndex}");
            return;
        }

        if (soundDictionary.TryGetValue(name, out Sound sound))
        {
            audioSources[sourceIndex].PlayOneShot(sound.clip, sound.volume);
        }
        else
        {
            Debug.LogWarning($"Звук с именем {name} не найден!");
        }
    }

    // Метод воспроизведения фоновой музыки с выбором источника
    public void PlayMusic(string name, int sourceIndex = 0, bool loop = true)
    {
        if (sourceIndex < 0 || sourceIndex >= audioSources.Count)
        {
            Debug.LogWarning($"Неверный индекс источника звука: {sourceIndex}");
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
            Debug.LogWarning($"Музыка с именем {name} не найдена!");
        }
    }

    // Метод остановки фоновой музыки для указанного источника
    public void StopMusic(int sourceIndex = 0)
    {
        if (sourceIndex < 0 || sourceIndex >= audioSources.Count)
        {
            Debug.LogWarning($"Неверный индекс источника звука: {sourceIndex}");
            return;
        }

        AudioSource source = audioSources[sourceIndex];
        if (source.isPlaying)
        {
            source.Stop();
        }
    }
}
