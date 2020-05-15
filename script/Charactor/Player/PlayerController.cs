using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerStatus))]
[RequireComponent(typeof(SearchCharacter))]

public class PlayerController : MonoBehaviour
{
    protected CharacterController characterController;
    protected Vector3 moveVelocity;
    protected Vector3 desireMove;
    protected Transform _transform;

    [SerializeField] protected float moveSpeed = 4f;
    [SerializeField] protected float moveSpeedBack = 4.5f;
    protected float rotateHorizontal;
    protected float rotateVertical;
    [SerializeField] protected float rotateHorizontalSpeed = 0.4f;

    protected Animator animator;
    protected PlayerStatus status;
    protected SearchCharacter searchCharactor;
    protected Dialog_PlayerMenu dialog_PlayerMenu;

    protected virtual void Start()
    {
        characterController = GetComponent<CharacterController>();
        _transform = base.transform;
        animator = GetComponent<Animator>();
        status = GetComponent<PlayerStatus>();
        searchCharactor = GetComponent<SearchCharacter>();
        dialog_PlayerMenu = GameObject.Find("PlayerMenuCanvas").GetComponent<Dialog_PlayerMenu>();
    }

    protected virtual void Update()
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

    //左アナログスティック
    protected void GetMoveInput()
    {
        moveVelocity.x = Input.GetAxis("Horizontal") * moveSpeed;
        moveVelocity.z = Input.GetAxis("Vertical") * moveSpeed;

        if (moveVelocity.z < 0)
        {
            moveVelocity.z *= moveSpeedBack;
        }
    }

    //右アナログスティック
    protected void UpdateViewPoint()
    {
        rotateHorizontal = Input.GetAxis("HorizontalRight");
        _transform.Rotate(new Vector3(0f, rotateHorizontal * rotateHorizontalSpeed, 0f));
    }
}
