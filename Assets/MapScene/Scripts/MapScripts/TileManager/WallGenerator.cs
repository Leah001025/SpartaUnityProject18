using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionList);
        var cornerWallPositions = FindWallsInDirections(floorPositions, Direction2D.diagonalDirectionList);

        CreateBasicWalls(tilemapVisualizer, basicWallPositions, floorPositions);
        CreateCornerWalls(tilemapVisualizer, cornerWallPositions, floorPositions);
    }

    private static void CreateCornerWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions)//�ڳ� �� ����
    {
        foreach (var position in cornerWallPositions)
        {
            string neighborsBinaryType = "";
            foreach (var direction in Direction2D.eightDirectionsList)
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition))
                {
                    neighborsBinaryType += "1";
                }
                else
                {
                    neighborsBinaryType += "0";
                }
            }
            tilemapVisualizer.PaintSingleCornerWall(position, neighborsBinaryType);
        }
    }

    private static void CreateBasicWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> basicWallPositions, HashSet<Vector2Int> floorPositions)//�� ����
    {
        foreach (var position in basicWallPositions)
        {
            string neighboursBinarType = "";

            foreach (var direction in Direction2D.cardinalDirectionList)
            {
                var neighborPosition = position + direction;
                if (floorPositions.Contains(neighborPosition))//�ٴ�Ÿ���� ���� ��
                {
                    neighboursBinarType += "1";
                }
                else//�ٴ�Ÿ���� ���� ��
                {
                    neighboursBinarType += "0";
                }
            }

            tilemapVisualizer.PaintSingleBasicWall(position, neighboursBinarType);
        }
        //wallTypesHelper�� �̿��� ���� ������ ���� 
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition) == false)//�ٷ� �� Ÿ���� floor�� �ƴ϶��
                {
                    wallPositions.Add(neighbourPosition);//�� ���� ��ġ ����
                }
            }
        }

        return wallPositions;
    }
}
