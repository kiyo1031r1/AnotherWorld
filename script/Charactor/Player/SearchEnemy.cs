using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchEnemy : MonoBehaviour
{
    //敵撃破時に使用
    public static SearchEnemy Instance => _instance;
    private static SearchEnemy _instance;
    private List<GameObject> enemyList = new List<GameObject>();

    private GameObject currentTarget;
    private Transform _transform;
    private bool isRock;
    private int targetNumber = 0;
    private float viewEuler;
    private Text lockOnText;

    void Start()
    {
        _instance = this;
        viewEuler = GetComponent<PlayerStatus>().ViewEuler;
        _transform = transform;

        var lockOnTextObj = GameObject.Find("LockOnText");
        if(lockOnTextObj)
        {
            lockOnText = lockOnTextObj.GetComponent<Text>();
            lockOnText.text = "";
        }
    }

    void Update()
    {
        if(currentTarget != null)
        {
            var targetDirection = currentTarget.transform.position - _transform.position;
            var targetRotation = Quaternion.LookRotation(targetDirection);

            //X軸とZ軸の角度を0にして水平に回転
            targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
            _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, Time.deltaTime * 15f);

            //視点変更完了後に対象をロック
            if (Mathf.Abs(_transform.eulerAngles.y - targetRotation.eulerAngles.y) < 1f)
            {
                isRock = true;
                lockOnText.text = "LOCK ON";
            }
        }

        else if (currentTarget != null && isRock)
        {
            _transform.LookAt(currentTarget.transform.position);
        }
    }

    public void RemoveEnemyList(GameObject gameObject)
    {
        enemyList.Remove(gameObject);

        if (gameObject == currentTarget)
        {
            TargetRelease();
        }
    }

    public void InEnemyDetectorRange(Collider collider)
    {
        var targetDirection = collider.transform.position - _transform.position;
        var targetEuler = Vector3.Angle(_transform.forward, targetDirection);
        if(targetEuler <= viewEuler)
        {
            if (!enemyList.Contains(collider.gameObject))
            {
                enemyList.Add(collider.gameObject);
            }
        }
    }

    public void OutEnemyDetectorRange(Collider collider)
    {
        RemoveEnemyList(collider.gameObject);
    }

    public void TargetLockOn()
    {
        if (enemyList.Count == 0) return;

        SoundManager.Instance.PlaySE(7);
        currentTarget = enemyList[targetNumber];
        targetNumber += 1;

        if(targetNumber == enemyList.Count)
        {
            targetNumber = 0;
        }
    }

    public void TargetRelease()
    {
        if (currentTarget == null) return;
        SoundManager.Instance.PlaySE(10);
        currentTarget = null;
        isRock = false;
        lockOnText.text = "";
        targetNumber = 0;
    }

}
