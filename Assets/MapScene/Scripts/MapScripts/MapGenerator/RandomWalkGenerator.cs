using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomWalkGenerator : MapGeneratorBase//실제 던전을 생성하는 역할의 클래스
{
    [SerializeField] protected RandomWalkSO randomWalkParameters;
    [SerializeField][Range(0, 10)] private int offset = 1;

    protected override void RunProcedurealGeneration()
    {
        //-----------------------------------------------------------------------------------------------------------//던전 생성

        RandomWalkAlgorithm.footPrint.Clear();
        RoomList.DungeonRooms.Clear();

        RunRandomWalk(randomWalkParameters, startPosition);

        List<BoundsInt> rooms = new List<BoundsInt>();//RoomInfo 클래스의 방 가져오기
        foreach(RoomInfo dungeonRoom in RoomList.DungeonRooms)
        {
            rooms.Add(dungeonRoom.room);
        }

        HashSet<Vector2Int> floor = CreateFloorRooms(rooms);//방 그리기


        List<RoomInfo> roomsPositions = new List<RoomInfo>();
        foreach (RoomInfo dungeonRoom in RoomList.DungeonRooms)
        {
            roomsPositions.Add(dungeonRoom);
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomsPositions);//통로의 해쉬셋
        floor.UnionWith(corridors);

        HashSet<Vector2Int> corridors2 = IncreaseCorrider(corridors);//통로 크기 늘리기
        floor.UnionWith(corridors2);

        tilemapVisualizer.Clear();

        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);

        //-----------------------------------------------------------------------------------------------------------//던전 설정

        RoomGenerateManager.instance.ClearRooms();//모든 방 오브젝트 삭제


        RoomGenerateManager.instance.SetRoom();//방 타입 선택
        RoomGenerateManager.instance.GenerateRoom();// 방 오브젝트 생성
    }

    //------------------------------------------------------------------------------------------------------------------------------------
    //던전 생성 함수들

    private HashSet<Vector2Int> IncreaseCorrider(HashSet<Vector2Int> corridors)//통로 크기 늘리기
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


    private HashSet<Vector2Int> ConnectRooms(List<RoomInfo> roomsPositions)//방 이어주기
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();


        while (roomsPositions.Count > 0)
        {
            RoomInfo currentRoom = roomsPositions[Random.Range(0, roomsPositions.Count)];//무작위로 방 하나 선정
            roomsPositions.Remove(currentRoom);//현재 위치는 앞으로 타겟에서 제외
            currentRoom.hasDoor = true;//현재 방은 통로 있음

            List<RoomInfo> closest = FindClosestPointTo(currentRoom, RoomList.DungeonRooms);// 가장 가까운 방들 찾기

            foreach (RoomInfo room in closest)
            {
                if (room.hasDoor == false)//통로가 없다면
                {
                    HashSet<Vector2Int> newCorrider = CreateCorridor(currentRoom, room);//통로 생성
                    corridors.UnionWith(newCorrider);
                }
            }
        }

        return corridors;
    }


    private HashSet<Vector2Int> CreateCorridor(RoomInfo currentRoom, RoomInfo destination)//통로 만들기
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoom.center;
        corridor.Add(position);

        Vector2Int currentRoomDoorDirection = Vector2Int.zero;
        Vector2Int destinationDoorDirection = Vector2Int.zero;

        while (position.y != destination.center.y)//목표 위치의 y값까지
        {
            if (destination.center.y > position.y)
            {
                position += Vector2Int.up;

                if(position == currentRoom.topDoor)//현재 방의 문에 닿았을 때
                {
                    currentRoomDoorDirection = Vector2Int.up;//방의 윗쪽문 활성화
                }
                if(position == destination.bottomSpawnPoint)//목표 방의 스폰위치에 닿았을 때
                {
                    destinationDoorDirection = Vector2Int.down;//방의 아랫쪽문 활성화
                }
            }
            else if (destination.center.y < position.y)
            {
                position += Vector2Int.down;

                if (position == currentRoom.bottomDoor)//현재 방의 문에 닿았을 때
                {
                    currentRoomDoorDirection = Vector2Int.down;//방의 아랫쪽문 활성화
                }
                if (position == destination.topSpawnPoint)//목표 방의 스폰위치에 닿았을 때
                {
                    destinationDoorDirection = Vector2Int.up;//방의 윗쪽문 활성화
                }
            }
            corridor.Add(position);
        }

        while (position.x != destination.center.x)//목표 위치의 x값까지
        {
            if (destination.center.x > position.x)
            {
                position += Vector2Int.right;

                if (position == currentRoom.rightDoor)//현재 방의 문에 닿았을 때
                {
                    currentRoomDoorDirection = Vector2Int.right;//방의 오른쪽문 활성화
                }
                if (position == destination.leftSpawnPoint)//목표 방의 스폰위치에 닿았을 때
                {
                    destinationDoorDirection = Vector2Int.left;//방의 왼쪽문 활성화
                }
            }
            else if (destination.center.x < position.x)
            {
                position += Vector2Int.left;

                if (position == currentRoom.leftDoor)//현재 방의 문에 닿았을 때
                {
                    currentRoomDoorDirection = Vector2Int.left;//방의 왼쪽문 활성화
                }
                if (position == destination.rightSpawnPoint)//목표 방의 스폰위치에 닿았을 때
                {
                    destinationDoorDirection = Vector2Int.right;//방의 오른쪽문 활성화
                }
            }
            corridor.Add(position);
        }

        currentRoom.OpenDoor(currentRoomDoorDirection, destination, destinationDoorDirection);//현재 방에서 목적지까지 가는 문 활성화
        destination.OpenDoor(destinationDoorDirection, currentRoom, currentRoomDoorDirection);//목적지에서 현재방으로 오는 문 활성화

        return corridor;
    }

    private List<RoomInfo> FindClosestPointTo(RoomInfo currentRoomCenter, List<RoomInfo> rooms)//가장 방 위치 찾기
    {
        List<RoomInfo> closestRoom = new List<RoomInfo>();
        float distance = float.MaxValue;

        foreach (var room in rooms)
        {
            if(room == currentRoomCenter)//현재 방은 건너 뛰기
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


    private HashSet<Vector2Int> CreateFloorRooms(List<BoundsInt> roomList)//바닥이 있는 방 생성
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


    protected void RunRandomWalk(RandomWalkSO randomWalkParameters, Vector2Int position)//RandomWalk 실행
    {
        var currentPosition = position;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        for (int i = 0; i < randomWalkParameters.iterations; i++)//iterations번 반복
        {
            var path = RandomWalkAlgorithm.SimpleRandomWalk(currentPosition, randomWalkParameters.walkLength);//RandomWalk 알고리즘 실행

            floorPositions.UnionWith(path);//HashSet 합치기
            if (randomWalkParameters.startRandomlyEachIteration)//랜덤 체크했으면, 반복할 때 무작위 위치에서 실행
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }

    } 
}
