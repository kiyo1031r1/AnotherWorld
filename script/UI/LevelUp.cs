using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelUp : MonoBehaviour
{
    [SerializeField] private GameObject text;
    private Transform _textTransform;
    private CanvasGroup _canvasGroup;
    private float duration = 2f;

    void Start()
    {
        _textTransform = text.GetComponent<Transform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        DisplayLevelUp();
    }

    public void DisplayLevelUp()
    {
        gameObject.SetActive(true);
        var transformCache = _textTransform;
        var defaultPosition = transformCache.localPosition;
        var nextPosition = defaultPosition + new Vector3(0, 50f);
        transformCache.DOLocalMove(nextPosition, duration);
        _canvasGroup.DOFade(0, duration)
            .OnComplete(() => Destroy(gameObject));
    }

}
