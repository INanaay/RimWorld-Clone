using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tile
{
    public enum TileType { Dirt, Floor, Water, Empty };
    private TileType _tileType;
    private Map _map;
    Vector2Int _position;
    Action<Tile> _tileTypeChangedCallback;

    public TileType Type { 
        get => _tileType;
        set {
            if (_tileType != value)
            {
                Debug.Log("Setter");
                _tileType = value;
                _tileTypeChangedCallback?.Invoke(this);
            }

        }
    }
    public Vector2Int Position { get => _position; }

    public Tile(Map map, int x, int y)
    {
        _tileType = TileType.Empty;
        _map = map;
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

}