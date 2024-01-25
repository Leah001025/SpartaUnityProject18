using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerateManager : MonoBehaviour
{
    [SerializeField] private GameObject prefabsMap;

    public static RoomGenerateManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void GenerateRoom()
    {
        foreach(RoomInfo room in RoomManager.DungeonRooms)
        {
            room.PrefabsObject = Instantiate(prefabsMap);
            room.PrefabsObject.transform.SetParent(GameObject.Find("Rooms").transform);
            room.SetObjectPosition();
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
