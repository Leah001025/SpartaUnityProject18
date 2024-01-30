using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public enum RoomType
{
    NoThing, START_ROOM, MONSTER_ROOM, BOSS_ROOM, ITEM_ROOM
}

public class RoomInfo
{
    private GameObject prefabsObject;

    private GameObject RoomSquare;//�� ���� üũ�� ���� �簢��

    public GameObject TopDoorObject;//���� �� ������Ʈ
    public GameObject BottomDoorObject;//�Ʒ� �� �� ������Ʈ
    public GameObject LeftDoorObject;//���� �� ������Ʈ
    public GameObject RightDoorObject;//������ �� ������Ʈ

    public BoundsInt room;//�� ��ü
    public Vector2Int center;// �� �߾� ��ġ
    public RoomType roomType;// �� Ÿ��

    private int width = 36;// �� ����
    private int height = 26;//�� ����

    public bool hasDoor = false;//����� ������ �ִ��� Ȯ��

    public bool IsTopDoor = false;
    public bool IsBottomDoor = false;
    public bool IsRightDoor = false;
    public bool IsLeftDoor = false;

    public RoomInfo leftDoorDestination;//���� ���� ����� ��
    public RoomInfo rightDoorDestination;//������ ���� ����� ��
    public RoomInfo topDoorDestination;//���� ���� ����� ��
    public RoomInfo bottomDoorDestination;//�Ʒ��� ���� ����� ��

    public Vector2Int leftDoor;//���� ���� ���� �� ��ġ
    public Vector2Int rightDoor;//���� ���� ������ �� ��ġ
    public Vector2Int topDoor;//���� ���� ���� �� ��ġ
    public Vector2Int bottomDoor;//���� ���� �Ʒ��� �� ��ġ

    public Vector2Int leftDoorEntrance;//���� ������ ���� ��, ������ ��ġ
    public Vector2Int rightDoorEntrance;//������ ������ ���� ��, ������ ��ġ
    public Vector2Int bottomDoorEntrance;//���� ������ ���� ��, ������ ��ġ
    public Vector2Int topDoorEntrance;//�Ʒ��� ������ ���� ��, ������ ��ġ

    public Vector2Int leftSpawnPoint;//���ʿ��� ������ ��ġ
    public Vector2Int rightSpawnPoint;//�����ʿ��� ������ ��ġ
    public Vector2Int topSpawnPoint;//���ʿ��� ������ ��ġ
    public Vector2Int bottomSpawnPoint;//�Ʒ��ʿ��� ������ ��ġ

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
        topSpawnPoint = this.center + new Vector2Int(0, (height / 2) - 4);
        bottomSpawnPoint = this.center - new Vector2Int(0, (height / 2) - 4);
    }

    private BoundsInt CreateRooms(Vector2Int center, int width, int height)//�� ����
    {
        BoundsInt box = new BoundsInt(new Vector3Int(center.x - (width / 2), center.y - (height / 2), 0), new Vector3Int(width, height, 0));//�� �����

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

    public void SetObject()//���� ������Ʈ ����
    {
        prefabsObject.transform.position = (Vector3Int)center;
        RoomSquare = prefabsObject.transform.Find("RoomInside").gameObject;
        GameObject roomMap = prefabsObject.transform.Find("RoomMap").gameObject;

        switch (roomType)
        {
            case RoomType.BOSS_ROOM:
                roomMap.GetComponent<SpriteRenderer>().color = new Color(200 / 255f, 50 / 255f, 50 / 255f, 150 / 255f);
                break;
            case RoomType.START_ROOM:
                RoomSquare.GetComponent<RoomInside>().isClear = true;
                roomMap.GetComponent<SpriteRenderer>().color = new Color(50 / 255f, 50 / 255f, 200 / 255f, 150 / 255f);
                break;
            case RoomType.MONSTER_ROOM:
                roomMap.GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 150 / 255f);
                break;
            case RoomType.ITEM_ROOM:
                RoomSquare.GetComponent<RoomInside>().isClear = true;
                roomMap.GetComponent<SpriteRenderer>().color = new Color(50 / 255f, 255 / 255f, 50 / 255f, 150 / 255f);
                break;
        }

        TopDoorObject = prefabsObject.transform.Find("TopDoor").gameObject;

        BottomDoorObject = prefabsObject.transform.Find("BottomDoor").gameObject;

        LeftDoorObject = prefabsObject.transform.Find("LeftDoor").gameObject;

        RightDoorObject = prefabsObject.transform.Find("RightDoor").gameObject;

        ActiveDoor();
        OpenDoor();
    }

    private void ActiveDoor()//�� ����
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

        //������ �� �ٸ���
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

    public void CloseDoor()//�� �ݱ�
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

    public void OpenDoor()//�� ����
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



    //-------------------------------------------------------------------------------------------------------------//�� �����ϱ�

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

    public void OpenDoor( Vector2Int direction, RoomInfo destinationRoom, Vector2Int destination)//������ ���� ������ ����(���� ���� �� ����, ����� ��, ����� ���� �� ����)
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
