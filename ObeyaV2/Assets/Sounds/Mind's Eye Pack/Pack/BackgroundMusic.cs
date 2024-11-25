using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour
{
    [System.Serializable]
    public class NightMusic
    {
        public AudioClip[] tracks;
    }

    public NightMusic[] nightMusics;
    private AudioSource audioSource;
    private int currentTrackIndex = 0;
    private NightManager nightManager;
    
    private float fadeOutDuration = 2f;
    private float fadeInDuration = 2f;
    private bool isFading = false;
    private float defaultVolume = 1f;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = defaultVolume;
        nightManager = FindObjectOfType<NightManager>();
        
        if (nightManager == null)
        {
            Debug.LogError("NightManager not found in the scene!");
            return;
        }

        PlayNextTrack();
    }

    void Update()
    {
        if (!audioSource.isPlaying && !isFading)
        {
            PlayNextTrack();
        }
    }

    private void UpdateBackgroundMusic()
    {
        if (!isFading)
        {
            StartCoroutine(FadeOutAndPlayNext());
        }
    }


    private IEnumerator FadeOutAndPlayNext()
    {
        isFading = true;
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeOutDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = 0;

        PlayNextTrack();

        while (audioSource.volume < defaultVolume)
        {
            audioSource.volume += defaultVolume * Time.deltaTime / fadeInDuration;
            yield return null;
        }

        audioSource.volume = defaultVolume;
        isFading = false;
    }


    private void PlayNextTrack()
    {
        int currentNight = nightManager.currentNight;
        if (currentNight <= nightMusics.Length && nightMusics[currentNight - 1].tracks.Length > 0)
        {
            AudioClip[] currentNightTracks = nightMusics[currentNight - 1].tracks;
            audioSource.clip = currentNightTracks[currentTrackIndex];
            audioSource.Play();
            currentTrackIndex = (currentTrackIndex + 1) % currentNightTracks.Length;
        }
        else
        {
            Debug.LogWarning("No music defined for night " + currentNight);
        }
    }
    
    public void OnNightChanged()
    {
        UpdateBackgroundMusic();
    }
}