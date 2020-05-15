using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dialog_Shop : MonoBehaviour
{
    //全体画面
    [SerializeField] private CanvasGroup shopCanvas;

    //money
    [SerializeField] private Text ownedMoney;

    //sideBar
    [SerializeField] private CanvasGroup sideBarCanvas;
    [SerializeField] private GameObject sideBarDefaultSelect;
    [SerializeField] private Button button_Buy;
    [SerializeField] private Button button_Sell;
    [SerializeField] private Button button_End;

    //MainMenu
    [SerializeField] private CanvasGroup mainMenuCanvas;
    [SerializeField] private CanvasGroup activeMainMenuCanvas;

    [SerializeField] private Button button_Item_Shop_Prefab;
    private const int MAX_ITEM_VIEW_NUMBER = 5;
    private GameObject selectItemCache;
    private Image selectItemImage;
    [SerializeField] private Color32 defaltButtonBgColor;
    [SerializeField] private Color32 selectButtonBgColor;
    [SerializeField] private Text messageText;

    //MainMenu > BuyItem
    [SerializeField] private CanvasGroup buyItemCanvas;
    [SerializeField] private GameObject buyItemDefaultSelect;
    [SerializeField] private GameObject buyItemsDirectory;
    [SerializeField] private Item_DataBase shopItemData;
    private Button_Item[] button_BuyItems;
    private int buyItemsLength;
    private Transform buyItemsContent;
    private Vector3 buyItemsContentCache;

    //MainMenu > SellItem
    [SerializeField] private CanvasGroup sellItemCanvas;
    [SerializeField] private GameObject sellItemDefaultSelect;
    [SerializeField] private GameObject sellItemsDirectory;
    [SerializeField] private Item_DataBase AllItem;
    private Button_Item[] button_SellItems;
    private CanvasGroup[] button_SellItemsCanvas;
    private int ownedItemsLength;
    private Transform sellItemsContent;
    private Vector3 sellItemsContentCache;

    //Attention
    [SerializeField] private CanvasGroup attentionCanvas;
    [SerializeField] private GameObject attentionDefaultSelect;
    [SerializeField] private Button button_Yes;
    [SerializeField] private Button button_No;

    [SerializeField] private PlayerStatus _status;
    private bool isChangeButton;
    private bool isCursorMovable = true;

    void Awake()
    {
        CreateBuyItem();
        CreateSellItem();

        buyItemsContent = buyItemsDirectory.transform;
        buyItemsContentCache = new Vector3(buyItemsDirectory.transform.localPosition.x, buyItemsDirectory.transform.localPosition.y);
        sellItemsContent = sellItemsDirectory.transform;
        sellItemsContentCache = new Vector3(sellItemsDirectory.transform.localPosition.x, sellItemsDirectory.transform.localPosition.y);
    }

    void Start()
    {
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

        if (attentionCanvas.interactable && isChangeButton)
        {
            SelectOfAttention();
        }
    }

    private void ResetDialog()
    {
        isChangeButton = false;
        EventSystem.current.SetSelectedGameObject(null);

        shopCanvas.interactable = false;
        shopCanvas.alpha = 0;
        sideBarCanvas.interactable = false;
        mainMenuCanvas.interactable = false;
        buyItemCanvas.interactable = false;
        buyItemCanvas.alpha = 0;
        sellItemCanvas.interactable = false;
        sellItemCanvas.alpha = 0;
        attentionCanvas.interactable = false;
        attentionCanvas.alpha = 0;
    }

    public void OpenDialog()
    {
        shopCanvas.interactable = true;
        shopCanvas.alpha = 1;
        SetOwnedMoney();
        ActiveSideBar();
        _status.ChangeStatusToFind();
    }

    private void CloseDialog()
    {
        ResetDialog();
        _status.ChangeStatusToMove();
    }

    private void ActiveSideBar()
    {
        sideBarCanvas.interactable = true;
        mainMenuCanvas.interactable = false;
        buyItemsContent.localPosition = buyItemsContentCache;
        sellItemsContent.localPosition = sellItemsContentCache;
        activeMainMenuCanvas.alpha = 1;
        buyItemCanvas.interactable = false;
        buyItemCanvas.alpha = 0;
        sellItemCanvas.interactable = false;
        sellItemCanvas.alpha = 0;
        attentionCanvas.interactable = false;
        attentionCanvas.alpha = 0;
        EventSystem.current.SetSelectedGameObject(sideBarDefaultSelect);
        messageText.text = "いらっしゃいませ！\nご要件はなんでしょうか？";
    }

    private void ActiveMainMenu() 
    {
        SetBuyItem();
        SetSellItem();
        sideBarCanvas.interactable = false;
        mainMenuCanvas.interactable = true;
        activeMainMenuCanvas.alpha = 0;
        attentionCanvas.interactable = false;
        attentionCanvas.alpha = 0;
    }

    private void ActiveBuyItem()
    {
        buyItemCanvas.interactable = true;
        buyItemCanvas.alpha = 1;
        sellItemCanvas.interactable = false;
        sellItemCanvas.alpha = 0;
    }

    private void ActiveSellItem()
    {
        buyItemCanvas.interactable = false;
        buyItemCanvas.alpha = 0;
        sellItemCanvas.interactable = true;
        sellItemCanvas.alpha = 1;
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
        EventSystem.current.SetSelectedGameObject(null);
        sideBarCanvas.interactable = false;
        mainMenuCanvas.interactable = false;
        attentionCanvas.interactable = false;
        attentionCanvas.alpha = 0;
    }

    private void PushBuyButton()
    {
        SoundManager.Instance.PlaySE(10);
        isChangeButton = true;
        ActiveMainMenu();
        ActiveBuyItem();
        EventSystem.current.SetSelectedGameObject(buyItemDefaultSelect);
    }

    private void PushSellButton()
    {
        SoundManager.Instance.PlaySE(10);
        isChangeButton = true;
        ActiveMainMenu();
        ActiveSellItem();
        EventSystem.current.SetSelectedGameObject(sellItemDefaultSelect);

    }

    private void PushEndButton()
    {
        SoundManager.Instance.PlaySE(10);
        StartCoroutine(PushEndButtnCoroutine());
    }

    private IEnumerator PushEndButtnCoroutine()
    {
        ActiveMessage();
        messageText.text = "またお越しくださいませ！";
        yield return new WaitForSeconds(0.5f);
        CloseDialog();
    }

    private void SelectOfItems()
    {
        isChangeButton = false;

        //アイテム未所持
        if (sellItemCanvas.alpha == 1 && ownedItemsLength == 0)
        {
            messageText.text = "アイテムを所持しておりません";
            return;
        }

        var index = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();

        if(buyItemCanvas.alpha == 1)
        {
            messageText.text = button_BuyItems[index].ItemData.Information;

            //スクロール処理
            if (index > MAX_ITEM_VIEW_NUMBER - 1 && Input.GetAxisRaw("VerticalXkey") == -1)
            {
                buyItemsContent.localPosition += new Vector3(0, 45);
            }
            
            if (index < buyItemsLength - MAX_ITEM_VIEW_NUMBER && Input.GetAxisRaw("VerticalXkey") == 1)
            {
                buyItemsContent.localPosition += new Vector3(0, -45);
            }
        }

        if (sellItemCanvas.alpha == 1)
        {
            messageText.text = button_SellItems[index].ItemData.Information;

            //スクロール処理
            if (index > MAX_ITEM_VIEW_NUMBER - 1 && Input.GetAxisRaw("VerticalXkey") == -1)
            {
                sellItemsContent.localPosition += new Vector3(0, 45);
            }
            if (index < ownedItemsLength - MAX_ITEM_VIEW_NUMBER && Input.GetAxisRaw("VerticalXkey") == 1)
            {
                sellItemsContent.localPosition += new Vector3(0, -45);
            }
        }
    }

    private void SelectOfAttention()
    {
        isChangeButton = false;
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

    private void PushYesButton()
    {
        StartCoroutine(PushYesButtonCoroutine());
    }

    private IEnumerator PushYesButtonCoroutine()
    {
        var selectItem = selectItemCache.GetComponent<Button_Item>();

        if(buyItemCanvas.alpha == 1)
        {
            //所持金不足
            if(selectItem.ItemData.BuyPrice > PlayData_OwnedMoney.Instance.OwnedMoney)
            {
                messageText.text = "所持金が不足しています";
                yield return new WaitForSeconds(0.5f);
                isChangeButton = true;
                PushNoButton();
                yield break;
            }

            SoundManager.Instance.PlaySE(5);
            PlayData_OwnedMoney.Instance.Use(selectItem.ItemData.BuyPrice);
            PlayData_OwnedMoney.Instance.Save();
            SetOwnedMoney();

            PlayData_OwnedItems.Instance.Add(selectItem.ItemData);
            PlayData_OwnedItems.Instance.Save();
            SetBuyItem();
        }

        if(sellItemCanvas.alpha == 1)
        {
            SoundManager.Instance.PlaySE(5);
            PlayData_OwnedMoney.Instance.Add(selectItem.ItemData.SellPrice);
            PlayData_OwnedMoney.Instance.Save();
            SetOwnedMoney();

            PlayData_OwnedItems.Instance.Use(selectItem.ItemData);
            PlayData_OwnedItems.Instance.Save();
            SetSellItem();
        }

        ActiveMessage();
        messageText.text = "毎度ありがとうございます";
        yield return new WaitForSeconds(0.5f);
        ActiveMainMenu();
        selectItemImage.color = defaltButtonBgColor;
        EventSystem.current.SetSelectedGameObject(selectItemCache);

        //所持アイテムが0の場合
        if (ownedItemsLength == 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        //所持アイテムの最後を売却した場合、一つ手前のアイテムを選択
        else if (selectItemCache.transform.GetSiblingIndex() == ownedItemsLength) 
        {
            EventSystem.current.SetSelectedGameObject(sellItemsDirectory.transform.GetChild(ownedItemsLength - 1).gameObject);
        }

        isChangeButton = true;
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
            if (buyItemCanvas.alpha == 1)
            {
                ActiveSideBar();
                EventSystem.current.SetSelectedGameObject(button_Buy.gameObject);
            }

            if (sellItemCanvas.alpha == 1)
            {
                ActiveSideBar();
                EventSystem.current.SetSelectedGameObject(button_Sell.gameObject);
            }
        }

        if (attentionCanvas.alpha == 1)
        {
            PushNoButton();
        }
    }

    private void CreateBuyItem()
    {
        //ショップデータを元に枠を作成
        buyItemsLength = shopItemData.ItemList.Length;

        //最初のみ別途付与
        buyItemDefaultSelect.GetComponent<Button>().onClick.AddListener(PushItemButton);

        for (int i = 1; i < buyItemsLength; i++)
        {
            var item = Instantiate(button_Item_Shop_Prefab, buyItemsDirectory.transform);
            item.onClick.AddListener(PushItemButton);
        }

        button_BuyItems = buyItemsDirectory.GetComponentsInChildren<Button_Item>();
    }

    private void CreateSellItem()
    {
        //最初のみ別途付与
        sellItemDefaultSelect.GetComponent<Button>().onClick.AddListener(PushItemButton);

        //全アイテムの枠を作成
        for (int i = 1; i < AllItem.ItemList.Length; i++)
        {
            var item = Instantiate(button_Item_Shop_Prefab, sellItemsDirectory.transform);
            item.onClick.AddListener(PushItemButton);
        }

        button_SellItems = sellItemsDirectory.GetComponentsInChildren<Button_Item>();
        button_SellItemsCanvas = sellItemsDirectory.GetComponentsInChildren<CanvasGroup>();
    }

    private void SetButtonEvent()
    {
        button_Buy.onClick.AddListener(PushBuyButton);
        button_Sell.onClick.AddListener(PushSellButton);
        button_End.onClick.AddListener(PushEndButton);
        button_Yes.onClick.AddListener(PushYesButton);
        button_No.onClick.AddListener(PushNoButton);
    }

    private void SetBuyItem()
    {
        for (int i = 0; i < buyItemsLength; i++)
        {
            button_BuyItems[i].SetItem(shopItemData.ItemList[i]);
            button_BuyItems[i].SetBuyPrice();
        }
    }

    private void SetSellItem()
    {
        ownedItemsLength = PlayData_OwnedItems.Instance.OwnedItemList.Length;

        for (int i = 0; i < AllItem.ItemList.Length; i++)
        {
            //所持アイテムのみデータを入れ、それ以外は非表示
            if (ownedItemsLength > i)
            {
                button_SellItems[i].SetItem(PlayData_OwnedItems.Instance.OwnedItemList[i].ItemData);
                button_SellItems[i].SetSellPrice();
                button_SellItemsCanvas[i].alpha = 1;
                button_SellItemsCanvas[i].interactable = true;
            }
            else
            {
                button_SellItemsCanvas[i].alpha = 0;
                button_SellItemsCanvas[i].interactable = false;
            }
        }
    }

    private void SetOwnedMoney()
    {
        ownedMoney.text = PlayData_OwnedMoney.Instance.OwnedMoney.ToString();
    }
}
