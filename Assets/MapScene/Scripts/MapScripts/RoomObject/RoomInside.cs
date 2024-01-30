using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RoomInside : MonoBehaviour
{
    public bool isClear = false;

    [SerializeField] private GameObject MonsterCount;
    [SerializeField] private GameObject MiniMap;

    private void OnTriggerStay2D(Collider2D collision)//�÷��̾ �濡 �ִ� ����
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

    private void OnTriggerEnter2D(Collider2D collision)//�÷��̾ �濡 �� �������� ��
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isClear)
            {
                for (int i = 0; i < MonsterCount.transform.childCount; i++)
                {
                    MonsterCount.transform.GetChild(i).gameObject.SetActive(true);//��� ���͸� Ȱ��ȭ
                }
            }

            SetMiniMap();
            SetCameraPosition();
        }
    }

    private void SetCameraPosition()
    {
        CameraManager.instance.center = transform.position;//���� ī�޶� ����
        CameraManager.instance.mapSize = new Vector2(transform.localScale.x / 2, transform.localScale.y / 2);
    }

    private void SetMiniMap()
    {
        CameraManager.instance.minimapCamera.transform.position = new Vector3(MiniMap.transform.position.x, MiniMap.transform.position.y, CameraManager.instance.minimapCamera.transform.position.z);//�̴ϸ� ī�޶� ����

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
        if (MonsterCount.transform.childCount == 0)//���͸� ��� óġ�ߴٸ�. 
        {
            isClear = true;//�� Ŭ���� ó��
        }
    }
}
