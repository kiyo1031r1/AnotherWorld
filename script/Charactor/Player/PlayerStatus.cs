using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public enum StatusEnum
    {
        Move,
        Attack,
        Die,
        Find,
    }

    private StatusEnum status = StatusEnum.Move;
    private float maxHP;
    private float currentHP;
    private float attack;
    private float defence;
    [SerializeField] private float viewEuler;

    private Animator animator;
    private Slider hitPointSlider;
    private Dialog_PlayerMenu playerMenu;

    private bool isAttackBuff;
    private  float attackBuffCache;
    private float attackBuffTime;
    private Image attackBuffIcon;

    public bool IsMove => status == StatusEnum.Move;
    public bool IsAttack => status == StatusEnum.Attack;
    public bool IsDie => status == StatusEnum.Die;
    public bool IsFind => status == StatusEnum.Find;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;
    public float Attack => attack;
    public float Defence => defence;
    public float ViewEuler => viewEuler;

    public bool IsAttackBuff => isAttackBuff;
    public float AttackBuffCache => attackBuffCache;

    void Start()
    {
        playerMenu = GameObject.Find("PlayerMenuCanvas").GetComponent<Dialog_PlayerMenu>();
        
        var hitPointSliderObj = GameObject.Find("HitPointSlider");
        if (hitPointSliderObj)
        {
            hitPointSlider = hitPointSliderObj.GetComponent<Slider>();
        }

        var attackBuffIconObj = GameObject.Find("ATKBuff");
        if(attackBuffIconObj)
        {
            attackBuffIcon = attackBuffIconObj.GetComponent<Image>();
        }

        animator = GetComponent<Animator>();
        SetParameter();
        currentHP = maxHP;
        DisplayHP();
        AttackBuffTimer();
    }

    void Update()
    {
        if (isAttackBuff)
        {
            AttackBuffTimer();
        }
    }

    public void SetParameter()
    {
        maxHP = PlayData_Status.Instance.HitPoint;
        attack = PlayData_Status.Instance.Attack;
        defence = PlayData_Status.Instance.Defence;
    }

    public void DisplayHP()
    {
        //Battleシーン以外の処理
        if (hitPointSlider == null) return;

        hitPointSlider.value = currentHP / maxHP;
    }

    public void ChangeStatusToAttack()
    {
        if (IsDie) return;
        status = StatusEnum.Attack;
    }

    public void ChangeStatusToMove()
    {
        if (IsDie) return;
        status = StatusEnum.Move;
    }

    public void ChangeStatusToDie()
    {
        if (IsDie) return;
        status = StatusEnum.Die;
        OnDie();
    }

    public void ChangeStatusToFind()
    {
        if (IsDie) return;
        status = StatusEnum.Find;
    }

    public void Damage(float attack)
    {
        if (IsDie || GuildRequestManager.Instance.FirstEventClearFlag && GuildRequestManager.Instance.SecondEventClearFlag) return;
        GetDamageEF.Instance.OnGetDamage();

        if (attack - defence <= 0) return;

        currentHP -= attack - defence;
        DisplayHP();
        playerMenu.UpdateHP();

        if (currentHP > 0) return;
        ChangeStatusToDie();
    }

    private void OnDie()
    {
        animator.SetTrigger("Die");
        GuildRequestManager.Instance.FailedRequest();
    }

    public void ChangeHPParameter(float currentHP)
    {
        var changeHP = this.currentHP + currentHP;
        this.currentHP = changeHP;

        if (changeHP > MaxHP)
        {
            this.currentHP = MaxHP;
        }
        DisplayHP();
    }

    public void ChangeAttackParameter(float attack)
    {
        //LevelUp時のattack上書き用に値を一時保存
        attackBuffCache = attack;

        this.attack = (float)Math.Round(PlayData_Status.Instance.Attack * attack);
    }

    public void OnAttackBuff(float time)
    {
        isAttackBuff = true;
        attackBuffIcon.color = new Color32(255, 255, 255, 255);
        attackBuffTime = time;
    }

    private void AttackBuffTimer()
    {
        if (attackBuffIcon == null) return;
        
        if (attackBuffTime > 0)
        {
            attackBuffTime -= Time.deltaTime;
        }

        if (attackBuffTime <= 0)
        {
            attack = PlayData_Status.Instance.Attack;
            isAttackBuff = false;
            attackBuffIcon.color = new Color32(255, 255, 255, 0);
            playerMenu.SetStatus();
            attackBuffTime = 0;
            attackBuffCache = 0;
        }
    }
}
