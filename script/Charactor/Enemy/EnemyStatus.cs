using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyStatus : MonoBehaviour
{
    public enum StatusEnum
    {
        Move,
        Chase,
        Angry,
        Attack,
        Die,
    }
    private StatusEnum status = StatusEnum.Move;

    public enum AttackRangeEnum
    {
        Long,
        Middle,
        Short,
        None,
    }
    private AttackRangeEnum _attackRange = AttackRangeEnum.None;

    private float currentHP;
    public Enemy EnemyData;
    private GuildRequestEvent guildRequestEvent;
    private Animator animator;
    private Transform _transform;
    private Vector3 defaultScale;

    public bool IsMove => status == StatusEnum.Move;
    public bool IsChase => status == StatusEnum.Chase;
    public bool IsAngry => status == StatusEnum.Angry;
    public bool IsAttack => status == StatusEnum.Attack;
    public bool IsDie => status == StatusEnum.Die;
    public AttackRangeEnum AttackRange => _attackRange;

    public float CurrentHP => currentHP;
    

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHP = EnemyData.MaxHP;
        guildRequestEvent = EnemyData.GuildRequestEvent;
        _transform = transform;
        defaultScale = _transform.localScale;
    }

    public void ChangeStatusToAttack()
    {
        if (IsAttack) return;
        status = StatusEnum.Attack;
    }

    public void ChangeStatusToMove()
    {
        if (IsDie) return;
        status = StatusEnum.Move;
    }

    public void ChangeStatusToChase()
    {
        if (IsDie) return;
        status = StatusEnum.Chase;
    }

    public void ChangeStatusToAngry()
    {
        if (IsDie) return;
        status = StatusEnum.Angry;
    }

    public void ChangeStatusToDie()
    {
        if (IsDie) return;
        status = StatusEnum.Die;
        OnDie();
    }

    public void Damage(float attack)
    {
        if (status == StatusEnum.Die) return;
        if (attack - EnemyData.Defence <= 0) return;

        SoundManager.Instance.PlaySE(14);
        currentHP -= attack - EnemyData.Defence;

        if (currentHP > 0)
        {
            //攻撃を受けた際すこしバウンドさせる
            _transform.localScale = _transform.localScale * 0.7f;
            _transform.DOScale(defaultScale, 0.5f).SetEase(Ease.OutBounce);
            return;
        }

        ChangeStatusToDie();
    }

    private void OnDie()
    {
        SoundManager.Instance.PlaySE(EnemyData.DieSE);
        PlayData_Status.Instance.AddExp(EnemyData.Exp);
        GuildRequestManager.Instance.CountUpEvent(guildRequestEvent);

        //死亡時にすこし体をゆらす
        _transform.DOShakeScale(1.0f)
            .OnComplete(() =>
            {
                animator.SetTrigger("Die");
                StartCoroutine(DestroyCoroutine());
            });
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        SearchEnemy.Instance.RemoveEnemyList(gameObject);
    }

    public void ChangeRangeToMiddle()
    {
        _attackRange = AttackRangeEnum.Middle;
    }

    public void ChangeRangeToShort()
    {
        _attackRange = AttackRangeEnum.Short;
    }

    public void ChangeRangeToNone()
    {
        _attackRange = AttackRangeEnum.None;
    }
}
