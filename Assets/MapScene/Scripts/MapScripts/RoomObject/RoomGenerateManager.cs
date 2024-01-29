using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerateManager : MonoBehaviour//������ ���� Ŭ������ �����ϱ� ���� Ŭ����
{
    [SerializeField] private GameObject prefabsMap;

    [SerializeField] private List<RoomInfo> roomList;

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

    private RoomInfo FindFarestPointTo(RoomInfo startRoom, List<RoomInfo> rooms)//���� �� ��ġ ã��
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

    public void GenerateRoom()
    {
        foreach(RoomInfo room in RoomList.DungeonRooms)
        {
            room.PrefabsObject = Instantiate(prefabsMap);
            room.PrefabsObject.transform.SetParent(GameObject.Find("Rooms").transform);
            room.SetObject();
        }
    }

    public void ClearRooms()
    {
        foreach(Transform room in GameObject.Find("Rooms").transform)
        {
            Destroy(room.gameObject);
        }
    }
}
