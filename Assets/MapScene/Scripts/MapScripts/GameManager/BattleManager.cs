using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public bool nowBattle = false;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
}
