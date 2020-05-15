using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GetDamageEF : MonoBehaviour
{
    public static GetDamageEF Instance => instance;
    private static GetDamageEF instance;
    [SerializeField] private CanvasGroup canvasGroup;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OnGetDamage()
    {
        canvasGroup.DOFade(1, 0f)
            .OnComplete(() => canvasGroup.DOFade(0f, 0.2f));
    }

}
