using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    private EnemyStatus status;
    [SerializeField] private DropItem dropItemPrefab;
    private bool isDropped;
    private float dropWait = 1f;


    void Start()
    {
        status = GetComponent<EnemyStatus>();
    }

    private void ItemDrop()
    {
        if (isDropped) return;
        isDropped = true;

        if (status.EnemyData.DropItem1 == null) return;

        //敵に第一ドロップが設定されている場合
        var dropItem1_Probability = Random.Range(0, 1f);
        if(dropItem1_Probability <= status.EnemyData.DropItem1_Probability)
        {
            var dropItem1 = Instantiate(dropItemPrefab, transform.position + new Vector3(Random.Range(0, 1f), 0, Random.Range(0, 1f)), Quaternion.identity);
            dropItem1.SetItem(status.EnemyData.DropItem1, status.EnemyData.DropItem1_Number);
        }

        if (status.EnemyData.DropItem2 == null) return;

        //敵に第二ドロップが設定されている場合
        var dropItem2_Probability = Random.Range(0, 1f);
        if (dropItem2_Probability <= status.EnemyData.DropItem2_Probability)
        {
            var dropItem2 = Instantiate(dropItemPrefab, transform.position + new Vector3(Random.Range(0, 1f), 0, Random.Range(0, 1f)), Quaternion.identity);
            dropItem2.SetItem(status.EnemyData.DropItem2, status.EnemyData.DropItem2_Number);
        }
    }


    void Update()
    {
        if(status.IsDie)
        {
            Invoke("ItemDrop", dropWait);
        }
    }
}
