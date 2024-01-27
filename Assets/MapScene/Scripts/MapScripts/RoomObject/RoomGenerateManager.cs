using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerateManager : MonoBehaviour//������ ���� Ŭ������ �����ϱ� ���� Ŭ����
{
    [SerializeField] private GameObject prefabsMap;//room ���� ������Ʈ�� ���ϴ�.

    [SerializeField] private List<GameObject> roomSettingList;//�� ��� ����Ʈ
    [SerializeField] private List<GameObject> TempMonsters;//���� ��ȯ �����

    public static RoomGenerateManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void SetRoom()//���� ���� ����
    {
        RoomList.DungeonRooms[0].RoomType = RoomType.START_ROOM;//ù��° ���� ��ŸƮ����(���Ͱ� �ȳ�����)
        RoomInfo farRoom = FindFarestPointTo(RoomList.DungeonRooms[0], RoomList.DungeonRooms);//��ŸƮ �������� ���� �� ���� ������
        farRoom.roomType = RoomType.BOSS_ROOM;

        foreach(RoomInfo room in RoomList.DungeonRooms)
        {
            if(room.RoomType == RoomType.NoThing)
            {
                room.RoomType = RoomType.MONSTER_ROOM;
            }
        }
    }

    private RoomInfo FindFarestPointTo(RoomInfo startRoom, List<RoomInfo> rooms)//���� �� �� ��ġ ã��
    {
        RoomInfo closestRoom = null;
        float distance = 0;

        foreach (var room in rooms)
        {
            float currentDistance = Vector2.Distance(room.center, startRoom.center);
            if (currentDistance > distance)
            {
                distance = currentDistance;
                closestRoom = room;
            }
        }

        return closestRoom;
    }

    public void GenerateRoom()//�� ������Ʈ ����
    {
        foreach(RoomInfo room in RoomList.DungeonRooms)
        {
            room.PrefabsObject = Instantiate(prefabsMap);
            room.PrefabsObject.transform.SetParent(GameObject.Find("Rooms").transform);

            if(room.RoomType == RoomType.MONSTER_ROOM)
            {
                SettingMonsterRoom(room.PrefabsObject);
            }

            room.SetObject();
        }
    }

    public void SettingMonsterRoom(GameObject parents)//���ͷ� ����
    {
        GameObject monster_room = Instantiate(roomSettingList[Random.Range(0, roomSettingList.Count)]);
        monster_room.transform.SetParent(parents.transform);//�θ� ��ü ����

        Transform spawnPointParent = monster_room.transform.Find("MonsterSpawnPoint");//���� ���� ����Ʈ ����
        List<Transform> monsterSpawnPoint = new List<Transform>();//���� ����Ʈ ����Ʈ

        for (int i = 0; i < spawnPointParent.childCount; i++)
        {
            monsterSpawnPoint.Add(spawnPointParent.GetChild(i));//��������Ʈ ������ŭ ��ġ�� �����ͼ� ����Ʈ�� ����
        }

        foreach(Transform spawnPoint in monsterSpawnPoint)//���� ��ȯ
        {
            //���⿡ ���͸� Instantiate�ϸ� �˴ϴ�.
            GameObject monster = Instantiate(TempMonsters[Random.Range(0, TempMonsters.Count)], spawnPoint.position, Quaternion.identity);//GameObject�� ���ͷ� �ٲ��ּ���
            monster.transform.SetParent(parents.transform.Find("MonsterCount"));//�ش� �濡 ��ȯ�� ������ ���� ���� ���� �θ� ��ü�� �ű�
        }
    }


    public void ClearRooms()//��� Room������Ʈ ����
    {
        foreach(Transform room in GameObject.Find("Rooms").transform)
        {
            Destroy(room.gameObject);
        }
    }
}
