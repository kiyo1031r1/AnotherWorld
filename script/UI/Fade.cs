using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    private static Fade instance;
    public static Fade Instance => instance;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void FadeIn()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(0, fadeDuration)
            .OnComplete(()=>canvasGroup.blocksRaycasts = false);
    }

    public void FadeOutFadeIn(string LoadScene)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1, fadeDuration)
            .OnComplete(() =>
            {
                SceneManager.LoadScene(LoadScene);
                ChangeBGM(LoadScene);
                FadeIn();
            });
    }

    private void ChangeBGM(string sceneName)
    {
        switch (sceneName)
        {
            case "TownScene":
                SoundManager.Instance.PlayBGM(0);
                break;
            case "Plain":
                SoundManager.Instance.PlayBGM(1);
                break;
            case "BossField":
                SoundManager.Instance.PlayBGM(2);
                break;
            case "GuildResultScene":
                SoundManager.Instance.PlayBGM(3);
                break;
        }
    }
}
