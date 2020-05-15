using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dialog_Guild: MonoBehaviour
{
    //全体画面
    [SerializeField] private CanvasGroup guildCanvas;

    //money
    [SerializeField] private Text guildRank;

    //sideBar
    [SerializeField] private CanvasGroup sideBarCanvas;
    [SerializeField] private GameObject sideBarDefaultSelect;
    [SerializeField] private Button button_requestList;
    [SerializeField] private Button button_End;

    //MainMenu
    [SerializeField] private CanvasGroup mainMenuCanvas;
    [SerializeField] private CanvasGroup activeMainMenuCanvas;
    [SerializeField] private GameObject mainMenuDefaultSelect;

    [SerializeField] private Button button_GuildRequest_Prefab;
    private const int MAX_ITEM_VIEW_NUMBER = 5;
    private GameObject selectItemCache;
    private Image selectItemImage;
    [SerializeField] private Color32 defaltButtonBgColor;
    [SerializeField] private Color32 selectButtonBgColor;
    [SerializeField] private Text messageText;

    [SerializeField] private GameObject requestsDirectory;
    [SerializeField] private GuildRequest_DataBase guildRequestData;
    private Button_GuildRequest[] button_GuildRequests;
    private int requestsLength;
    private Transform requestsContent;
    private Vector3 requestsContentCache;

    //Attention
    [SerializeField] private CanvasGroup attentionCanvas;
    [SerializeField] private GameObject attentionDefaultSelect;
    [SerializeField] private Button button_Yes;
    [SerializeField] private Button button_No;

    [SerializeField] private PlayerStatus status;
    private bool isChangeButton;
    private bool isCursorMovable = true;

    void Awake()
    {
        CreateGuildRequest();
        requestsContent = requestsDirectory.transform;
        requestsContentCache = new Vector3(requestsDirectory.transform.localPosition.x, requestsDirectory.transform.localPosition.y);
    }

    void Start()
    {
        SetGuildRequest();
        SetGuildRank();
        SetButtonEvent();
        ResetDialog();
    }

    void Update()
    {
        //×ボタン
        if (Input.GetButtonDown("Fire2"))
        {
            PushCancelButton();
        }

        //十字キー縦操作
        if (Input.GetAxis("VerticalXkey") == 1 && isCursorMovable)
        {
            SoundManager.Instance.PlaySE(11);
            isCursorMovable = false;
            isChangeButton = true;
        }

        if (Input.GetAxis("VerticalXkey") == -1 && isCursorMovable)
        {
            SoundManager.Instance.PlaySE(11);
            isCursorMovable = false;
            isChangeButton = true;
        }

        if (Input.GetAxis("VerticalXkey") == 0 && !isCursorMovable)
        {
            isCursorMovable = true;
        }

        if (mainMenuCanvas.interactable && isChangeButton)
        {
            SelectOfItems();
        }
    }

    private void ResetDialog()
    {
        isChangeButton = false;
        EventSystem.current.SetSelectedGameObject(null);

        guildCanvas.interactable = false;
        guildCanvas.alpha = 0;
        sideBarCanvas.interactable = false;
        mainMenuCanvas.interactable = false;
        attentionCanvas.interactable = false;
        attentionCanvas.alpha = 0;
    }

    public void OpenDialog()
    {
        guildCanvas.interactable = true;
        guildCanvas.alpha = 1;
        ActiveSideBar();
        status.ChangeStatusToFind();
    }

    public void CloseDialog()
    {
        ResetDialog();
        status.ChangeStatusToMove();
    }

    private void ActiveSideBar()
    {
        sideBarCanvas.interactable = true;
        mainMenuCanvas.interactable = false;
        activeMainMenuCanvas.alpha = 1;
        requestsContent.localPosition = requestsContentCache;
        attentionCanvas.interactable = false;
        attentionCanvas.alpha = 0;
        messageText.text = "冒険者ギルドへようこそ！";
        EventSystem.current.SetSelectedGameObject(sideBarDefaultSelect);
    }

    private void ActiveMainMenu()
    {
        sideBarCanvas.interactable = false;
        mainMenuCanvas.interactable = true;
        activeMainMenuCanvas.alpha = 0;
        attentionCanvas.interactable = false;
        attentionCanvas.alpha = 0;
        EventSystem.current.SetSelectedGameObject(mainMenuDefaultSelect);
    }

    private void ActiveAttention()
    {
        sideBarCanvas.interactable = false;
        mainMenuCanvas.interactable = false;
        attentionCanvas.interactable = true;
        attentionCanvas.alpha = 1;
        EventSystem.current.SetSelectedGameObject(attentionDefaultSelect);
    }

    private void ActiveMessage()
    {
        sideBarCanvas.interactable = false;
        mainMenuCanvas.interactable = false;
        attentionCanvas.interactable = false;
        attentionCanvas.alpha = 0;
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void PushRequestListButton()
    {
        SoundManager.Instance.PlaySE(10);
        isChangeButton = true;
        ActiveMainMenu();
    }

    private void PushEndButton()
    {
        SoundManager.Instance.PlaySE(10);
        StartCoroutine(PushEndButtnCoroutine());
    }

    private IEnumerator PushEndButtnCoroutine()
    {
        ActiveMessage();
        messageText.text = "またよろしくお願いします！";
        yield return new WaitForSeconds(0.5f);
        CloseDialog();
    }

    private void SelectOfItems()
    {
        isChangeButton = false;

        var index = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();
        messageText.text = button_GuildRequests[index].GuildRequest.Information;

        //スクロール処理
        if (index > MAX_ITEM_VIEW_NUMBER - 1 && Input.GetAxisRaw("VerticalXkey") == -1)
        {
            requestsContent.localPosition += new Vector3(0, 45);
        }
        
        if (index < requestsLength - MAX_ITEM_VIEW_NUMBER && Input.GetAxisRaw("VerticalXkey") == 1)
        {
            requestsContent.localPosition += new Vector3(0, -45);
        }
    }

    private void PushRequestButton()
    {
        SoundManager.Instance.PlaySE(10);
        isChangeButton = true;
        selectItemCache = EventSystem.current.currentSelectedGameObject;
        selectItemImage = selectItemCache.GetComponent<Image>();
        selectItemImage.color = selectButtonBgColor;
        ActiveAttention();
    }

    private void PushYesButton()
    {
        SoundManager.Instance.PlaySE(10);
        isChangeButton = true;
        StartCoroutine(PushYesButtonCoroutine());
    }

    private IEnumerator PushYesButtonCoroutine()
    {
        var selectRequest = selectItemCache.GetComponent<Button_GuildRequest>();

        //ランク不足
        if(selectRequest.GuildRequest.Rank > PlayData_Status.Instance.Rank)
        {
            messageText.text = "ランクが足りません";
            yield return new WaitForSeconds(0.5f);
            PushNoButton();
            yield break;
        }

        GuildRequestManager.Instance.SetRequestID(selectRequest.GuildRequest);
        ActiveMessage();
        messageText.text = "では、よろしくお願いします！";
        yield return new WaitForSeconds(0.5f);

        Fade.Instance.FadeOutFadeIn(selectRequest.GuildRequest.Location);
    }

    private void PushNoButton()
    {
        SoundManager.Instance.PlaySE(10);
        isChangeButton = true;
        ActiveMainMenu();
        selectItemImage.color = defaltButtonBgColor;
        EventSystem.current.SetSelectedGameObject(selectItemCache);
    }

    private void PushCancelButton()
    {
        SoundManager.Instance.PlaySE(10);
        isChangeButton = true;

        if (sideBarCanvas.interactable)
        {
            PushEndButton();
        }

        if (mainMenuCanvas.interactable)
        {
            ActiveSideBar();
        }

        if (attentionCanvas.interactable)
        {
            PushNoButton();
        }
    }

    private void CreateGuildRequest()
    {
        //ギルド依頼データを元に枠を作成
        requestsLength = guildRequestData.GuildRequestList.Length;
        
        //最初のみ別途付与
        mainMenuDefaultSelect.GetComponent<Button>().onClick.AddListener(PushRequestButton);

        for (int i = 1; i < requestsLength; i++)
        {
            var item = Instantiate(button_GuildRequest_Prefab, requestsDirectory.transform);
            item.onClick.AddListener(PushRequestButton);
        }

        button_GuildRequests = requestsDirectory.GetComponentsInChildren<Button_GuildRequest>();
    }

    private void SetButtonEvent()
    {
        button_requestList.onClick.AddListener(PushRequestListButton);
        button_End.onClick.AddListener(PushEndButton);
        button_Yes.onClick.AddListener(PushYesButton);
        button_No.onClick.AddListener(PushNoButton);
    }

    private void SetGuildRequest()
    {
        for (int i = 0; i < requestsLength; i++)
        {
            button_GuildRequests[i].SetRequest(guildRequestData.GuildRequestList[i]);
        }
    }

    private void SetGuildRank()
    {
        guildRank.text = PlayData_Status.Instance.RankText;
    }

}
