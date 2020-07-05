using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum TileType { Floor, Empty };

public class Tile
{
    private TileType _tileType;
    public Map Map { get; protected set; }
    Vector2Int _position;
    Action<Tile> _tileTypeChangedCallback;

    public Furniture Furniture
    {
        get; protected set;
    }

    public TileType Type { 
        get => _tileType;
        set {
            if (_tileType != value)
            {
                _tileType = value;
                _tileTypeChangedCallback?.Invoke(this);
            }

        }
    }
    public Vector2Int Position { get => _position; }

    public Tile(Map map, int x, int y)
    {
        _tileType = TileType.Empty;
        Map = map;
        _position = new Vector2Int(x, y);
    }

    public void RegisterOnTileTypeChangedCallback(Action<Tile> callback)
    {
        _tileTypeChangedCallback += callback;
    }

    public void UnregisterOnTileTypeChangedCallback(Action<Tile> callback)
    {
        _tileTypeChangedCallback -= callback;
    }

    public bool PlaceFurniture(Furniture objInstance)
    {
        if (objInstance == null)
        {
            Furniture = null;
            return true;
        }

        if (Furniture != null)
        {
            Debug.LogError("Trying to place an object to a tile that already has one");
            return false;
        }

        Furniture = objInstance;
        return true;
    }
}