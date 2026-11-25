using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public void Awake()
    {
        Instance = this;
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        StartCoroutine(PlaySoundCoroutine(clip, volume));
    }

    IEnumerator PlaySoundCoroutine(AudioClip clip, float volume)
    {
        AudioSource source = transform.AddComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;
        source.Play();

        yield return new WaitForSeconds(clip.length * 2);

        Destroy(source);
    }
}
