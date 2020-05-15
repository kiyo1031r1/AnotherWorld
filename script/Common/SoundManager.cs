using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public static SoundManager Instance => _instance;

    void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private AudioSource audioSourceBGM;
    [SerializeField] private AudioSource audioSourceSE;
    [SerializeField] private AudioClip[] audioClipBGM;
    [SerializeField] private AudioClip[] audioClipSE;

    public void PlayBGM(int index)
    {
        audioSourceBGM.Stop();
        audioSourceBGM.clip = audioClipBGM[index];
        audioSourceBGM.Play();
    }

    public void PlaySE(int index)
    {
        audioSourceSE.PlayOneShot(audioClipSE[index]);
    }

    public void PlaySE(AudioClip audioClip)
    {
        audioSourceSE.PlayOneShot(audioClip);
    }

}
