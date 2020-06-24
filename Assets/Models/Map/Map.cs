using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    Tile[,] _tiles;
    int _width;
    int _height;

    public int Width
    {
        get { return this._width; }
    }

    public int Height
    {
        get { return this._height; }
    }

    public Map(int width = 100, int height = 100)
    {
        this._width = width;
        this._height = height;

        _tiles = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _tiles[x, y] = new Tile(this, x, y);
            }
        }
 
        Debug.Log("Map created with " + (width * height) + " tiles");
    }

    public Tile GetTileAt(int x, int y)
    {
        if (x > _width || x < 0 || y > _height || y < 0)
        {
            Debug.LogError("Tile " + x + "," + y + " is out if range");
        } 

        return _tiles[x, y];
    }

    public void RandomizeTiles()
    {
        Debug.Log(_tiles.Length);
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (Random.Range(0, 2) == 0)
                {
                    _tiles[x, y].Type = Tile.TileType.Empty;
                }
                else
                {
                    _tiles[x, y].Type = Tile.TileType.Dirt;

                }
            }
        }
    }
}