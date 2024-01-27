using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomWalkAlgorithm
{
    public static List<Vector2Int> footPrint = new List<Vector2Int>();

    //RandomWalk �˰���
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
            var newPosition = previousposition + Direction2D.GetRandomDirection();//������ �������� �̵�

            while (footPrint.Contains(newPosition))//�̹� �鸰 ���̶��
            {
                newPosition += Direction2D.GetRandomDirection();//�ٽ� �̵�
            }

            footPrint.Add(newPosition);// �̹� �鸰 �� ǥ��
            path.Add(newPosition);
            RoomManager.DungeonRooms.Add(new RoomInfo(newPosition));
            previousposition = newPosition;
        }

        return path;
    }
}

public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionList = new List<Vector2Int>()//4���� �̵�
    {
        new Vector2Int(0, 1),//up
        new Vector2Int(1, 0),//right
        new Vector2Int(0, -1),//down
        new Vector2Int(-1, 0)//left
    };

    public static List<Vector2Int> diagonalDirectionList = new List<Vector2Int>()//�밢�� ����
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

    public static List<Vector2Int> walkList = new List<Vector2Int>()//�� ���� ��� ��ġ �̵�
    {
        new Vector2Int(0, 40),//up
        new Vector2Int(50, 0),//right
        new Vector2Int(0, -40),//down
        new Vector2Int(-50, 0)//left
    };

    public static Vector2Int GetRandomDirection()//�� ���� ��� ��ġ �̵�
    {
        return walkList[Random.Range(0, walkList.Count)];
    }

    public static Vector2Int GetRandomCardinalDirection()//������ 4���� �̵�
    {
        return cardinalDirectionList[Random.Range(0, cardinalDirectionList.Count)];
    }
}
