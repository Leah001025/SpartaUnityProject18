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

    private GameObject RoomSquare;//�� ���� üũ�� ���� �簢��

    //public GameObject TopDoor;
    //public GameObject BottomDoor;
    //public GameObject LeftDoor;
    //public GameObject RightDoor;

    public BoundsInt room;//�� ��ü
    public Vector2Int center;// �� �߾� ��ġ
    public RoomType roomType;// �� Ÿ��

    public bool isClear = false;//���� Ŭ���� �ߴ��� Ȯ��
    public bool isFirst = false;//�ش� �濡 ó�� �����ϴ��� Ȯ��

    private int width = 36;// �� ����
    private int height = 26;//�� ����

    public Vector2Int leftDoorDestination;//���� ������ ���� ��, ������ ��ġ
    public Vector2Int rightDoorDestination;//������ ������ ���� ��, ������ ��ġ
    public Vector2Int topDoorDestination;//���� ������ ���� ��, ������ ��ġ
    public Vector2Int downDoorDestination;//�Ʒ��� ������ ���� ��, ������ ��ġ

    public Vector2Int leftDoor;//���� �� ��ġ
    public Vector2Int rightDoor;//������ �� ��ġ
    public Vector2Int topDoor;//���� �� ��ġ
    public Vector2Int downDoor;//�Ʒ��� �� ��ġ

    public Vector2Int leftSpawnPoint;//���ʿ��� ������ ��ġ
    public Vector2Int rightSpawnPoint;//�����ʿ��� ������ ��ġ
    public Vector2Int topSpawnPoint;//���ʿ��� ������ ��ġ
    public Vector2Int downSpawnPoint;//�Ʒ��ʿ��� ������ ��ġ

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

    private BoundsInt CreateRooms(Vector2Int center, int width, int height)//�� ����
    {
        BoundsInt box = new BoundsInt(new Vector3Int(center.x - (width / 2), center.y - (height / 2), 0), new Vector3Int(width, height, 0));//�� �����

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

    //�̵� ��ġ ��ȯ
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

    //�� Ȱ��ȭ
    public void OpenDoor(Vector2Int direction, Vector2Int destination)//������ ���� ������ ����
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
