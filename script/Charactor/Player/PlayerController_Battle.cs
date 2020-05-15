using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//街の操作に加え以下が可能
//攻撃・索敵

[RequireComponent(typeof(PlayerNormalAttack))]
[RequireComponent(typeof(SearchEnemy))]

public class PlayerController_Battle : PlayerController
{
    private PlayerNormalAttack normalAttack;
    private SearchEnemy searchEnemy;

    protected override void Start()
    {
        base.Start();
        normalAttack = GetComponent<PlayerNormalAttack>();
        searchEnemy = GetComponent<SearchEnemy>();
        searchCharactor = GetComponent<SearchCharacter>();
    }

    protected override void Update()
    {
        if (status.IsMove)
        {
            UpdateViewPoint();
            GetMoveInput();

            //〇ボタン
            if (Input.GetButtonDown("Fire3"))
            {
                searchCharactor.FindStart();
            }

            //□ボタン
            if (Input.GetButtonDown("Fire1"))
            {
                dialog_PlayerMenu.OpenDialog();
            }

            //×ボタン
            if (Input.GetButtonDown("Fire2"))
            {
                searchEnemy.TargetRelease();
            }

            //R1ボタン
            if (Input.GetButtonDown("Fire5"))
            {
                normalAttack.AttackStart();
            }

            //R2ボタン
            if (Input.GetButtonDown("Fire7"))
            {
                searchEnemy.TargetLockOn();
            }

            desireMove = moveVelocity.x * _transform.right + moveVelocity.z * _transform.forward;
            animator.SetFloat("MoveSpeed", desireMove.sqrMagnitude);
            characterController.Move(desireMove * Time.deltaTime);
        }

        else
        {
            moveVelocity.x = 0;
            moveVelocity.z = 0;
            animator.SetFloat("MoveSpeed", 0);
        }
    }
}
