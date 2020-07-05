using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture 
{
    public Tile Tile {
        get; protected set;
    }

    public string ObjectType {
        get; protected set;
    }
    float _movementCost; //this is a multiplier.

    // For example, a sofa might be a 3x2 object, when the graphics are only 3x1
    int _width;
    int _height;

    public bool LinksToNeighbour {
        get; protected set;
    }

    Action<Furniture> _onChangeCallback;

    Func<Tile, bool> _funcPositionValidation;

    protected Furniture ()
    {

    }

    static public Furniture CreatePrototype(string objectType, float movementCost = 1f, int width = 1, int height = 1, bool linksToNeighbour = false)
    {
        Furniture furn = new Furniture();
        furn.ObjectType = objectType;
        furn._movementCost = movementCost;
        furn._width = width;
        furn._height = height;
        furn.LinksToNeighbour = linksToNeighbour;

        furn._funcPositionValidation = furn.IsValidPosition;

        return (furn);
    }

    //TODO Ref or not ?
    static public Furniture PlaceInstance(Furniture proto, Tile tile)
    {

        if (proto._funcPositionValidation(tile) == false)
        {
            Debug.LogError("PlaceInstance - Position validity Function returned false");
            return null;
        }

        Furniture obj = new Furniture();
        obj.ObjectType = proto.ObjectType;
        obj._movementCost = proto._movementCost;
        obj._width = proto._width;
        obj._height = proto._height;
        obj.LinksToNeighbour = proto.LinksToNeighbour;

        obj.Tile = tile;

        if (tile.PlaceFurniture(obj) == false)
        {
            //We werent able to place the object in this tile. It is most likely already occupied.
            return null;
        }


        if (obj.LinksToNeighbour)
        {
            // this furniture has neighbours, so we should inform them that this furniture change.
            // We have to trigger their OnChangeCallback

            Tile t;
            int x = tile.Position.x;
            int y = tile.Position.y;

            t = tile.Map.GetTileAt(x, y + 1);
            if (t != null && t.Furniture != null && t.Furniture.ObjectType == obj.ObjectType)
            {
                // We have a northern Neighbour with the same object type as us, so 
                // tell it that it has changed by firing this callback
                t.Furniture._onChangeCallback(t.Furniture);
            }
            t = tile.Map.GetTileAt(x + 1, y);
            if (t != null && t.Furniture != null && t.Furniture.ObjectType == obj.ObjectType)
            {
                t.Furniture._onChangeCallback(t.Furniture);
            }
            t = tile.Map.GetTileAt(x, y - 1);
            if (t != null && t.Furniture != null && t.Furniture.ObjectType == obj.ObjectType)
            {
                t.Furniture._onChangeCallback(t.Furniture);
            }
            t = tile.Map.GetTileAt(x - 1, y);
            if (t != null && t.Furniture != null && t.Furniture.ObjectType == obj.ObjectType)
            {
                t.Furniture._onChangeCallback(t.Furniture);
            }
        }

        return (obj);
    }

    public void RegisterOnChangeCallback(Action<Furniture> callback)
    {
        _onChangeCallback += callback;
    }

    public void UnregisterOnChangeCallback(Action<Furniture> callback)
    {
        _onChangeCallback -= callback;
    }

    public bool IsValidPosition(Tile tile)
    {
        if (tile.Type != TileType.Floor || tile.Furniture != null)
        {
            return false;
        }
        
        return true;
    }

    public bool isValidPosition_Door(Tile t)
    {
        //Make sur we have E and W wall or N and S wall.

        if (IsValidPosition(t) == false ) {
            return false;
        }
        return true;
    }
}
