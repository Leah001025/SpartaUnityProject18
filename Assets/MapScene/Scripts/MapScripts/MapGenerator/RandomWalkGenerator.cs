using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomWalkGenerator : MapGeneratorBase//���� ������ �����ϴ� ������ Ŭ����
{
    [SerializeField] protected RandomWalkSO randomWalkParameters;
    [SerializeField][Range(0, 10)] private int offset = 1;

    protected override void RunProcedurealGeneration()
    {
        //-----------------------------------------------------------------------------------------------------------//���� ����

        RandomWalkAlgorithm.footPrint.Clear();
        RoomList.DungeonRooms.Clear();

        RunRandomWalk(randomWalkParameters, startPosition);

        List<BoundsInt> rooms = new List<BoundsInt>();//RoomInfo Ŭ������ �� ��������
        foreach(RoomInfo dungeonRoom in RoomList.DungeonRooms)
        {
            rooms.Add(dungeonRoom.room);
        }

        HashSet<Vector2Int> floor = CreateFloorRooms(rooms);//�� �׸���


        List<RoomInfo> roomsPositions = new List<RoomInfo>();
        foreach (RoomInfo dungeonRoom in RoomList.DungeonRooms)
        {
            roomsPositions.Add(dungeonRoom);
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomsPositions);//����� �ؽ���
        floor.UnionWith(corridors);

        HashSet<Vector2Int> corridors2 = IncreaseCorrider(corridors);//��� ũ�� �ø���
        floor.UnionWith(corridors2);

        tilemapVisualizer.Clear();

        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);

        //-----------------------------------------------------------------------------------------------------------//���� ����

        RoomGenerateManager.instance.ClearRooms();//��� �� ������Ʈ ����


        RoomGenerateManager.instance.SetRoom();//�� Ÿ�� ����
        RoomGenerateManager.instance.GenerateRoom();// �� ������Ʈ ����
    }

    //------------------------------------------------------------------------------------------------------------------------------------
    //���� ���� �Լ���

    private HashSet<Vector2Int> IncreaseCorrider(HashSet<Vector2Int> corridors)//��� ũ�� �ø���
    {
        HashSet<Vector2Int> newCorriderPosition = new HashSet<Vector2Int>();

        foreach (var corridor in corridors)
        {
            for (int i = 0; i < Direction2D.cardinalDirectionList.Count; i++)
            {
                newCorriderPosition.Add(corridor + Direction2D.cardinalDirectionList[i]);
            }
        }

        return newCorriderPosition;
    }


    private HashSet<Vector2Int> ConnectRooms(List<RoomInfo> roomsPositions)//�� �̾��ֱ�
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();


        while (roomsPositions.Count > 0)
        {
            RoomInfo currentRoom = roomsPositions[Random.Range(0, roomsPositions.Count)];//�������� �� �ϳ� ����
            roomsPositions.Remove(currentRoom);//���� ��ġ�� ������ Ÿ�ٿ��� ����
            currentRoom.hasDoor = true;//���� ���� ��� ����

            List<RoomInfo> closest = FindClosestPointTo(currentRoom, RoomList.DungeonRooms);// ���� ����� ��� ã��

            foreach (RoomInfo room in closest)
            {
                if (room.hasDoor == false)//��ΰ� ���ٸ�
                {
                    HashSet<Vector2Int> newCorrider = CreateCorridor(currentRoom, room);//��� ����
                    corridors.UnionWith(newCorrider);
                }
            }
        }

        return corridors;
    }


    private HashSet<Vector2Int> CreateCorridor(RoomInfo currentRoom, RoomInfo destination)//��� �����
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoom.center;
        corridor.Add(position);

        Vector2Int currentRoomDoorDirection = Vector2Int.zero;
        Vector2Int destinationDoorDirection = Vector2Int.zero;

        while (position.y != destination.center.y)//��ǥ ��ġ�� y������
        {
            if (destination.center.y > position.y)
            {
                position += Vector2Int.up;

                if(position == currentRoom.topDoor)//���� ���� ���� ����� ��
                {
                    currentRoomDoorDirection = Vector2Int.up;//���� ���ʹ� Ȱ��ȭ
                }
                if(position == destination.bottomSpawnPoint)//��ǥ ���� ������ġ�� ����� ��
                {
                    destinationDoorDirection = Vector2Int.down;//���� �Ʒ��ʹ� Ȱ��ȭ
                }
            }
            else if (destination.center.y < position.y)
            {
                position += Vector2Int.down;

                if (position == currentRoom.bottomDoor)//���� ���� ���� ����� ��
                {
                    currentRoomDoorDirection = Vector2Int.down;//���� �Ʒ��ʹ� Ȱ��ȭ
                }
                if (position == destination.topSpawnPoint)//��ǥ ���� ������ġ�� ����� ��
                {
                    destinationDoorDirection = Vector2Int.up;//���� ���ʹ� Ȱ��ȭ
                }
            }
            corridor.Add(position);
        }

        while (position.x != destination.center.x)//��ǥ ��ġ�� x������
        {
            if (destination.center.x > position.x)
            {
                position += Vector2Int.right;

                if (position == currentRoom.rightDoor)//���� ���� ���� ����� ��
                {
                    currentRoomDoorDirection = Vector2Int.right;//���� �����ʹ� Ȱ��ȭ
                }
                if (position == destination.leftSpawnPoint)//��ǥ ���� ������ġ�� ����� ��
                {
                    destinationDoorDirection = Vector2Int.left;//���� ���ʹ� Ȱ��ȭ
                }
            }
            else if (destination.center.x < position.x)
            {
                position += Vector2Int.left;

                if (position == currentRoom.leftDoor)//���� ���� ���� ����� ��
                {
                    currentRoomDoorDirection = Vector2Int.left;//���� ���ʹ� Ȱ��ȭ
                }
                if (position == destination.rightSpawnPoint)//��ǥ ���� ������ġ�� ����� ��
                {
                    destinationDoorDirection = Vector2Int.right;//���� �����ʹ� Ȱ��ȭ
                }
            }
            corridor.Add(position);
        }

        currentRoom.OpenDoor(currentRoomDoorDirection, destination, destinationDoorDirection);//���� �濡�� ���������� ���� �� Ȱ��ȭ
        destination.OpenDoor(destinationDoorDirection, currentRoom, currentRoomDoorDirection);//���������� ��������� ���� �� Ȱ��ȭ

        return corridor;
    }

    private List<RoomInfo> FindClosestPointTo(RoomInfo currentRoomCenter, List<RoomInfo> rooms)//���� �� ��ġ ã��
    {
        List<RoomInfo> closestRoom = new List<RoomInfo>();
        float distance = float.MaxValue;

        foreach (var room in rooms)
        {
            if(room == currentRoomCenter)//���� ���� �ǳ� �ٱ�
            {
                continue;
            }

            float currentDistance = Vector2.Distance(room.center, currentRoomCenter.center);
            if (currentDistance < distance)
            {
                distance = currentDistance;
            }
        }

        foreach (var room in rooms)
        {
            float currentDistance = Vector2.Distance(room.center, currentRoomCenter.center);
            if (currentDistance == distance)
            {
                closestRoom.Add(room);
            }
        }

        return closestRoom;
    }


    private HashSet<Vector2Int> CreateFloorRooms(List<BoundsInt> roomList)//�ٴ��� �ִ� �� ����
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        foreach (var room in roomList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }

        return floor;
    }


    protected void RunRandomWalk(RandomWalkSO randomWalkParameters, Vector2Int position)//RandomWalk ����
    {
        var currentPosition = position;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        for (int i = 0; i < randomWalkParameters.iterations; i++)//iterations�� �ݺ�
        {
            var path = RandomWalkAlgorithm.SimpleRandomWalk(currentPosition, randomWalkParameters.walkLength);//RandomWalk �˰��� ����

            floorPositions.UnionWith(path);//HashSet ��ġ��
            if (randomWalkParameters.startRandomlyEachIteration)//���� üũ������, �ݺ��� �� ������ ��ġ���� ����
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }

    } 
}
