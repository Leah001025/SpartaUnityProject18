using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap wallTilemap;

    [SerializeField] private List<TileBase> floorTile;

    [SerializeField] private TileBase wallTop;//위쪽 벽
    [SerializeField] private TileBase wallBottom;//아랫쪽 벽
    [SerializeField] private TileBase wallLeft;//왼쪽 벽
    [SerializeField] private TileBase wallRight;//오른쪽 벽
    [SerializeField] private TileBase wallFull;//양쪽 벽

    [SerializeField] private TileBase wallRightFront;//아래 오른쪽 ㄴ자
    [SerializeField] private TileBase wallLeftFront;//아래 왼쪽 ㄴ자

    [SerializeField] private TileBase rightFront;//오른쪽 아래 기둥
    [SerializeField] private TileBase leftFront;//왼쪽 아래 기둥
    [SerializeField] private TileBase cornerRightwall;//오른쪽 위 벽
    [SerializeField] private TileBase cornerLeftwall;//왼쪽 위 벽

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintFloorTiles(floorPositions, floorTilemap, floorTile);
    }

    private void PaintFloorTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, List<TileBase> tile)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(tilemap, tile[Random.Range(0, floorTile.Count)], position);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

    internal void PaintSingleBasicWall(Vector2Int position, string binaryType)
    {
        //Debug.Log(position + " type : " + binaryType);
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        if (WallTypesHelper.wallTop.Contains(typeAsInt))
        {
            tile = wallTop;
        }
        else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = wallRight;
        }
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallLeft;
        }
        else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
        {
            tile = wallBottom;
        }
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            tile = wallFull;
        }

        if (tile != null)
        {
            PaintSingleTile(wallTilemap, tile, position);
        }

    }

    internal void PaintSingleCornerWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallLeftFront;
        }
        else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt))
        {
            tile = wallRightFront;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt))
        {
            tile = rightFront;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt))
        {
            tile = leftFront;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt))
        {
            tile = cornerLeftwall;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt))
        {
            tile = cornerRightwall;
        }
        else if (WallTypesHelper.wallFullEightDirections.Contains(typeAsInt))
        {
            tile = wallFull;
        }
        else if (WallTypesHelper.wallBottmEightDirections.Contains(typeAsInt))
        {
            tile = wallBottom;
        }

        if (tile != null)
        {
            PaintSingleTile(wallTilemap, tile, position);
        }
    }
}
