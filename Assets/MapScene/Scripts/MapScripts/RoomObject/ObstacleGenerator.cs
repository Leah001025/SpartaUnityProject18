using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField] private int minObstacle = 5;
    [SerializeField] private int maxObstacle = 10;

    [SerializeField] private GameObject PrefabObstacle;

    [SerializeField] private List<Sprite> obstacleImages;

    public static ObstacleGenerator instance;

    private void Awake()
    {
        instance = this;
    }

    public void SetObstacle()
    {
        foreach (RoomInfo room in RoomList.DungeonRooms)
        {
            for(int i = 0; i < Random.Range(minObstacle, maxObstacle); i++)
            {
                if (room.RoomType != RoomType.START_ROOM)
                {
                    GameObject obstacle = Instantiate(PrefabObstacle);
                    obstacle.transform.position = new Vector3(Random.Range(room.MinWidth + 2, room.MaxWidth - 2), Random.Range(room.MinHeight + 2, room.MaxHeight - 2), 0);
                    obstacle.GetComponent<SpriteRenderer>().sprite = obstacleImages[Random.Range(0, obstacleImages.Count)];
                    obstacle.transform.SetParent(GameObject.Find("Obstacles").transform);
                }
            }
        }
    }

    public void ClearObstacles()
    {
        foreach (Transform room in GameObject.Find("Obstacles").transform)
        {
            Destroy(room.gameObject);
        }
    }
}
