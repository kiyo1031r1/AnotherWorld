using System;
using UnityEngine;
using System.Linq;

public class ItemManager : MonoBehaviour
{
    [SerializeField] Item_Parameter_DataBase itemParameterList;
    public static ItemManager Instance => _instance;
    private static ItemManager _instance;

    private ItemManager()
    {

    }

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

    public void UseItem(Item useItem, PlayerStatus status)
    {
        switch (useItem.ItemCategory)
        {
            //パラメータ系
            case Item.ItemCategoryEnum.Parameter:
                var item = itemParameterList.Item_ParameteList.FirstOrDefault(x => x.ItemID == useItem.ItemID);

                //一時効果
                if (item.EffectTimeType == Item_Parameter.EffectTimeTypeEnum.Temporary)
                {

                    //固定値
                    if (item.EffectRatioType == Item_Parameter.EffectRatioTypeEnum.Fixed)
                    {
                        status.ChangeHPParameter(item.CurrentHP);
                    }

                    //割合
                    else if (item.EffectRatioType == Item_Parameter.EffectRatioTypeEnum.Percentage)
                    {
                        status.ChangeHPParameter((float)Math.Round(status.MaxHP * item.CurrentHP));
                        status.ChangeAttackParameter(item.Attack);
                    }

                    //効果時間
                    if (item.EffectTimeTarget == Item_Parameter.EffectTimeTargetEnum.Attack)
                    {
                        status.OnAttackBuff(item.EffectTime);
                    }
                }

                //永続効果
                else if (item.EffectTimeType == Item_Parameter.EffectTimeTypeEnum.Permanent)
                {

                    //固定値
                    if (item.EffectRatioType == Item_Parameter.EffectRatioTypeEnum.Fixed)
                    {
                        PlayData_Status.Instance.AddParameter(item.MaxHP, item.Attack);
                        status.SetParameter();
                        status.DisplayHP();

                        //バフ中の上書き処理
                        if (status.IsAttackBuff)
                        {
                            status.ChangeAttackParameter(status.AttackBuffCache);
                        }
                    }
                }

                PlayData_OwnedItems.Instance.Use(useItem);
                PlayData_OwnedItems.Instance.Save();
                break;
        }
    }
}
