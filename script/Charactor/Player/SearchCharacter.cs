using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchCharacter : MonoBehaviour
{
    private Transform _transform;
    private GameObject buttonRound;
    private CanvasGroup buttonRoundCanvas;
    private Dialog_Shop shopDialog;
    private Dialog_Guild guildDialog;
    private Dialog_GuildItem guildItemDialog;
    private PlayerStatus status;
    private enum TargetEnum
    {
        None,
        Item,
        Guild,
        Shop,
        GuildBox,
    }
    private TargetEnum _target;
    private GameObject _targetObject;

    void Start()
    {
        _transform = transform;
        status = GetComponent<PlayerStatus>();

        buttonRound = GameObject.Find("PopUpCanvas");
        if(buttonRound)
        {
            buttonRoundCanvas = buttonRound.GetComponent<CanvasGroup>();
        }

        //各ダイアログがある場合のみ取得
        var shop = GameObject.Find("ShopCanvas");
        if(shop)
        {
            shopDialog = shop.GetComponent<Dialog_Shop>();
        }

        var guild = GameObject.Find("GuildCanvas");
        if(guild)
        {
            guildDialog = guild.GetComponent<Dialog_Guild>();
        }

        var guildItem = GameObject.Find("GuildItemCanvas");
        if(guildItem)
        {
            guildItemDialog = guildItem.GetComponent<Dialog_GuildItem>();
        }

        buttonRoundCanvas.alpha = 0;
        buttonRoundCanvas.interactable = false;
    }

    public void InCharactorDetectorRange(Collider collider)
    {
        var targetDirection = collider.transform.position - _transform.position;
        var targetEuler = Vector3.Angle(_transform.forward, targetDirection);

        //視角内
        if (targetEuler <= status.ViewEuler)
        {
            //話しかける前
            if(!status.IsFind)
            {
                //〇ボタン吹き出し
                if (collider.CompareTag("ButtonRound"))
                {
                    buttonRoundCanvas.alpha = 1;

                    //親オブジェクトのタグ情報を取得
                    _targetObject = collider.transform.parent.gameObject;
                    switch (_targetObject.gameObject.tag)
                    {
                        case "Item":
                            _target = TargetEnum.Item;
                            break;
                        case "Guild":
                            _target = TargetEnum.Guild;
                            break;
                        case "Shop":
                            _target = TargetEnum.Shop;
                            break;
                        case "GuildBox":
                            _target = TargetEnum.GuildBox;
                            break;
                    }
                }
            }
            //話しかけた後
            else
            {
                buttonRoundCanvas.alpha = 0;
            }
        }

        //視角外
        else
        {
            buttonRoundCanvas.alpha = 0;
            _target = TargetEnum.None;
        }

    }

    public void OutCharactorDetectorRange(Collider collider)
    {
        buttonRoundCanvas.alpha = 0;
        _target = TargetEnum.None;
    }

    public void FindStart()
    {
        if (buttonRoundCanvas.alpha == 0) return;

        SoundManager.Instance.PlaySE(10);
        buttonRoundCanvas.alpha = 0;

        switch (_target)
        {
            case TargetEnum.Item:
                var dropItem = _targetObject.GetComponent<DropItem>();
                dropItem.GetItem();
                break;
            case TargetEnum.Guild:
                guildDialog.OpenDialog();
                break;
            case TargetEnum.Shop:
                shopDialog.OpenDialog();
                break;
            case TargetEnum.GuildBox:
                guildItemDialog.OpenDialog();
                break;
            default:
                break;
        }
    }
}
