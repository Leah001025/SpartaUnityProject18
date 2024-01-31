using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerateManager : MonoBehaviour//각 방의 오브젝트를 세팅하는 스크립트 
{
    [SerializeField] private StageInfoSO stageinfo;//스테이지 정보(방 갯수 등)

    [SerializeField] private GameObject prefabsMap;//room 오브젝트 prefabs

    [SerializeField] private List<GameObject> roomSettingList;//방의 장애물과 몬스터 스폰 위치 설정 리스트
    [SerializeField] private GameObject itemRoomSetting;

    public static RoomGenerateManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void SetRoom()//방의 유형 결정
    {
        RoomList.DungeonRooms[0].RoomType = RoomType.START_ROOM;//첫번째로 생성된 방은 시작방
        RoomList.DungeonRooms[RoomList.DungeonRooms.Count - 1].RoomType = RoomType.BOSS_ROOM;//마지막으로 생성된 방은 보스방
        RoomList.DungeonRooms[Random.Range(1, RoomList.DungeonRooms.Count - 2)].RoomType = RoomType.ITEM_ROOM;//시작방과 보스방 제외 무작위 방을 아이템방으로 설정

        foreach (RoomInfo room in RoomList.DungeonRooms)
        {
            if(room.RoomType == RoomType.NoThing)
            {
                room.RoomType = RoomType.MONSTER_ROOM;
            }
        }
    }


    public void GenerateRoom()//방 오브젝트 생성
    {
        foreach(RoomInfo room in RoomList.DungeonRooms)
        {
            room.PrefabsObject = Instantiate(prefabsMap);
            room.PrefabsObject.transform.SetParent(GameObject.Find("Rooms").transform);

            if(room.RoomType == RoomType.MONSTER_ROOM)//몬스터방일 때
            {
                SettingMonsterRoom(room.PrefabsObject);
            }
            else if(room.RoomType == RoomType.BOSS_ROOM)//보스방일 때
            {
                SettingBossRoom(room.PrefabsObject);
            }
            else if(room.RoomType == RoomType.ITEM_ROOM)//아이템방일 때
            {
                SettingItemRoom(room.PrefabsObject);
            }

            room.SetObject();
        }

        BattleManager.instance.MonsterCountList.Clear();//몬스터 수 초기화
    }

    public void SettingMonsterRoom(GameObject parents)//몬스터방 설정
    {
        GameObject monster_room = Instantiate(roomSettingList[Random.Range(0, roomSettingList.Count)]);
        monster_room.transform.SetParent(parents.transform);//prefab 오브젝트를 부모 오브젝트의 하위로

        Transform spawnPointParent = monster_room.transform.Find("MonsterSpawnPoint");//몬스터 스폰 위치 찾기
        List<Transform> monsterSpawnPoint = new List<Transform>();

        for (int i = 0; i < spawnPointParent.childCount; i++)
        {
            monsterSpawnPoint.Add(spawnPointParent.GetChild(i));//몬스터 스폰 위치값들을 가져옴
        }

        foreach(Transform spawnPoint in monsterSpawnPoint)//몬스터 소환
        {
            GameObject monster = Instantiate(stageinfo.monsterList[Random.Range(0, stageinfo.monsterList.Count)], spawnPoint.position, Quaternion.identity);//몬스터 소환
            monster.transform.SetParent(parents.transform.Find("MonsterCount"));//현재 몬스터 수를 세기위해 부모를 MonsterCount로 설정
            monster.SetActive(false);//몬스터가 벽 넘어에서 플레이어를 추격하는 것을 막기 위해 모든 몬스터를 비활성화
        }
    }

    public void SettingBossRoom(GameObject parents)//보스방 설정
    {
        GameObject boss_room = Instantiate(stageinfo.boss_Room);
        boss_room.transform.SetParent(parents.transform);//prefab 오브젝트를 부모 오브젝트의 하위로

        Transform spawnPointParent = boss_room.transform.Find("BossSpawnPoint");//보스 스폰 위치 찾기
        List<Transform> bossSpawnPoint = new List<Transform>();

        for (int i = 0; i < spawnPointParent.childCount; i++)
        {
            bossSpawnPoint.Add(spawnPointParent.GetChild(i));//몬스터 스폰 위치값들을 가져옴
        }

        int index = 0;
        foreach (Transform spawnPoint in bossSpawnPoint)//보스 소환
        {
            GameObject boss = Instantiate(stageinfo.bossList[index++], spawnPoint.position, Quaternion.identity);//보스 소환
            BattleManager.instance.BossCount.Add(boss);
            boss.transform.SetParent(parents.transform.Find("MonsterCount"));//현재 몬스터 수를 세기위해 부모를 MonsterCount로 설정
            boss.SetActive(false);//몬스터가 벽 넘어에서 플레이어를 추격하는 것을 막기 위해 모든 몬스터를 비활성화
        }

        //------------------------------------------------------------
        //잡몹이 같이 소환될 때를 상정
        spawnPointParent = boss_room.transform.Find("MonsterSpawnPoint");//몬스터 스폰 위치 찾기
        List<Transform> monsterSpawnPoint = new List<Transform>();
        if (spawnPointParent.childCount > 0)
        {
            for (int i = 0; i < spawnPointParent.childCount; i++)
            {
                monsterSpawnPoint.Add(spawnPointParent.GetChild(i));//몬스터 스폰 위치값들을 가져옴
            }

            foreach (Transform spawnPoint in monsterSpawnPoint)//몬스터 소환
            {
                GameObject monster = Instantiate(stageinfo.monsterList[Random.Range(0, stageinfo.monsterList.Count)], spawnPoint.position, Quaternion.identity);//몬스터 소환
                monster.transform.SetParent(parents.transform.Find("MonsterCount"));//현재 몬스터 수를 세기위해 부모를 MonsterCount로 설정
                monster.SetActive(false);//몬스터가 벽 넘어에서 플레이어를 추격하는 것을 막기 위해 모든 몬스터를 비활성화
            }
        }
    }

    public void SettingItemRoom(GameObject parents)
    {
        GameObject item_room = Instantiate(itemRoomSetting);
        item_room.transform.SetParent(parents.transform);//prefab 오브젝트를 부모 오브젝트의 하위로

        Transform spawnPointParent = item_room.transform.Find("ItemSpawnPoint");//아이템 스폰 위치 찾기
        List<Transform> itemSpawnPoint = new List<Transform>();

        for (int i = 0; i < spawnPointParent.childCount; i++)
        {
            itemSpawnPoint.Add(spawnPointParent.GetChild(i));//아이템 스폰 위치값들을 가져옴
        }

        foreach(Transform spawnPoint in itemSpawnPoint)//아이템 소환
        {
            GameObject item = Instantiate(stageinfo.itemList[Random.Range(0, stageinfo.itemList.Count)], spawnPoint.position, Quaternion.identity);//아이템 소환
            item.transform.SetParent(parents.transform.Find("ItemCount"));//아이템 오브젝트의 부모 설정
        }
    }

    public void ClearRooms()//모든 Room 오브젝트 초기화
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
