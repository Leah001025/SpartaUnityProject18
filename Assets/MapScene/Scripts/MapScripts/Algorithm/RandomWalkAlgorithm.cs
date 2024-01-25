using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomWalkAlgorithm
{
    public static List<Vector2Int> footPrint = new List<Vector2Int>();

    //RandomWalk 알고리즘
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        path.Add(startPosition);
        var previousposition = startPosition;
        
        if (!footPrint.Contains(startPosition))
        {
            footPrint.Add(startPosition);
            RoomManager.DungeonRooms.Add(new RoomInfo(startPosition));
        }

        for (int i = 0; i < walkLength; i++)
        {
            var newPosition = previousposition + Direction2D.GetRandomDirection();//무작위 방향으로 이동

            while (footPrint.Contains(newPosition))//이미 들린 곳이라면
            {
                newPosition += Direction2D.GetRandomDirection();//다시 이동
            }

            footPrint.Add(newPosition);// 이미 들린 곳 표시
            path.Add(newPosition);
            RoomManager.DungeonRooms.Add(new RoomInfo(newPosition));
            previousposition = newPosition;
        }

        return path;
    }
}

public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionList = new List<Vector2Int>()//4방향 이동
    {
        new Vector2Int(0, 1),//up
        new Vector2Int(1, 0),//right
        new Vector2Int(0, -1),//down
        new Vector2Int(-1, 0)//left
    };

    public static List<Vector2Int> diagonalDirectionList = new List<Vector2Int>()//대각선 방향
    {
        new Vector2Int(1, 1),//up - right
        new Vector2Int(1, -1),//right - down
        new Vector2Int(-1, -1),//down - left
        new Vector2Int(-1, 1)//left - up
    };

    public static List<Vector2Int> eightDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0, 1),//up
        new Vector2Int(1, 1),//up - right

        new Vector2Int(1, 0),//right
        new Vector2Int(1, -1),//right - down

        new Vector2Int(0, -1),//down
        new Vector2Int(-1, -1),//down - left

        new Vector2Int(-1, 0),//left
        new Vector2Int(-1, 1)//left - up
    };

    public static List<Vector2Int> walkList = new List<Vector2Int>()//방 생성 기반 위치 이동
    {
        new Vector2Int(0, 40),//up
        new Vector2Int(50, 0),//right
        new Vector2Int(0, -40),//down
        new Vector2Int(-50, 0)//left
    };

    public static Vector2Int GetRandomDirection()//방 생성 기반 위치 이동
    {
        return walkList[Random.Range(0, walkList.Count)];
    }

    public static Vector2Int GetRandomCardinalDirection()//무작위 4방향 이동
    {
        return cardinalDirectionList[Random.Range(0, cardinalDirectionList.Count)];
    }
}
