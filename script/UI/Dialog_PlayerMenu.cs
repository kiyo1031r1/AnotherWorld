using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dialog_PlayerMenu : MonoBehaviour
{
    //全体画面
    [SerializeField] private CanvasGroup playerMenuCanvas;

    //money
    [SerializeField] private Text ownedMoney;

    //sideBar
    [SerializeField] private GameObject sideBarDefaultSelect;
    [SerializeField] private CanvasGroup sideBarCanvas;
    [SerializeField] private Button button_Status;
    [SerializeField] private Button button_Items;
    [SerializeField] private Button button_GuildInformation;
    [SerializeField] private Button button_End;

    //MainMenu
    [SerializeField] private CanvasGroup mainMenuCanvas;
    [SerializeField] private CanvasGroup activeMainMenuCanvas;

    [SerializeField] private Button button_OwnedItem_Prefab;
    private Transform itemsContent;
    private Vector3 itemsContentCache;
    private const int MAX_ITEM_VIEW_NUMBER = 5;
    private GameObject selectItemCache;
    private Image selectItemImage;
    [SerializeField] private Color32 defaltButtonBgColor;
    [SerializeField] private Color32 selectButtonBgColor;
    [SerializeField] private Text messageText;

    //MainMenu > Items
    [SerializeField] private CanvasGroup ownedItemsCanvas;
    [SerializeField] private GameObject ownedItemsDefaultSelect;
    [SerializeField] private GameObject itemsDirectory;
    private Button_Item[] button_OwnedItems;
    private CanvasGroup[] button_OwnedItemsCanvas;
    private int ownedItemsLength;

    //MainMenu > Status
    [SerializeField] private CanvasGroup statusCanvas;
    [SerializeField] private Text level;
    [SerializeField] private Text hitPoint;
    [SerializeField] private Text attack;
    [SerializeField] private Text defence;
    [SerializeField] private Text exp;

    //MainMenu > GuildInformation
    [SerializeField] private CanvasGroup guildInformationCanvas;
    [SerializeField] private GameObject guildInformationDefalutSelect;
    [SerializeField] private CanvasGroup requestInformationsCanvas;
    [SerializeField] private CanvasGroup requestInformationEnptyCanvas;
    [SerializeField] private Text requestName;
    [SerializeField] private Text requestReward;
    [SerializeField] private Text requestRank;
    [SerializeField] private Text requestFirstEventName;
    [SerializeField] private Text requestFirstEventNumber;
    [SerializeField] private Text requestSecondEventName;
    [SerializeField] private Text requestSecondEventNumber;
    [SerializeField] private Button button_retire;
    [SerializeField] private Text rank;
    [SerializeField] private Text clearIron;
    [SerializeField] private Text clearBronze;
    [SerializeField] private Text clearCapper;

    //end
    private bool isEndable;

    //Attention
    [SerializeField] private CanvasGroup attentionCanvas;
    [SerializeField] private Button button_Yes;
    [SerializeField] private Button button_No;
    [SerializeField] private GameObject attentionDefaultSelect;

    private PlayerStatus status;
    private bool isChangeButton;
    private bool isCursorMovable = true;
    private GameObject currentSelectObject;

    [SerializeField] private Item_DataBase AllItem;

    void Awake()
    {
        CreateOwnedItem();
        itemsContent = itemsDirectory.transform;
        itemsContentCache = new Vector3(itemsDirectory.transform.localPosition.x, itemsDirectory.transform.localPosition.y);
    }

    void Start()
    {
        var playerTownObj = GameObject.Find("Player_Town");
        var playerBattleObj = GameObject.Find("Player_Battle");
        if(playerTownObj)
        {
            status = playerTownObj.GetComponent<PlayerStatus>();
        }
        else if(playerBattleObj)
        {
            status = playerBattleObj.GetComponent<PlayerStatus>();
        }

        SetGuildInformation();
        SetButtonEvent();
        ResetDialog();
    }

    void Update()
    {
        //マウスクリック無効化
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            EventSystem.current.SetSelectedGameObject(currentSelectObject);
        }

        //ギルド依頼中の入力重複防止
        if (playerMenuCanvas.interactable)
        {
            //×ボタン
            if (Input.GetButtonDown("Fire2"))
            {
                PushCancelButton();
            }

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
                currentSelectObject = EventSystem.current.currentSelectedGameObject;
                isCursorMovable = true;
            }

            if (sideBarCanvas.interactable && isChangeButton)
            {
                SelectOfSideBar();
            }

            if (ownedItemsCanvas.interactable && isChangeButton)
            {
                SelectOfItems();
            }

            if (attentionCanvas.interactable && isChangeButton)
            {
                SelectOfAttention();
            }
        }
    }

    private void ResetDialog()
    {
        isChangeButton = false;
        EventSystem.current.SetSelectedGameObject(null);

        playerMenuCanvas.interactable = false;
        playerMenuCanvas.alpha = 0;
        sideBarCanvas.interactable = false;
        mainMenuCanvas.interactable = false;
        statusCanvas.interactable = false;
        statusCanvas.alpha = 0;
        ownedItemsCanvas.interactable = false;
        ownedItemsCanvas.alpha = 0;
        guildInformationCanvas.interactable = false;
        guildInformationCanvas.alpha = 0;
        attentionCanvas.interactable = false;
        attentionCanvas.alpha = 0;
    }

    public void OpenDialog()
    {
        SoundManager.Instance.PlaySE(10);
        playerMenuCanvas.interactable = true;
        playerMenuCanvas.alpha = 1;
        SetOwnedItem();
        SetOwnedMoney();
        SetStatus();
        DisplayGuildInformation();
        ActiveSideBar();
        ownedItemsCanvas.alpha = 1;
        status.ChangeStatusToFind();
    }

    private void CloseDialog()
    {
        ResetDialog();
        status.ChangeStatusToMove();
    }

    private void ActiveSideBar()
    {
        //アイテム使用後の内容を反映
        SetStatus();

        sideBarCanvas.interactable = true;
        mainMenuCanvas.interactable = false;
        activeMainMenuCanvas.alpha = 1;
        attentionCanvas.interactable = false;
        attentionCanvas.alpha = 0;
        messageText.text = "";
        itemsContent.localPosition = itemsContentCache;
        EventSystem.current.SetSelectedGameObject(sideBarDefaultSelect);
        currentSelectObject = EventSystem.current.currentSelectedGameObject;
    }

    private void ActiveMainMenu()
    {
        sideBarCanvas.interactable = false;
        mainMenuCanvas.interactable = true;
        activeMainMenuCanvas.alpha = 0;
        attentionCanvas.interactable = false;
        attentionCanvas.alpha = 0;
    }

    private void ActiveStatusMenu()
    {
        statusCanvas.interactable = true;
        ownedItemsCanvas.interactable = false;
        guildInformationCanvas.interactable = false;
    }

    private void ActiveItemsMenu()
    {
        statusCanvas.interactable = false;
        ownedItemsCanvas.interactable = true;
        guildInformationCanvas.interactable = false;
        EventSystem.current.SetSelectedGameObject(ownedItemsDefaultSelect);
        currentSelectObject = EventSystem.current.currentSelectedGameObject;
    }

    private void ActiveGuildInformationMenu()
    {
        statusCanvas.interactable = false;
        ownedItemsCanvas.interactable = false;
        guildInformationCanvas.interactable = true;

        //ギルド依頼を受けていない場合のリタイアボタン対策
        if(GuildRequestManager.Instance.IsRequest)
        {
            EventSystem.current.SetSelectedGameObject(guildInformationDefalutSelect);
            currentSelectObject = EventSystem.current.currentSelectedGameObject;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void ActiveAttention()
    {
        sideBarCanvas.interactable = false;
        mainMenuCanvas.interactable = false;
        ownedItemsCanvas.interactable = false;
        guildInformationCanvas.interactable = false;
        attentionCanvas.alpha = 1;
        attentionCanvas.interactable = true;
        EventSystem.current.SetSelectedGameObject(attentionDefaultSelect);
        currentSelectObject = EventSystem.current.currentSelectedGameObject;
    }

    private void SelectOfSideBar()
    {
        isChangeButton = false;

        //メインメニュー表示切替
        if (EventSystem.current.currentSelectedGameObject == button_Items.gameObject)
        {
            statusCanvas.alpha = 0;
            ownedItemsCanvas.alpha = 1;
            guildInformationCanvas.alpha = 0;
        }

        if (EventSystem.current.currentSelectedGameObject == button_Status.gameObject)
        {
            statusCanvas.alpha = 1;
            ownedItemsCanvas.alpha = 0;
            guildInformationCanvas.alpha = 0;
        }

        if (EventSystem.current.currentSelectedGameObject == button_GuildInformation.gameObject)
        {
            statusCanvas.alpha = 0;
            ownedItemsCanvas.alpha = 0;
            guildInformationCanvas.alpha = 1;
        }
    }

    private void SelectOfItems()
    {
        isChangeButton = false;

        if (ownedItemsCanvas.alpha == 1 && ownedItemsLength == 0)
        {
            messageText.text = "アイテムを所持しておりません";
            return;
        }

        var index = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();
        messageText.text = button_OwnedItems[index].ItemData.Information;

        //スクロール処理
        if (index > MAX_ITEM_VIEW_NUMBER - 1 && Input.GetAxisRaw("VerticalXkey") == -1)
        {
            itemsContent.localPosition += new Vector3(0, 45);
        }

        if (index < ownedItemsLength - MAX_ITEM_VIEW_NUMBER && Input.GetAxisRaw("VerticalXkey") == 1)
        {
            itemsContent.localPosition += new Vector3(0, -45);
        }
    }

    private void SelectOfAttention()
    {
        isChangeButton = false;
    }

    private void PushStatusButton()
    {
        SoundManager.Instance.PlaySE(10);
        isChangeButton = true;
        ActiveMainMenu();
        ActiveStatusMenu();
    }

    private void PushItemsButton()
    {
        SoundManager.Instance.PlaySE(10);
        isChangeButton = true;
        ActiveMainMenu();
        ActiveItemsMenu();
    }

    private void PushGuildInformationButton()
    {
        SoundManager.Instance.PlaySE(10);
        isChangeButton = true;
        ActiveMainMenu();
        ActiveGuildInformationMenu();
    }

    private void PushEndButton()
    {
        SoundManager.Instance.PlaySE(10);
        isEndable = true;
        ActiveAttention();
    }

    private void PushItemButton()
    {
        SoundManager.Instance.PlaySE(10);
        isChangeButton = true;
        selectItemCache = EventSystem.current.currentSelectedGameObject;
        selectItemImage = selectItemCache.GetComponent<Image>();
        selectItemImage.color = selectButtonBgColor;
        ActiveAttention();
    }

    private void PushRetireButton()
    {
        SoundManager.Instance.PlaySE(10);
        PushItemButton();
    }


    private void PushYesButton()
    {
        SoundManager.Instance.PlaySE(10);
        StartCoroutine(PushYesButtonCoroutine());
    }
    private IEnumerator PushYesButtonCoroutine()
    {
        //ゲーム終了確認時
        if (isEndable)
        {
            //コンパイル時はコメントアウトする
                //UnityEditor.EditorApplication.isPlaying = false;
            
            Application.Quit();
            yield break;
        }

        //所持アイテム画面の処理
        if (ownedItemsCanvas.alpha == 1)
        {
            isChangeButton = true;

            var selectItem = selectItemCache.GetComponent<Button_Item>();

            //街では使用不可能な仕様
            if (SceneManager.GetActiveScene().name == "TownScene")
            {
                messageText.text = "街の中では使用できません";
                yield return new WaitForSeconds(0.5f);
                PushNoButton();
                yield break;
            }

            //納品アイテムの場合
            if (selectItem.ItemData.ItemCategory == Item.ItemCategoryEnum.Guild)
            {
                messageText.text = "納品アイテムの為、使用できません";
                yield return new WaitForSeconds(0.5f);
                PushNoButton();
                yield break;
            }

            //納品アイテム以外は使用可能
            ItemManager.Instance.UseItem(selectItem.ItemData, status);
            SetOwnedItem();

            ActiveMainMenu();
            ActiveItemsMenu();
            selectItemImage.color = defaltButtonBgColor;
            EventSystem.current.SetSelectedGameObject(selectItemCache);
            currentSelectObject = EventSystem.current.currentSelectedGameObject;

            //使用後に所持数が0になった場合の処理
            if (ownedItemsLength == 0)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
            else if (selectItemCache.transform.GetSiblingIndex() == ownedItemsLength)
            {
                EventSystem.current.SetSelectedGameObject(itemsDirectory.transform.GetChild(ownedItemsLength - 1).gameObject);
                currentSelectObject = EventSystem.current.currentSelectedGameObject;
            }
        }

        //ギルド情報画面の処理
        else if (guildInformationCanvas.alpha == 1)
        {
            isChangeButton = true;
            selectItemImage.color = defaltButtonBgColor;
            status.ChangeStatusToDie();
            CloseDialog();
        }
    }

    private void PushNoButton()
    {
        SoundManager.Instance.PlaySE(10);
        isChangeButton = true;

        //ゲーム終了確認時
        if (isEndable)
        {
            isEndable = false;
            ActiveSideBar();
            EventSystem.current.SetSelectedGameObject(button_End.gameObject);
            currentSelectObject = EventSystem.current.currentSelectedGameObject;
            return;
        }

        //所持アイテム画面の処理
        if (ownedItemsCanvas.alpha == 1)
        {
            selectItemImage.color = defaltButtonBgColor;
            ActiveMainMenu();
            ActiveItemsMenu();
            EventSystem.current.SetSelectedGameObject(selectItemCache);
            currentSelectObject = EventSystem.current.currentSelectedGameObject;
        }

        //ギルド情報画面の処理
        else if (guildInformationCanvas.alpha == 1)
        {
            selectItemImage.color = defaltButtonBgColor;
            ActiveMainMenu();
            ActiveGuildInformationMenu();
        }
    }

    private void PushCancelButton()
    {
        SoundManager.Instance.PlaySE(10);
        isChangeButton = true;

        //ゲーム終了確認時
        if (isEndable)
        {
            isEndable = false;
            ActiveSideBar();
            EventSystem.current.SetSelectedGameObject(button_End.gameObject);
            currentSelectObject = EventSystem.current.currentSelectedGameObject;
            return;
        }

        if (sideBarCanvas.interactable)
        {
            CloseDialog();
        }

        if (mainMenuCanvas.interactable)
        {
            ActiveSideBar();

            //defaultのステータス以外のボタンフォーカス処理
            if (statusCanvas.interactable)
            {
                EventSystem.current.SetSelectedGameObject(button_Status.gameObject);
                currentSelectObject = EventSystem.current.currentSelectedGameObject;
            }
            if(guildInformationCanvas.interactable)
            {
                EventSystem.current.SetSelectedGameObject(button_GuildInformation.gameObject);
                currentSelectObject = EventSystem.current.currentSelectedGameObject;
            }
        }

        if (attentionCanvas.interactable)
        {
            //所持アイテム画面の処理
            if (ownedItemsCanvas.alpha == 1)
            {
                selectItemImage.color = defaltButtonBgColor;
                ActiveMainMenu();
                ActiveItemsMenu();
                EventSystem.current.SetSelectedGameObject(selectItemCache);
                currentSelectObject = EventSystem.current.currentSelectedGameObject;
            }

            //ギルド情報画面の処理
            if (guildInformationCanvas.alpha == 1)
            {
                selectItemImage.color = defaltButtonBgColor;
                ActiveMainMenu();
                ActiveGuildInformationMenu();
            }
        }

    }

    private void CreateOwnedItem()
    {
        //最初のみ別途付与
        ownedItemsDefaultSelect.GetComponent<Button>().onClick.AddListener(PushItemButton);

        //全アイテムの枠を作成
        for (int i = 1; i < AllItem.ItemList.Length; i++)
        {
            var item = Instantiate(button_OwnedItem_Prefab, itemsDirectory.transform);
            item.onClick.AddListener(PushItemButton);
        }

        button_OwnedItems = itemsDirectory.GetComponentsInChildren<Button_Item>();
        button_OwnedItemsCanvas = itemsDirectory.GetComponentsInChildren<CanvasGroup>();
    }

    private void SetButtonEvent()
    {
        button_Status.onClick.AddListener(PushStatusButton);
        button_Items.onClick.AddListener(PushItemsButton);
        button_GuildInformation.onClick.AddListener(PushGuildInformationButton);
        button_End.onClick.AddListener(PushEndButton);
        button_Yes.onClick.AddListener(PushYesButton);
        button_No.onClick.AddListener(PushNoButton);
        button_retire.onClick.AddListener(PushRetireButton);
    }

    private void SetOwnedItem()
    {
        ownedItemsLength = PlayData_OwnedItems.Instance.OwnedItemList.Length;
        for (int i = 0; i < AllItem.ItemList.Length; i++)
        {
            //所持アイテムのみデータを入れ、それ以外は非表示
            if (ownedItemsLength > i)
            {
                button_OwnedItems[i].SetItem(PlayData_OwnedItems.Instance.OwnedItemList[i].ItemData);
                button_OwnedItemsCanvas[i].alpha = 1;
                button_OwnedItemsCanvas[i].interactable = true;
            }
            else
            {
                button_OwnedItemsCanvas[i].alpha = 0;
                button_OwnedItemsCanvas[i].interactable = false;
            }
        }
    }

    private void SetOwnedMoney()
    {
        ownedMoney.text = PlayData_OwnedMoney.Instance.OwnedMoney.ToString();
    }

    public void SetStatus()
    {
        //LvとExpはPlayDataで管理
        //その他はアイテム等で変動する為ステータスから取得
        level.text = PlayData_Status.Instance.Level.ToString();
        hitPoint.text = status.CurrentHP + " / " + status.MaxHP;
        attack.text = status.Attack.ToString();
        defence.text = status.Defence.ToString();
        exp.text = PlayData_Status.Instance.CurrentExp + " / " + PlayData_Status.Instance.NeedExp;
    }

    public void UpdateHP()
    {
        hitPoint.text = status.CurrentHP + " / " + status.MaxHP;
    }

    private void SetGuildInformation()
    {
        rank.text = PlayData_Status.Instance.RankText;
        clearIron.text = PlayData_Status.Instance.ClearIronNumber.ToString();
        clearBronze.text = PlayData_Status.Instance.ClearBronzeNumber.ToString();
        clearCapper.text = PlayData_Status.Instance.ClearCopperNumber.ToString();

        //フィールド遷移後のみ確認可能な仕様
        if (!GuildRequestManager.Instance.GuildRequest) return;
        
        requestName.text = GuildRequestManager.Instance.GuildRequest.RequestName;
        requestReward.text = GuildRequestManager.Instance.GuildRequest.Reward + "円";
        requestRank.text = GuildRequestManager.Instance.GuildRequest.RankText;
        requestFirstEventName.text = GuildRequestManager.Instance.FirstEventName;

        //第二イベントの有無により表示切替
        if (GuildRequestManager.Instance.IsSecondEvent)
        {
            requestSecondEventName.text = GuildRequestManager.Instance.SecondEventName;
        }
        else
        {
            requestSecondEventName.text = "";
            requestSecondEventNumber.text = "";
        }
    }

    private void DisplayGuildInformation()
    {
        //ギルド依頼時の表示
        if (GuildRequestManager.Instance.IsRequest)
        {
            requestInformationsCanvas.alpha = 1;
            requestInformationEnptyCanvas.alpha = 0;
            UpdateGuildInformation();
        }
        //ギルド未依頼時の表示
        else
        {
            requestInformationsCanvas.alpha = 0;
            requestInformationEnptyCanvas.alpha = 1;
        }
    }

    //ギルド依頼状況の更新
    private void UpdateGuildInformation()
    {
        requestFirstEventNumber.text =
            GuildRequestManager.Instance.CurrentFirstEventClearNumber + " / " + GuildRequestManager.Instance.GuildRequest.FirstEventClearNumber;

        if (!GuildRequestManager.Instance.IsSecondEvent) return;
        requestSecondEventNumber.text =
            GuildRequestManager.Instance.CurrentSecondEventClearNumber + " / " + GuildRequestManager.Instance.GuildRequest.SecondEventClearNumber;
    }

}
