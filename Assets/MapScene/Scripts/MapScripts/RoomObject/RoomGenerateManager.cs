using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerateManager : MonoBehaviour//각각의 방의 클래스를 설정하기 위한 클래스
{
    [SerializeField] private GameObject prefabsMap;

    [SerializeField] private List<RoomInfo> roomList;

    public static RoomGenerateManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void SetRoom()//방의 유형 선택
    {
        RoomList.DungeonRooms[0].RoomType = RoomType.START_ROOM;//첫번째 방은 스타트지점(몬스터가 안나오게)
        RoomInfo farRoom = FindFarestPointTo(RoomList.DungeonRooms[0], RoomList.DungeonRooms);//스타트 지점에서 가장 먼 방은 보스방
        farRoom.roomType = RoomType.BOSS_ROOM;

        foreach(RoomInfo room in RoomList.DungeonRooms)
        {
            if(room.RoomType == RoomType.NoThing)
            {
                room.RoomType = RoomType.MONSTER_ROOM;
            }
        }
    }

    private RoomInfo FindFarestPointTo(RoomInfo startRoom, List<RoomInfo> rooms)//가장 방 위치 찾기
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
