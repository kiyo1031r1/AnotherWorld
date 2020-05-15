using System;
using UnityEngine;
using UnityEditor;
using System.Linq;

[Serializable]
public class PlayData_Status
{
    const string playerPrefsKey = "STATUS";
    private static PlayData_Status instance;
    public static PlayData_Status Instance
    {
        get
        {
            if(instance == null)
            {
                instance = PlayerPrefs.HasKey(playerPrefsKey)
                    ? JsonUtility.FromJson<PlayData_Status>(PlayerPrefs.GetString(playerPrefsKey))
                    : new PlayData_Status();
            }
            return instance;
        }

    }

    private PlayData_Status()
    {

    }

    [SerializeField] public PlayerStatusData_DataBase playerStatusDataBase;
    [SerializeField] public GuildRankUp_DataBase guildRankUpDataBase;
    [SerializeField] private int level;
    private const int MAX_LEVEl = 10;
    [SerializeField] private float hitPoint;
    [SerializeField] private float attack;
    [SerializeField] private float defence;
    [SerializeField] private float needExp;
    [SerializeField] private float currentExp;
    [SerializeField] private GuildRequest.RankEnum rank;
    [SerializeField] private int clearIronNumber;
    [SerializeField] private int clearBronzeNumber;
    [SerializeField] private int clearCopperNumber;
    [SerializeField] private bool _isRankUp;

    public int Level => level;
    public float HitPoint => hitPoint;
    public float Attack => attack;
    public  float Defence => defence;
    public  float NeedExp => needExp;
    public  float CurrentExp => currentExp;
    public GuildRequest.RankEnum Rank => rank;
    public string RankText
    {
        get
        {
            string rankText = "";
            switch(rank)
            {
                case GuildRequest.RankEnum.Iron:
                    rankText = "アイアン級";
                    break;
                case GuildRequest.RankEnum.Bronze:
                    rankText = "ブロンズ級";
                    break;
                case GuildRequest.RankEnum.Copper:
                    rankText = "カッパー級";
                    break;
            }
            return rankText;
         }
    }
    public  int ClearIronNumber => clearIronNumber;
    public int ClearBronzeNumber => clearBronzeNumber;
    public int ClearCopperNumber => clearCopperNumber;
    public bool IsRankUp => _isRankUp;

    //TODO スタート画面で初めから選択時に使用
    public void LoadFirstStatus()
    {
        var firstStatus = playerStatusDataBase.playerStatusDataList.Find(x => x.level == 1);
        level = firstStatus.level;
        hitPoint = firstStatus.hitPoint;
        attack = firstStatus.attack;
        defence = firstStatus.defence;
        needExp = firstStatus.needExp;
        currentExp = 0;
        rank = GuildRequest.RankEnum.Iron;
        clearIronNumber = 0;
        clearBronzeNumber = 0;
        clearCopperNumber = 0;
    }

    public void Save()
    {
        var saveData = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(playerPrefsKey, saveData);
        PlayerPrefs.Save();
    }

    public void AddExp(float exp)
    {
        if (needExp == 0) return;
        currentExp += exp;
        while (true)
        {
            if (currentExp >= needExp)
            {
                exp = currentExp - needExp;
                LevelUP();

                if (needExp == 0)
                {
                    break;
                }
                currentExp = exp;
            }
            else
            {
                break;
            }
        }
        Save();
    }

    public void AddParameter(float addHitPoint = 0, float addAttack = 0)
    {
        hitPoint += addHitPoint;
        attack += addAttack;
        Save();
    }

    public void LevelUP()
    {
        if (level >= MAX_LEVEl) return;

        level += 1;
        var addStatus = playerStatusDataBase.playerStatusDataList.Find(x => x.level == level);
        hitPoint += addStatus.hitPoint;
        attack += addStatus.attack;
        defence += addStatus.defence;
        needExp = addStatus.needExp;
        currentExp = 0;

        var playerStatus = GameObject.Find("Player_Battle").GetComponent<PlayerStatus>();
        playerStatus.SetParameter();

        if(playerStatus.IsAttackBuff)
        {
            playerStatus.ChangeAttackParameter(playerStatus.AttackBuffCache);
        }

        Message.Instance.LevelUpMessaga();
    }

    public void AddClearGuildRequest(GuildRequest.RankEnum rank)
    {
        switch (rank)
        {
            case GuildRequest.RankEnum.Iron:
                clearIronNumber++;
                break;

            case GuildRequest.RankEnum.Bronze:
                clearBronzeNumber++;
                break;

            case GuildRequest.RankEnum.Copper:
                clearCopperNumber++;
                break;
        }
        RankUP();
        Save();
    }

    public void RankUP()
    {
        //条件を元にギルドランクをアップ
        var rankData = guildRankUpDataBase.GuildRankUpList.FirstOrDefault(x => x.Rank == rank);

        if (clearIronNumber >= rankData.ClearIronNumber
            && clearBronzeNumber >= rankData.ClearBronzeNumber
            && clearCopperNumber >= rankData.ClearCopperNumber)
        {
            rank = rankData.NextRank;
            _isRankUp = true;
        }
    }

    public void OffRankUpFlug()
    {
        if(_isRankUp)
        {
            _isRankUp = false;
            Save();
        }
    }
}
