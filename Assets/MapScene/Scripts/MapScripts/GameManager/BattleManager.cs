using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public bool nowBattle = false;
    public bool bossDead = false;

    public List<GameObject> BossCount;
    public List<GameObject> MonsterCountList;

    public int currentMonsterCount;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentMonsterCount = 0;

        RandomWalkGenerator.Instance.GenerateDungeon();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (nowBattle)
        {
            foreach(RoomInfo room in RoomList.DungeonRooms)
            {
                room.CloseDoor();
            }
        }
        else
        {
            foreach (RoomInfo room in RoomList.DungeonRooms)
            {
                room.OpenDoor();
            }
        }
    }

    private void Update()
    {
        UpdateCount();

        if (BossCount.Count == 0)
        {
            bossDead = true;
        }
        else
        {
            foreach (GameObject boss in BossCount)
            {
                if (boss == null)
                {
                    BossCount.Remove(boss);
                }
            }
        }
    }

    private void UpdateCount()
    {
        currentMonsterCount = 0;
        foreach(GameObject count in MonsterCountList)
        {
            currentMonsterCount += count.transform.childCount;
        }
    }
}
