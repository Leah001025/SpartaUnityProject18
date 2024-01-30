using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerateManager : MonoBehaviour//각각의 방의 클래스를 설정하기 위한 클래스
{
    [SerializeField] private StageInfoSO stageinfo;//스테이지 정보가 들어갑니다.(몬스터 리스트, 보스 등)

    [SerializeField] private GameObject prefabsMap;//room 게임 오브젝트가 들어갑니다.

    [SerializeField] private List<GameObject> roomSettingList;//방 모양 리스트
    [SerializeField] private GameObject itemRoomSetting;

    public static RoomGenerateManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void SetRoom()//방의 유형 선택
    {
        RoomList.DungeonRooms[0].RoomType = RoomType.START_ROOM;//첫번째 방은 스타트지점(몬스터가 안나오게)
        RoomList.DungeonRooms[RoomList.DungeonRooms.Count - 1].RoomType = RoomType.BOSS_ROOM;//마지막 방이 보스방
        RoomList.DungeonRooms[Random.Range(1, RoomList.DungeonRooms.Count - 2)].RoomType = RoomType.ITEM_ROOM;

        foreach (RoomInfo room in RoomList.DungeonRooms)
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

            if(room.RoomType == RoomType.MONSTER_ROOM)//몬스터방 일 때
            {
                SettingMonsterRoom(room.PrefabsObject);
            }
            else if(room.RoomType == RoomType.BOSS_ROOM)//보스방 일 때
            {
                SettingBossRoom(room.PrefabsObject);
            }
            else if(room.RoomType == RoomType.ITEM_ROOM)
            {
                SettingItemRoom(room.PrefabsObject);
            }

            room.SetObject();
        }

        BattleManager.instance.MonsterCountList.Clear();//몬스터 수 초기화 
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
            GameObject monster = Instantiate(stageinfo.monsterList[Random.Range(0, stageinfo.monsterList.Count)], spawnPoint.position, Quaternion.identity);//몬스터 객체 생성
            monster.transform.SetParent(parents.transform.Find("MonsterCount"));//해당 방에 소환된 몬스터의 수를 세기 위해 부모 객체를 옮김
            monster.SetActive(false);//모든 몬스터들 비활성화(혹시 방 너머에서 플레이어를 감지하여 플레이어를 따라오는것을 방지하기 위함)
        }
    }

    public void SettingBossRoom(GameObject parents)//보스룸 세팅
    {
        GameObject boss_room = Instantiate(stageinfo.boss_Room);
        boss_room.transform.SetParent(parents.transform);//부모 객체 설정

        Transform spawnPointParent = boss_room.transform.Find("BossSpawnPoint");//보스 스폰 포인트 설정
        List<Transform> bossSpawnPoint = new List<Transform>();//스폰 포인트 리스트

        for (int i = 0; i < spawnPointParent.childCount; i++)
        {
            bossSpawnPoint.Add(spawnPointParent.GetChild(i));//스폰포인트 갯수만큼 위치를 가져와서 리스트에 저장
        }

        int index = 0;
        foreach (Transform spawnPoint in bossSpawnPoint)//보스 소환
        {
            GameObject boss = Instantiate(stageinfo.bossList[index++], spawnPoint.position, Quaternion.identity);//보스 객체 생성
            BattleManager.instance.BossCount.Add(boss);
            boss.transform.SetParent(parents.transform.Find("MonsterCount"));//해당 방에 소환된 몬스터의 수를 세기 위해 부모 객체를 옮김
            boss.SetActive(false);//모든 몬스터들 비활성화(혹시 방 너머에서 플레이어를 감지하여 플레이어를 따라오는것을 방지하기 위함)
        }

        //------------------------------------------------------------
        //보스룸에서 잡몹이 등장 할 경우 상정
        spawnPointParent = boss_room.transform.Find("MonsterSpawnPoint");//몬스터 스폰 포인트 설정
        List<Transform> monsterSpawnPoint = new List<Transform>();//스폰 포인트 리스트
        if (spawnPointParent.childCount > 0)
        {
            for (int i = 0; i < spawnPointParent.childCount; i++)//잡몹이 있을 때
            {
                monsterSpawnPoint.Add(spawnPointParent.GetChild(i));//스폰포인트 갯수만큼 위치를 가져와서 리스트에 저장
            }

            foreach (Transform spawnPoint in monsterSpawnPoint)//몬스터 소환
            {
                //여기에 몬스터를 Instantiate하면 됩니다.
                GameObject monster = Instantiate(stageinfo.monsterList[Random.Range(0, stageinfo.monsterList.Count)], spawnPoint.position, Quaternion.identity);//몬스터 객체 생성
                monster.transform.SetParent(parents.transform.Find("MonsterCount"));//해당 방에 소환된 몬스터의 수를 세기 위해 부모 객체를 옮김
                monster.SetActive(false);//모든 몬스터들 비활성화(혹시 방 너머에서 플레이어를 감지하여 플레이어를 따라오는것을 방지하기 위함)
            }
        }
    }

    public void SettingItemRoom(GameObject parents)
    {
        GameObject item_room = Instantiate(itemRoomSetting);
        item_room.transform.SetParent(parents.transform);//부모 객체 설정
    }

    public void ClearRooms()//모든 Room오브젝트 삭제
    {
        foreach(Transform room in GameObject.Find("Rooms").transform)
        {
            Destroy(room.gameObject);
        }
    }

    public string GetStageName()
    {
        return stageinfo.stageName;
    }
}
