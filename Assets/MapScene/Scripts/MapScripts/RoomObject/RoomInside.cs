using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInside : MonoBehaviour
{
    public bool isClear = false;

    [SerializeField] GameObject MonsterCount;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isClear)
            {
                BattleManager.instance.nowBattle = false;
            }
            else
            {
                BattleManager.instance.nowBattle = true;
            }
        }
    }

    private void Update()
    {
        if(MonsterCount.transform.childCount == 0)
        {
            isClear = true;
        }
    }
}
