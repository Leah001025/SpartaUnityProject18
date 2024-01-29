using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public enum RoomType
{
    NoThing, START_ROOM, MONSTER_ROOM, BOSS_ROOM
}

public class RoomInfo
{
    private GameObject prefabsObject;

    private GameObject RoomSquare;//방 입장 체크를 위한 사각형

    public GameObject TopDoorObject;//위쪽 문 오브젝트
    public GameObject BottomDoorObject;//아래 쪽 문 오브젝트
    public GameObject LeftDoorObject;//왼쪽 문 오브젝트
    public GameObject RightDoorObject;//오른쪽 문 오브젝트

    public BoundsInt room;//방 전체
    public Vector2Int center;// 방 중앙 위치
    public RoomType roomType;// 방 타입

    private int width = 36;// 방 넓이
    private int height = 26;//방 높이

    public bool hasDoor = false;//연결된 복도가 있는지 확인

    public bool IsTopDoor = false;
    public bool IsBottomDoor = false;
    public bool IsRightDoor = false;
    public bool IsLeftDoor = false;

    public RoomInfo leftDoorDestination;//왼쪽 문과 연결되 방
    public RoomInfo rightDoorDestination;//오른쪽 문과 연결되 방
    public RoomInfo topDoorDestination;//위쪽 문과 연결되 방
    public RoomInfo bottomDoorDestination;//아래쪽 문과 연결되 방

    public Vector2Int leftDoor;//현재 방의 왼쪽 문 위치
    public Vector2Int rightDoor;//현재 방의 오른쪽 문 위치
    public Vector2Int topDoor;//현재 방의 위쪽 문 위치
    public Vector2Int bottomDoor;//현재 방의 아래쪽 문 위치

    public Vector2Int leftDoorEntrance;//왼쪽 문으로 들어갔을 때, 나오는 위치
    public Vector2Int rightDoorEntrance;//오른쪽 문으로 들어갔을 때, 나오는 위치
    public Vector2Int bottomDoorEntrance;//위쪽 문으로 들어갔을 때, 나오는 위치
    public Vector2Int topDoorEntrance;//아래쪽 문으로 들어갔을 때, 나오는 위치

    public Vector2Int leftSpawnPoint;//왼쪽에서 나오는 위치
    public Vector2Int rightSpawnPoint;//오른쪽에서 나오는 위치
    public Vector2Int topSpawnPoint;//위쪽에서 나오는 위치
    public Vector2Int bottomSpawnPoint;//아래쪽에서 나오는 위치

    public RoomInfo(Vector2Int center)
    {
        this.center = center;
        room = CreateRooms(this.center, width, height);

        roomType = RoomType.NoThing;

        leftDoor = this.center - new Vector2Int((width / 2) + 3, 0);
        rightDoor = this.center + new Vector2Int((width / 2) + 3, 0);
        topDoor = this.center + new Vector2Int(0, (height / 2) + 3);
        bottomDoor = this.center - new Vector2Int(0, (height / 2) + 3);

        leftSpawnPoint = this.center - new Vector2Int((width / 2) - 2, 0);
        rightSpawnPoint = this.center + new Vector2Int((width / 2) - 2, 0);
        topSpawnPoint = this.center + new Vector2Int(0, (height / 2) - 2);
        bottomSpawnPoint = this.center - new Vector2Int(0, (height / 2) - 2);
    }

    private BoundsInt CreateRooms(Vector2Int center, int width, int height)//방 생성
    {
        BoundsInt box = new BoundsInt(new Vector3Int(center.x - (width / 2), center.y - (height / 2), 0), new Vector3Int(width, height, 0));//방 만들기

        return box;
    }

    //-------------------------------------------------------------------------------------------------------------

    public BoundsInt Room
    {
        get { return room; }
    }

    public RoomType RoomType
    {
        get { return roomType; }
        set { roomType = value; }
    }

    public int Width
    {
        get { return width; }
    }

    public int MinWidth
    {
        get { return center.x - (width / 2); }
    }

    public int MaxWidth
    {
        get { return center.x + (width / 2); }
    }

    public int Height
    {
        get { return height; }
    }

    public int MinHeight
    {
        get { return center.y - (height / 2); }
    }

    public int MaxHeight
    {
        get { return center.y + (height / 2); }
    }


    public GameObject PrefabsObject
    {
        get { return prefabsObject; }
        set { prefabsObject = value; }
    }

    //-------------------------------------------------------------------------------------------------------------

    public void SetObject()//게임 오브젝트 설정
    {
        prefabsObject.transform.position = (Vector3Int)center;
        RoomSquare = prefabsObject.transform.Find("RoomInside").gameObject;

        switch (roomType)
        {
            case RoomType.BOSS_ROOM:
                RoomSquare.GetComponent<SpriteRenderer>().color = new Color(200 / 255f, 50 / 255f, 50 / 255f, 100 / 255f);
                break;
            case RoomType.START_ROOM:
                RoomSquare.GetComponent<RoomInside>().isClear = true;
                RoomSquare.GetComponent<SpriteRenderer>().color = new Color(50 / 255f, 50 / 255f, 200 / 255f, 100 / 255f);
                break;
            case RoomType.MONSTER_ROOM:
                RoomSquare.GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 100 / 255f);
                break;
        }

        TopDoorObject = prefabsObject.transform.Find("TopDoor").gameObject;

        BottomDoorObject = prefabsObject.transform.Find("BottomDoor").gameObject;

        LeftDoorObject = prefabsObject.transform.Find("LeftDoor").gameObject;

        RightDoorObject = prefabsObject.transform.Find("RightDoor").gameObject;

        ActiveDoor();
        OpenDoor();
    }

    private void ActiveDoor()//문 생성
    {
        if (IsTopDoor)
        {
            TopDoorObject.SetActive(true);
            TopDoorObject.transform.Find("OpenedDoor").GetComponent<Telepoter>().TelepotePosition = topDoorEntrance;
        }
        if (IsBottomDoor)
        {
            BottomDoorObject.SetActive(true);
            BottomDoorObject.transform.Find("OpenedDoor").GetComponent<Telepoter>().TelepotePosition = bottomDoorEntrance;
        }
        if (IsLeftDoor)
        {
            LeftDoorObject.SetActive(true);
            LeftDoorObject.transform.Find("OpenedDoor").GetComponent<Telepoter>().TelepotePosition = leftDoorEntrance;
        }
        if (IsRightDoor)
        {
            RightDoorObject.SetActive(true);
            RightDoorObject.transform.Find("OpenedDoor").GetComponent<Telepoter>().TelepotePosition = rightDoorEntrance;
        }

        //보스룸 문 다르게
        if(topDoorDestination?.roomType == RoomType.BOSS_ROOM)
        {
            TopDoorObject.transform.Find("OpenedDoor").GetComponent<SpriteRenderer>().color = Color.red;
            TopDoorObject.transform.Find("ClosedDoor").GetComponent<SpriteRenderer>().color = Color.magenta;
        }
        if (bottomDoorDestination?.roomType == RoomType.BOSS_ROOM)
        {
            BottomDoorObject.transform.Find("OpenedDoor").GetComponent<SpriteRenderer>().color = Color.red;
            BottomDoorObject.transform.Find("ClosedDoor").GetComponent<SpriteRenderer>().color = Color.magenta;
        }
        if (leftDoorDestination?.roomType == RoomType.BOSS_ROOM)
        {
            LeftDoorObject.transform.Find("OpenedDoor").GetComponent<SpriteRenderer>().color = Color.red;
            LeftDoorObject.transform.Find("ClosedDoor").GetComponent<SpriteRenderer>().color = Color.magenta;
        }
        if (rightDoorDestination?.roomType == RoomType.BOSS_ROOM)
        {
            RightDoorObject.transform.Find("OpenedDoor").GetComponent<SpriteRenderer>().color = Color.red;
            RightDoorObject.transform.Find("ClosedDoor").GetComponent<SpriteRenderer>().color = Color.magenta;
        }
    }

    public void CloseDoor()//문 닫기
    {
        if (IsTopDoor)
        {
            TopDoorObject.transform.Find("ClosedDoor").gameObject.SetActive(true);
            TopDoorObject.transform.Find("OpenedDoor").gameObject.SetActive(false);
        }
        if (IsBottomDoor)
        {
            BottomDoorObject.transform.Find("ClosedDoor").gameObject.SetActive(true);
            BottomDoorObject.transform.Find("OpenedDoor").gameObject.SetActive(false);
        }
        if (IsLeftDoor)
        {
            LeftDoorObject.transform.Find("ClosedDoor").gameObject.SetActive(true);
            LeftDoorObject.transform.Find("OpenedDoor").gameObject.SetActive(false);
        }
        if (IsRightDoor)
        {
            RightDoorObject.transform.Find("ClosedDoor").gameObject.SetActive(true);
            RightDoorObject.transform.Find("OpenedDoor").gameObject.SetActive(false);
        }
    }

    public void OpenDoor()//문 열기
    {
        if (IsTopDoor)
        {
            TopDoorObject.transform.Find("ClosedDoor").gameObject.SetActive(false);
            TopDoorObject.transform.Find("OpenedDoor").gameObject.SetActive(true);
        }
        if (IsBottomDoor)
        {
            BottomDoorObject.transform.Find("ClosedDoor").gameObject.SetActive(false);
            BottomDoorObject.transform.Find("OpenedDoor").gameObject.SetActive(true);
        }
        if (IsLeftDoor)
        {
            LeftDoorObject.transform.Find("ClosedDoor").gameObject.SetActive(false);
            LeftDoorObject.transform.Find("OpenedDoor").gameObject.SetActive(true);
        }
        if (IsRightDoor)
        {
            RightDoorObject.transform.Find("ClosedDoor").gameObject.SetActive(false);
            RightDoorObject.transform.Find("OpenedDoor").gameObject.SetActive(true);
        }
    }



    //-------------------------------------------------------------------------------------------------------------//문 연결하기

    public void SetSpawnPoint(Vector2Int spawnPoint, RoomInfo destinationRoom, Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            destinationRoom.topDoorEntrance = spawnPoint;
        }
        if (direction == Vector2Int.down)
        {
            destinationRoom.bottomDoorEntrance = spawnPoint;
        }
        if (direction == Vector2Int.left)
        {
            destinationRoom.leftDoorEntrance = spawnPoint;
        }
        if (direction == Vector2Int.right)
        {
            destinationRoom.rightDoorEntrance = spawnPoint;
        }
    }

    public void OpenDoor( Vector2Int direction, RoomInfo destinationRoom, Vector2Int destination)//각각의 문의 목적지 설정(현재 방의 문 방향, 연결된 방, 연결된 방의 문 방향)
    {
        if (direction == Vector2Int.up)
        {
            topDoorDestination = destinationRoom;
            SetSpawnPoint(topSpawnPoint, destinationRoom, destination);
            IsTopDoor = true;
        }
        if (direction == Vector2Int.down)
        {
            bottomDoorDestination = destinationRoom;
            SetSpawnPoint(bottomSpawnPoint, destinationRoom, destination);
            IsBottomDoor = true;
        }
        if (direction == Vector2Int.left)
        {
            leftDoorDestination = destinationRoom;
            SetSpawnPoint(leftSpawnPoint, destinationRoom, destination);
            IsLeftDoor = true;
        }
        if (direction == Vector2Int.right)
        {
            rightDoorDestination = destinationRoom;
            SetSpawnPoint(rightSpawnPoint, destinationRoom, destination);
            IsRightDoor = true;
        }
    }
}
