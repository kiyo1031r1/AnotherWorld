using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class Dialog_GuildItem : MonoBehaviour
{
    //全体画面
    [SerializeField] private CanvasGroup GuildItemCanvas;

    //sideBar
    [SerializeField] private CanvasGroup sideBarCanvas;
    [SerializeField] private GameObject sideBarDefaultSelect;
    [SerializeField] private Button button_AddItem;
    [SerializeField] private Button button_End;

    //MainMenu
    [SerializeField] private CanvasGroup mainMenuCanvas;
    [SerializeField] private CanvasGroup activeMainMenuCanvas;
    [SerializeField] private GameObject mainMenuDefaultSelect;
    private GameObject selectItemCache;
    private Image selectItemImage;
    [SerializeField] private Color32 defaltButtonBgColor;
    [SerializeField] private Color32 selectButtonBgColor;
    [SerializeField] private Text messageText;

    //GuildItem
    [SerializeField] private Button button_FirstGuildItem;
    [SerializeField] private Button button_SecondGuildItem;
    private Button_GuildItem firstGuildItem;
    private Button_GuildItem secondGuildItem;

    //Attention
    [SerializeField] private CanvasGroup attentionCanvas;
    [SerializeField] private GameObject attentionDefaultSelect;
    [SerializeField] private Button button_Yes;
    [SerializeField] private Button button_No;

    private PlayerStatus status;
    private bool isChangeButton;
    private bool isCursorMovable = true;
    [SerializeField] private Item_DataBase guildItemData;

    void Start()
    {
        status = GameObject.Find("Player_Battle").GetComponent<PlayerStatus>();
        SetGuildItem();
        UpdateGuildItem();
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

        if (sideBarCanvas.interactable && isChangeButton)
        {
            SelectOfSideBar();
        }

        if (mainMenuCanvas.interactable && isChangeButton)
        {
            SelectOfItems();
        }

        if (attentionCanvas.interactable && isChangeButton)
        {
            SelectOfAttention();
        }
    }

    private void ResetDialog()
    {
        isChangeButton = false;
        EventSystem.current.SetSelectedGameObject(null);

        GuildItemCanvas.interactable = false;
        GuildItemCanvas.alpha = 0;
        sideBarCanvas.interactable = false;
        mainMenuCanvas.interactable = false;
        attentionCanvas.interactable = false;
        attentionCanvas.alpha = 0;
    }

    public void OpenDialog()
    {
        GuildItemCanvas.interactable = true;
        GuildItemCanvas.alpha = 1;
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
        attentionCanvas.interactable = false;
        attentionCanvas.alpha = 0;

        //納品依頼以外の場合
        if (GuildRequestManager.Instance.GuildRequest.RequestType == GuildRequest.RequestTypeEnum.Hunting)
        {
            messageText.text = "アイテム納品依頼を受けておりません";
        }
        else
        {
            messageText.text = "";
        }

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

    private void SelectOfSideBar()
    {
        isChangeButton = false;
    }

    private void SelectOfItems()
    {
        isChangeButton = false;

        if (EventSystem.current.currentSelectedGameObject == button_FirstGuildItem.gameObject)
        {
            messageText.text = firstGuildItem.Item.Information;
        }
        
        if(EventSystem.current.currentSelectedGameObject == button_SecondGuildItem.gameObject)
        {
            messageText.text = secondGuildItem.Item.Information;
        }
    }

    private void SelectOfAttention()
    {
        isChangeButton = false;
    }

    private void PushAddItemButton()
    {
        if (GuildRequestManager.Instance.GuildRequest.RequestType == GuildRequest.RequestTypeEnum.Hunting) return;

        SoundManager.Instance.PlaySE(10);
        isChangeButton = true;
        ActiveMainMenu();
    }

    private void PushEndButton()
    {
        SoundManager.Instance.PlaySE(10);
        CloseDialog();
    }

    private void PushGuildItemButton()
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

        if (selectItemCache == button_FirstGuildItem.gameObject)
        {
            StartCoroutine(AddGuildItemCoroutine(firstGuildItem));
        }
        
        if (selectItemCache == button_SecondGuildItem.gameObject)
        {
            StartCoroutine(AddGuildItemCoroutine(secondGuildItem));
        }
    }

    private IEnumerator AddGuildItemCoroutine(Button_GuildItem guildItem)
    {
        var hasItem = PlayData_OwnedItems.Instance.SearchItem(guildItem.Item);

        //アイテムを所持していない場合
        if (hasItem == null)
        {
            ActiveMessage();
            messageText.text = "アイテムを所持しておりません";
            yield return new WaitForSeconds(0.5f);
            ActiveMainMenu();
            selectItemImage.color = defaltButtonBgColor;
            EventSystem.current.SetSelectedGameObject(selectItemCache);
            yield break;
        }
        else
        {
            var hasItemNumberCache = hasItem.Number;
            for (var i = 0; hasItemNumberCache > i; i++)
            {
                //超過分は持って帰る
                if (!GuildRequestManager.Instance.FirstEventClearFlag && GuildRequestManager.Instance.SecondEventClearFlag)
                {
                    PlayData_OwnedItems.Instance.Use(hasItem.ItemData);
                    PlayData_OwnedItems.Instance.Save();
                    GuildRequestManager.Instance.CountUpEvent(guildItem.Item.GuildRequestEvent);
                }
            }

            UpdateGuildItem();

            //依頼クリア時の処理
            if (GuildRequestManager.Instance.FirstEventClearFlag && GuildRequestManager.Instance.SecondEventClearFlag)
            {
                CloseDialog();
            }
            else
            {
                ActiveMainMenu();
                selectItemImage.color = defaltButtonBgColor;
                EventSystem.current.SetSelectedGameObject(selectItemCache);
            }
        }
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
        isChangeButton = true;

        if (sideBarCanvas.interactable)
        {
            PushEndButton();
        }

        if (mainMenuCanvas.interactable)
        {
            SoundManager.Instance.PlaySE(10);
            ActiveSideBar();
        }

        if (attentionCanvas.interactable)
        {
            PushNoButton();
        }
    }

    private void SetButtonEvent()
    {
        button_AddItem.onClick.AddListener(PushAddItemButton);
        button_End.onClick.AddListener(PushEndButton);
        button_FirstGuildItem.onClick.AddListener(PushGuildItemButton);
        button_SecondGuildItem.onClick.AddListener(PushGuildItemButton);
        button_Yes.onClick.AddListener(PushYesButton);
        button_No.onClick.AddListener(PushNoButton);
    }

    private void SetGuildItem()
    {
        //討伐クエストの場合はボタン非表示
        if (GuildRequestManager.Instance.GuildRequest.RequestType == GuildRequest.RequestTypeEnum.Hunting)
        {
            button_FirstGuildItem.gameObject.SetActive(false);
            button_SecondGuildItem.gameObject.SetActive(false);
        }

        else
        {
            this.firstGuildItem = button_FirstGuildItem.GetComponent<Button_GuildItem>();
            var firstGuildItem = guildItemData.ItemList.FirstOrDefault(x => x.GuildRequestEvent == GuildRequestManager.Instance.GuildRequest.FirstTargetEvent);
            this.firstGuildItem.SetGuildItem(firstGuildItem);
            this.firstGuildItem.TargetNumber.text = GuildRequestManager.Instance.GuildRequest.FirstEventClearNumber.ToString();

            if (GuildRequestManager.Instance.IsSecondEvent)
            {
                this.secondGuildItem = button_SecondGuildItem.GetComponent<Button_GuildItem>();
                var secondGuildItem = guildItemData.ItemList.FirstOrDefault(x => x.GuildRequestEvent == GuildRequestManager.Instance.GuildRequest.SecondTargetEvent);
                this.secondGuildItem.SetGuildItem(secondGuildItem);
                this.secondGuildItem.TargetNumber.text = GuildRequestManager.Instance.GuildRequest.SecondEventClearNumber.ToString();
            }
            //第二イベントが設定されていない場合はボタン非表示
            else
            {
                button_SecondGuildItem.gameObject.SetActive(false);
            }
        }
    }

    private void UpdateGuildItem()
    {
        if (GuildRequestManager.Instance.GuildRequest.RequestType == GuildRequest.RequestTypeEnum.Hunting) return;
        firstGuildItem.CurrentNumber.text = GuildRequestManager.Instance.CurrentFirstEventClearNumber.ToString();

        if (!GuildRequestManager.Instance.IsSecondEvent) return;
        secondGuildItem.CurrentNumber.text = GuildRequestManager.Instance.CurrentSecondEventClearNumber.ToString();
    }
}
