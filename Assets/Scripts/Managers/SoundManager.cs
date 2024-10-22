using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioMixer mixer;

    AudioSource[] audioSources;
    AudioMixerGroup[] mixerGroup;

    protected override void Init()
    {
        audioSources = new AudioSource[(int)Enums.ESoundType.MAX];
        mixerGroup = mixer.FindMatchingGroups("Master");

        GameObject temp;
        for (int i = 0; i < audioSources.Length; i++)
        {
            temp = new GameObject($"{(Enums.ESoundType)i}");
            audioSources[i] = temp.AddComponent<AudioSource>();
            audioSources[i].transform.SetParent(transform);
            audioSources[i].outputAudioMixerGroup = mixerGroup[i + 1];
        }

        // BGM
        audioSources[(int)Enums.ESoundType.BGM].loop = true;
        // SFX
        audioSources[(int)Enums.ESoundType.SFX].playOnAwake = false;
        audioSources[(int)Enums.ESoundType.SFX].loop = false;
    }

    public void Play(Enums.ESoundType playType, AudioClip clip)
    {
        AudioSource audioSource = audioSources[(int)playType];
        switch(playType)
        {
            case Enums.ESoundType.BGM:
                {
                    if (audioSource.isPlaying)
                        audioSource.Stop();

                    audioSource.clip = clip;
                    audioSource.Play();
                }
                break;
            case Enums.ESoundType.SFX:
                {
                    audioSource.PlayOneShot(clip);
                }
                break;
        }
    }

    public void PlayClipAtPoint(AudioClip clip, Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(clip, pos);
    }

    public void Stop(Enums.ESoundType playType)
    {
        audioSources[(int)playType].Stop();
    }
}
