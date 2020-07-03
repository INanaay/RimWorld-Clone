using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum TileType { Dirt, Floor, Water, Empty };

public class Tile
{
    private TileType _tileType;
    private Map _map;
    Vector2Int _position;
    Action<Tile> _tileTypeChangedCallback;

    public InstalledObject InstalledObject
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

    public bool PlaceObject(InstalledObject objInstance)
    {
        if (objInstance == null)
        {
            InstalledObject = null;
            return true;
        }

        if (InstalledObject != null)
        {
            Debug.LogError("Trying to place an object to a tile that already has one");
            return false;
        }

        InstalledObject = objInstance;
        return true;
    }
}