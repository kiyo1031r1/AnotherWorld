using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Button button_Start;
    [SerializeField] private Button button_Reset;
    [SerializeField] private Button button_Yes;
    [SerializeField] private Button button_No;
    [SerializeField] private CanvasGroup mainCanvas;
    [SerializeField] private CanvasGroup attentionCanvas;
    [SerializeField] private CanvasGroup dataResetCanvas;
    [SerializeField] private DataLoadManager dataLoadManager;
    private bool isCursorMovable = true;
    private bool isCursorMovableH = true;
    private GameObject currentSelectObject;

    void Start()
    {
        SoundManager.Instance.PlayBGM(0);
        button_Start.onClick.AddListener(PushStartButton);
        button_Reset.onClick.AddListener(PushResetButton);
        button_Yes.onClick.AddListener(PushYesButton);
        button_No.onClick.AddListener(PushNoButton);
        EventSystem.current.SetSelectedGameObject(button_Start.gameObject);
        currentSelectObject = EventSystem.current.currentSelectedGameObject;

        attentionCanvas.alpha = 0;
        attentionCanvas.interactable = false;
        dataResetCanvas.alpha = 0;
    }

    void Update()
    {
        //マウスクリック無効化
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            EventSystem.current.SetSelectedGameObject(currentSelectObject);
        }

        //×ボタン
        if (Input.GetButtonDown("Fire2") && attentionCanvas.interactable)
        {
            PushNoButton();
        }

        //十字キー縦操作
        if (Input.GetAxis("VerticalXkey") == 1 && isCursorMovable)
        {
            SoundManager.Instance.PlaySE(11);
            isCursorMovable = false;
        }

        if (Input.GetAxis("VerticalXkey") == -1 && isCursorMovable)
        {
            SoundManager.Instance.PlaySE(11);
            isCursorMovable = false;
        }

        if (Input.GetAxis("VerticalXkey") == 0 && !isCursorMovable)
        {
            currentSelectObject = EventSystem.current.currentSelectedGameObject;
            isCursorMovable = true;
        }

        //十字キー横操作
        if (Input.GetAxis("HorizontalXkey") == 1 && isCursorMovableH)
        {
            SoundManager.Instance.PlaySE(11);
            isCursorMovableH = false;
        }

        if (Input.GetAxis("HorizontalXkey") == -1 && isCursorMovableH)
        {
            SoundManager.Instance.PlaySE(11);
            isCursorMovableH = false;
        }

        if (Input.GetAxis("HorizontalXkey") == 0 && !isCursorMovableH)
        {
            currentSelectObject = EventSystem.current.currentSelectedGameObject;
            isCursorMovableH = true;
        }
    }

    private void PushStartButton()
    {
        SoundManager.Instance.PlaySE(12);

        //初回時
        if(PlayData_Status.Instance.Level == 0)
        {
            PlayData_Status.Instance.LoadFirstStatus();
        }

        Fade.Instance.FadeOutFadeIn("TownScene");
    }

    private void PushResetButton()
    {
        SoundManager.Instance.PlaySE(10);
        attentionCanvas.alpha = 1;
        attentionCanvas.interactable = true;
        mainCanvas.interactable = false;
        EventSystem.current.SetSelectedGameObject(button_No.gameObject);
        currentSelectObject = EventSystem.current.currentSelectedGameObject;
    }

    private void PushYesButton()
    {
        SoundManager.Instance.PlaySE(10);
        attentionCanvas.alpha = 0;
        attentionCanvas.interactable = false;
        dataResetCanvas.alpha = 1;
        Invoke("DataReset", 1f);
    }

    private void PushNoButton()
    {
        SoundManager.Instance.PlaySE(10);
        attentionCanvas.alpha = 0;
        attentionCanvas.interactable = false;
        mainCanvas.interactable = true;
        EventSystem.current.SetSelectedGameObject(button_Reset.gameObject);
        currentSelectObject = EventSystem.current.currentSelectedGameObject;
    }

    private void DataReset()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        //各データの再読み込み、初期データの読み込み
        dataLoadManager.LoadStatusData();
        dataLoadManager.LoadItemData();
        PlayData_Status.Instance.LoadFirstStatus();
        PlayData_Status.Instance.Save();
        PlayData_OwnedItems.Instance.LoadFirstItem();
        PlayData_OwnedItems.Instance.Save();

        dataResetCanvas.alpha = 0;
        mainCanvas.interactable = true;
        EventSystem.current.SetSelectedGameObject(button_Start.gameObject);
        currentSelectObject = EventSystem.current.currentSelectedGameObject;
    }
}
