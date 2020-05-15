using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShortRangeAttackDetecotr : MonoBehaviour
{
    private EnemyStatus status;
    [SerializeField, Range(0, 1)] private float attack_JabProbability;
    [SerializeField, Range(0, 1)] private float attack_SpinKickProbability;
    //使用しないが、設定値ミスを防ぐため
    [SerializeField, Range(0, 1)] private float attack_RisingPProbability;
    private BossShortRangeAttack_Jab attack_Jab;
    private BossShortRangeAttack_SpinKick attack_SpinKick;
    private BossShortRangeAttack_RisingP attack_RisingP;


    void Start()
    {
        status = GetComponent<EnemyStatus>();
        attack_Jab = GetComponent<BossShortRangeAttack_Jab>();
        attack_SpinKick = GetComponent<BossShortRangeAttack_SpinKick>();
        attack_RisingP = GetComponent<BossShortRangeAttack_RisingP>();
        attack_SpinKickProbability = attack_JabProbability + attack_SpinKickProbability;
    }

    public void OnShortRangeAttackDetector(Collider collider)
    {
        status.ChangeRangeToShort();

        if (status.AttackRange == EnemyStatus.AttackRangeEnum.Short && status.IsChase || status.IsAngry)
        {
            var probability = Random.Range(0, 1f);
            if (probability < attack_JabProbability)
            {
                attack_Jab.AttackStart();
            }
            else if(attack_JabProbability <= probability && probability < attack_SpinKickProbability)
            {
                attack_SpinKick.AttackStart();
            }
            else
            {
                attack_RisingP.AttackStart();
            }
        }
    }
    public void InShortRange(Collider collider)
    {
        status.ChangeRangeToShort();
    }

    public void OutShortRange(Collider collider)
    {
        status.ChangeRangeToNone();
    }
}
