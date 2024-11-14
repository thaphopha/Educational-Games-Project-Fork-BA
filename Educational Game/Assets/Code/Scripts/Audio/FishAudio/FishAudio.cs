using UnityEngine;

public class FishAudio : MonoBehaviour
{
    [SerializeField] AudioClip eatClip;
    [SerializeField] AudioClip sprintClip;

    AudioSource eatAudioSource;
    AudioSource swimAudioSource;
    AudioLowPassFilter lowPassFilter;
    AudioReverbFilter reverbFilter;

    private void Awake()
    {
        eatAudioSource = gameObject.AddComponent<AudioSource>();
        eatAudioSource.clip = eatClip;

        swimAudioSource = gameObject.AddComponent<AudioSource>();
        swimAudioSource.clip = sprintClip;

        lowPassFilter = gameObject.AddComponent<AudioLowPassFilter>();
        lowPassFilter.cutoffFrequency = 500.0f;
        lowPassFilter.lowpassResonanceQ = 1.0f;

        reverbFilter = gameObject.AddComponent<AudioReverbFilter>();
        reverbFilter.reverbPreset = AudioReverbPreset.Underwater;

        eatAudioSource.spatialBlend = 1f;
        swimAudioSource.spatialBlend = 1f;

        eatAudioSource.volume = 0.25f;
    }

    public void PlayEatAudio()
    {
        eatAudioSource.Play();
    }

    public void PlaySprintAudio()
    {
        if (!swimAudioSource.isPlaying)
        {
            swimAudioSource.Play();
        }
    }

    public void stopSwimAudio()
    {
        swimAudioSource.Stop();
    }
}
