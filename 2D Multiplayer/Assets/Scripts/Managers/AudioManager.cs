using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : SingletonPersistent<AudioManager>
{
    public float fadeDuration = 1f;


    [Header("Music")]
    [SerializeField]
    private AudioSource m_musicSource;
    public  AudioClip introMusic, gameplayMusic, bossMusic;

    [SerializeField]
    [Range(0f, 1f)]
    private float m_maxMusicVolume;

    [Header("SFX")]
    [SerializeField]
    private AudioSource m_sfxSource;


    public bool fadingOut;

    private void Start()
    {
        m_musicSource.volume = m_maxMusicVolume;
        SwitchMusic(introMusic);
    }

    public void PlaySoundEffect(AudioClip clip, float volume = 1f)
    {
        m_sfxSource.PlayOneShot(clip, volume);
    }

    public void StopMusic()
    {
        m_musicSource.Stop();
    }

    public void SwitchMusic(AudioClip nextMusic)
    {
        if (fadingOut) return; // don't switch if music is already fading out

        fadingOut = true;

        StartCoroutine(FadeOutMusic(nextMusic));


    }

    IEnumerator FadeOutMusic(AudioClip nextMusic)
    {
        float startVolume = m_musicSource.volume;
        float timer = 0;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            m_musicSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return fadeDuration;
        }

        m_musicSource.Stop();
        m_musicSource.volume = startVolume;
        fadingOut = false;
        m_musicSource.clip = nextMusic;
        StartCoroutine(FadeInMusic());

    }

    IEnumerator FadeInMusic()
    {

        float startVolume = 0f;
        float timer = 0;

        m_musicSource.Play();

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            m_musicSource.volume = Mathf.Lerp(startVolume, m_maxMusicVolume, timer / fadeDuration);
            yield return fadeDuration;
        }

        m_musicSource.volume = m_maxMusicVolume;

    }
}