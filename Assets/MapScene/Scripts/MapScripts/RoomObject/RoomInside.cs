using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RoomInside : MonoBehaviour
{
    public bool isClear = false;

    [SerializeField] private GameObject MonsterCount;
    [SerializeField] private GameObject MiniMap;

    private void OnTriggerStay2D(Collider2D collision)//플레이어가 방에 있는 동안
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isClear || Managers.Game.useWatch)
            {
                BattleManager.instance.nowBattle = false;
            }
            else
            {
                BattleManager.instance.nowBattle = true;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)//플레이어가 방에 막 입장했을 때
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isClear)
            {
                for (int i = 0; i < MonsterCount.transform.childCount; i++)
                {
                    MonsterCount.transform.GetChild(i).gameObject.SetActive(true);//모든 몬스터를 활성화
                }
            }

            SetMiniMap();
            SetCameraPosition();
        }
    }

    private void SetCameraPosition()
    {
        CameraManager.instance.center = transform.position;//메인 카메라 설정
        CameraManager.instance.mapSize = new Vector2(transform.localScale.x / 2, transform.localScale.y / 2);
    }

    private void SetMiniMap()
    {
        CameraManager.instance.minimapCamera.transform.position = new Vector3(MiniMap.transform.position.x, MiniMap.transform.position.y, CameraManager.instance.minimapCamera.transform.position.z);//미니맵 카메라 설정

        Color color = MiniMap.GetComponent<SpriteRenderer>().color;
        color.a = 1f;
        MiniMap.GetComponent<SpriteRenderer>().color = color;
    }

    private void Start()
    {
        BattleManager.instance.MonsterCountList.Add(MonsterCount);
    }

    private void Update()
    {
        if (MonsterCount.transform.childCount == 0)//몬스터를 모두 처치했다면. 
        {
            isClear = true;//방 클리어 처리
        }
    }
}
