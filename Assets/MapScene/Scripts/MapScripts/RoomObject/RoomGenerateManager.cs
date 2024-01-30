using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerateManager : MonoBehaviour//������ ���� Ŭ������ �����ϱ� ���� Ŭ����
{
    [SerializeField] private StageInfoSO stageinfo;//�������� ������ ���ϴ�.(���� ����Ʈ, ���� ��)

    [SerializeField] private GameObject prefabsMap;//room ���� ������Ʈ�� ���ϴ�.

    [SerializeField] private List<GameObject> roomSettingList;//�� ��� ����Ʈ
    [SerializeField] private GameObject itemRoomSetting;

    public static RoomGenerateManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void SetRoom()//���� ���� ����
    {
        RoomList.DungeonRooms[0].RoomType = RoomType.START_ROOM;//ù��° ���� ��ŸƮ����(���Ͱ� �ȳ�����)
        RoomList.DungeonRooms[RoomList.DungeonRooms.Count - 1].RoomType = RoomType.BOSS_ROOM;//������ ���� ������
        RoomList.DungeonRooms[Random.Range(1, RoomList.DungeonRooms.Count - 2)].RoomType = RoomType.ITEM_ROOM;

        foreach (RoomInfo room in RoomList.DungeonRooms)
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

            if(room.RoomType == RoomType.MONSTER_ROOM)//���͹� �� ��
            {
                SettingMonsterRoom(room.PrefabsObject);
            }
            else if(room.RoomType == RoomType.BOSS_ROOM)//������ �� ��
            {
                SettingBossRoom(room.PrefabsObject);
            }
            else if(room.RoomType == RoomType.ITEM_ROOM)
            {
                SettingItemRoom(room.PrefabsObject);
            }

            room.SetObject();
        }

        BattleManager.instance.MonsterCountList.Clear();//���� �� �ʱ�ȭ 
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
            GameObject monster = Instantiate(stageinfo.monsterList[Random.Range(0, stageinfo.monsterList.Count)], spawnPoint.position, Quaternion.identity);//���� ��ü ����
            monster.transform.SetParent(parents.transform.Find("MonsterCount"));//�ش� �濡 ��ȯ�� ������ ���� ���� ���� �θ� ��ü�� �ű�
            monster.SetActive(false);//��� ���͵� ��Ȱ��ȭ(Ȥ�� �� �ʸӿ��� �÷��̾ �����Ͽ� �÷��̾ ������°��� �����ϱ� ����)
        }
    }

    public void SettingBossRoom(GameObject parents)//������ ����
    {
        GameObject boss_room = Instantiate(stageinfo.boss_Room);
        boss_room.transform.SetParent(parents.transform);//�θ� ��ü ����

        Transform spawnPointParent = boss_room.transform.Find("BossSpawnPoint");//���� ���� ����Ʈ ����
        List<Transform> bossSpawnPoint = new List<Transform>();//���� ����Ʈ ����Ʈ

        for (int i = 0; i < spawnPointParent.childCount; i++)
        {
            bossSpawnPoint.Add(spawnPointParent.GetChild(i));//��������Ʈ ������ŭ ��ġ�� �����ͼ� ����Ʈ�� ����
        }

        int index = 0;
        foreach (Transform spawnPoint in bossSpawnPoint)//���� ��ȯ
        {
            GameObject boss = Instantiate(stageinfo.bossList[index++], spawnPoint.position, Quaternion.identity);//���� ��ü ����
            BattleManager.instance.BossCount.Add(boss);
            boss.transform.SetParent(parents.transform.Find("MonsterCount"));//�ش� �濡 ��ȯ�� ������ ���� ���� ���� �θ� ��ü�� �ű�
            boss.SetActive(false);//��� ���͵� ��Ȱ��ȭ(Ȥ�� �� �ʸӿ��� �÷��̾ �����Ͽ� �÷��̾ ������°��� �����ϱ� ����)
        }

        //------------------------------------------------------------
        //�����뿡�� ����� ���� �� ��� ����
        spawnPointParent = boss_room.transform.Find("MonsterSpawnPoint");//���� ���� ����Ʈ ����
        List<Transform> monsterSpawnPoint = new List<Transform>();//���� ����Ʈ ����Ʈ
        if (spawnPointParent.childCount > 0)
        {
            for (int i = 0; i < spawnPointParent.childCount; i++)//����� ���� ��
            {
                monsterSpawnPoint.Add(spawnPointParent.GetChild(i));//��������Ʈ ������ŭ ��ġ�� �����ͼ� ����Ʈ�� ����
            }

            foreach (Transform spawnPoint in monsterSpawnPoint)//���� ��ȯ
            {
                //���⿡ ���͸� Instantiate�ϸ� �˴ϴ�.
                GameObject monster = Instantiate(stageinfo.monsterList[Random.Range(0, stageinfo.monsterList.Count)], spawnPoint.position, Quaternion.identity);//���� ��ü ����
                monster.transform.SetParent(parents.transform.Find("MonsterCount"));//�ش� �濡 ��ȯ�� ������ ���� ���� ���� �θ� ��ü�� �ű�
                monster.SetActive(false);//��� ���͵� ��Ȱ��ȭ(Ȥ�� �� �ʸӿ��� �÷��̾ �����Ͽ� �÷��̾ ������°��� �����ϱ� ����)
            }
        }
    }

    public void SettingItemRoom(GameObject parents)
    {
        GameObject item_room = Instantiate(itemRoomSetting);
        item_room.transform.SetParent(parents.transform);//�θ� ��ü ����

        Transform spawnPointParent = item_room.transform.Find("ItemSpawnPoint");//���� ���� ����Ʈ ����
        List<Transform> itemSpawnPoint = new List<Transform>();//���� ����Ʈ ����Ʈ

        for (int i = 0; i < spawnPointParent.childCount; i++)
        {
            itemSpawnPoint.Add(spawnPointParent.GetChild(i));//��������Ʈ ������ŭ ��ġ�� �����ͼ� ����Ʈ�� ����
        }

        foreach(Transform spawnPoint in itemSpawnPoint)//���� ��ȯ
        {
            //���⿡ ���͸� Instantiate�ϸ� �˴ϴ�.
            GameObject item = Instantiate(stageinfo.itemList[Random.Range(0, stageinfo.itemList.Count)], spawnPoint.position, Quaternion.identity);//���� ��ü ����
            item.transform.SetParent(parents.transform.Find("ItemCount"));//�ش� �濡 ��ȯ�� ������ ���� ���� ���� �θ� ��ü�� �ű�
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
