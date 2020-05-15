using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalAttack : MonoBehaviour
{
    private PlayerStatus status;
    private Animator animator;
    [SerializeField] private float baseDamage;
    [SerializeField] private float coolTime;
    [SerializeField] private Collider hitDetecter;
    [SerializeField] private GameObject hitEF;

    void Start()
    {
        status = GetComponent<PlayerStatus>();
        animator = GetComponent<Animator>();
        hitDetecter.gameObject.SetActive(false);
        hitDetecter.enabled = false;
    }
    public void AttackStart()
    {
        if (!status.IsAttack)
        {
            status.ChangeStatusToAttack();
            SoundManager.Instance.PlaySE(6);
            animator.SetTrigger("Attack");
        }
    }

    public void OnAttackHitDetector()
    {
        hitDetecter.gameObject.SetActive(true);
        hitDetecter.enabled = true;
    }

    public void OnAttackHit(Collider collider)
    {
        var targetStatus = collider.GetComponent<EnemyStatus>();
        if (targetStatus == null || targetStatus.IsDie) return;

        //プレイヤー攻撃時はヒットEFを表示
        Instantiate(hitEF, hitDetecter.transform.position, Quaternion.identity);

        targetStatus.Damage(baseDamage * status.Attack);

        //ターゲットがプレイヤー未発見時に攻撃された場合、プレイヤーに気づく
        if (targetStatus.IsMove)
        {
            targetStatus.ChangeStatusToAngry();
        }
    }

    public void AttackFinish()
    {
        hitDetecter.gameObject.SetActive(false);
        hitDetecter.enabled = false;
        StartCoroutine(CoolTimeCoroutine());
    }

    private IEnumerator CoolTimeCoroutine()
    {
        yield return new WaitForSeconds(coolTime);
        status.ChangeStatusToMove();
    }
}
