using UnityEngine;

public static class AudioHelper
{
    public static AudioSource PlayClip2D(AudioClip clip, float volume)
    {
        GameObject audio = new GameObject("Audio2D");
        AudioSource audioSource = audio.AddComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.volume = volume;

        audioSource.Play();
        Object.Destroy(audio, clip.length);

        return audioSource;
    }
}
