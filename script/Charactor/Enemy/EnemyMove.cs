using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyStatus))]

public class EnemyMove : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform _transform;
    private EnemyStatus status;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        status = GetComponent<EnemyStatus>();
        animator = GetComponent<Animator>();
        _transform = transform;
        agent.speed = status.EnemyData.Speed;
    }

    void Update()
    {
        if(status.IsMove)
        {
            StartCoroutine(RandomMove());
        }
    }

    private IEnumerator RandomMove()
    {

        while(true)
        {
            if(agent.pathStatus != NavMeshPathStatus.PathInvalid && status.IsMove && !agent.hasPath)
            {
                var nextPosition = _transform.position +
                new Vector3((Random.Range(-1f, 1f)), 0f, Random.Range(-1f, 1f)).normalized *
                Random.Range(0.5f, 8f);
                agent.destination = nextPosition;
                animator.SetFloat("MoveSpeed", agent.speed);
            }
            yield return new WaitForSeconds(Random.Range(5.0f, 10f));
        }
    }

    public void InPlayerDetectRange(Collider collider)
    {
        //通常時は視野に入らなければ気づかれない
        if (status.IsMove)
        {
            var targetDirection = collider.transform.position - _transform.position;
            var targetEuler = Vector3.Angle(_transform.forward, targetDirection);
            if (targetEuler <= status.EnemyData.ViewEuler)
            {
                status.ChangeStatusToChase();
            }
        }
        //追跡中は追跡範囲内にプレイヤーがいると接近
        else if(status.IsChase)
        {
            agent.isStopped = false;
            agent.destination = collider.transform.position;
            animator.SetFloat("MoveSpeed", agent.speed);
        }
    }

    public void InPlayerDetectorRangeAngry(Collider collider)
    {

        //激高中はスピードアップし、追跡範囲も広くなる
        if (status.IsAngry)
        {
            agent.speed = status.EnemyData.Speed * 2f;
            agent.isStopped = false;
            agent.destination = collider.transform.position;
            animator.SetFloat("MoveSpeed", agent.speed);
        }
        else if (status.IsAttack || status.IsDie)
        {
            agent.isStopped = true;
            animator.SetFloat("MoveSpeed", 0);
        }
    }

    public void OutPlayerDetectRange(Collider collider)
    {
        //激高中は通常の追跡範囲を越えても追ってくる
        if (status.IsAngry) return;
        status.ChangeStatusToMove();
        agent.ResetPath();
    }

    public void OutPlayerDetectRangeAngry(Collider collider)
    {
        status.ChangeStatusToMove();
        agent.ResetPath();
        agent.speed = status.EnemyData.Speed;
    }
}
