using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShortRangeAttack_Jab : MonoBehaviour
{
    private EnemyStatus status;
    private Animator animator;
    [SerializeField] private float baseDamage;
    [SerializeField] private float coolTime;
    [SerializeField] private Collider hitDetecter;

    void Start()
    {
        status = GetComponent<EnemyStatus>();
        animator = GetComponent<Animator>();
        hitDetecter.gameObject.SetActive(false);
        hitDetecter.enabled = false;
    }

    public void AttackStart()
    {
        if (!status.IsAttack)
        {
            status.ChangeStatusToAttack();
            animator.SetTrigger("Jab");
            animator.SetFloat("MoveSpeed", 0);
        }
    }

    public void OnAttackHitDetector_Jab()
    {
        SoundManager.Instance.PlaySE(0);
        hitDetecter.gameObject.SetActive(true);
        hitDetecter.enabled = true;
    }

    public  void OnAttackHit(Collider collider)
    {
        var targetStatus = collider.GetComponent<PlayerStatus>();
        if (targetStatus == null) return;
        targetStatus.Damage(baseDamage * status.EnemyData.Attack);
    }

    public void AttackFinish_Jab()
    {
        hitDetecter.gameObject.SetActive(false);
        hitDetecter.enabled = false;
        StartCoroutine(CoolTimeCoroutine());
    }

    private IEnumerator CoolTimeCoroutine()
    {
        yield return new WaitForSeconds(coolTime);
        //一度攻撃すると激高状態となる
        status.ChangeStatusToAngry();
    }
}
