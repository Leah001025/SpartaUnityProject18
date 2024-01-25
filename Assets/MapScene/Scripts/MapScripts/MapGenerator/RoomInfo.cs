using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RoomType
{
    START_ROOM, MONSTER_ROOM, BOSS_ROOM
}

public class RoomInfo
{
    private GameObject prefabsObject;

    private GameObject RoomSquare;//방 입장 체크를 위한 사각형

    //public GameObject TopDoor;
    //public GameObject BottomDoor;
    //public GameObject LeftDoor;
    //public GameObject RightDoor;

    public BoundsInt room;//방 전체
    public Vector2Int center;// 방 중앙 위치
    public RoomType roomType;// 방 타입

    public bool isClear = false;//방을 클리어 했는지 확인
    public bool isFirst = false;//해당 방에 처음 입장하는지 확인

    private int width = 36;// 방 넓이
    private int height = 26;//방 높이

    public Vector2Int leftDoorDestination;//왼쪽 문으로 들어갔을 때, 나오는 위치
    public Vector2Int rightDoorDestination;//오른쪽 문으로 들어갔을 때, 나오는 위치
    public Vector2Int topDoorDestination;//위쪽 문으로 들어갔을 때, 나오는 위치
    public Vector2Int downDoorDestination;//아래쪽 문으로 들어갔을 때, 나오는 위치

    public Vector2Int leftDoor;//왼쪽 문 위치
    public Vector2Int rightDoor;//오른쪽 문 위치
    public Vector2Int topDoor;//위쪽 문 위치
    public Vector2Int downDoor;//아래쪽 문 위치

    public Vector2Int leftSpawnPoint;//왼쪽에서 나오는 위치
    public Vector2Int rightSpawnPoint;//오른쪽에서 나오는 위치
    public Vector2Int topSpawnPoint;//위쪽에서 나오는 위치
    public Vector2Int downSpawnPoint;//아래쪽에서 나오는 위치

    public RoomInfo(Vector2Int center)
    {
        this.center = center;
        room = CreateRooms(this.center, width, height);

        leftDoor = this.center - new Vector2Int((width / 2) + 1, 0);
        rightDoor = this.center + new Vector2Int((width / 2) + 1, 0);
        topDoor = this.center + new Vector2Int(0, (height / 2) + 1);
        downDoor = this.center - new Vector2Int(0, (height / 2) + 1);

        leftSpawnPoint = this.center - new Vector2Int((width / 2) - 1, 0);
        rightSpawnPoint = this.center + new Vector2Int((width / 2) - 1, 0);
        topSpawnPoint = this.center + new Vector2Int(0, (height / 2) - 1);
        downSpawnPoint = this.center - new Vector2Int(0, (height / 2) - 1);
    }

    private BoundsInt CreateRooms(Vector2Int center, int width, int height)//방 생성
    {
        BoundsInt box = new BoundsInt(new Vector3Int(center.x - (width / 2), center.y - (height / 2), 0), new Vector3Int(width, height, 0));//방 만들기

        return box;
    }

    public BoundsInt Room
    {
        get { return room; }
    }

    public RoomType RoomType
    {
        get { return roomType; }
        set { roomType = value; }
    }

    public bool IsClear
    {
        get { return isClear; }
    }

    public void RoomClear()
    {
        isClear = true;
    }

    public GameObject PrefabsObject
    {
        get { return prefabsObject; }
        set { prefabsObject = value; }
    }

    public void SetObjectPosition()
    {
        prefabsObject.transform.position = (Vector3Int)center;
    }

    //이동 위치 반환
    public Vector2Int GetSpawnPoint(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            return topSpawnPoint;
        }
        if (direction == Vector2Int.down)
        {
            return downSpawnPoint;
        }
        if (direction == Vector2Int.left)
        {
            return leftSpawnPoint;
        }
        else
        {
            return rightSpawnPoint;
        }

    }

    //문 활성화
    public void OpenDoor(Vector2Int direction, Vector2Int destination)//각각의 문의 목적지 설정
    {
        if (direction == Vector2Int.up)
        {
            topDoorDestination = destination;
            //topDoor.SetActive(true);
        }
        if (direction == Vector2Int.down)
        {
            downDoorDestination = destination;
            //bottomDoor.SetActive(true);
        }
        if (direction == Vector2Int.left)
        {
            leftDoorDestination = destination;
            //leftDoor.SetActive(true);
        }
        if (direction == Vector2Int.right)
        {
            rightDoorDestination = destination;
            //rightDoor.SetActive(true);
        }
    }
}
