using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerateManager : MonoBehaviour//각각의 방의 클래스를 설정하기 위한 클래스
{
    [SerializeField] private GameObject prefabsMap;//room 게임 오브젝트가 들어갑니다.

    [SerializeField] private List<GameObject> roomSettingList;//방 모양 리스트
    [SerializeField] private List<GameObject> TempMonsters;//몬스터 소환 실험용

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

    private RoomInfo FindFarestPointTo(RoomInfo startRoom, List<RoomInfo> rooms)//가장 먼 방 위치 찾기
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

    public void GenerateRoom()//방 오브젝트 생성
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

    public void SettingMonsterRoom(GameObject parents)//몬스터룸 세팅
    {
        GameObject monster_room = Instantiate(roomSettingList[Random.Range(0, roomSettingList.Count)]);
        monster_room.transform.SetParent(parents.transform);//부모 객체 설정

        Transform spawnPointParent = monster_room.transform.Find("MonsterSpawnPoint");//몬스터 스폰 포인트 설정
        List<Transform> monsterSpawnPoint = new List<Transform>();//스폰 포인트 리스트

        for (int i = 0; i < spawnPointParent.childCount; i++)
        {
            monsterSpawnPoint.Add(spawnPointParent.GetChild(i));//스폰포인트 갯수만큼 위치를 가져와서 리스트에 저장
        }

        foreach(Transform spawnPoint in monsterSpawnPoint)//몬스터 소환
        {
            //여기에 몬스터를 Instantiate하면 됩니다.
            GameObject monster = Instantiate(TempMonsters[Random.Range(0, TempMonsters.Count)], spawnPoint.position, Quaternion.identity);//GameObject를 몬스터로 바꿔주세요
            monster.transform.SetParent(parents.transform.Find("MonsterCount"));//해당 방에 소환된 몬스터의 수를 세기 위해 부모 객체를 옮김
        }
    }


    public void ClearRooms()//모든 Room오브젝트 삭제
    {
        foreach(Transform room in GameObject.Find("Rooms").transform)
        {
            Destroy(room.gameObject);
        }
    }
}
