using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstalledObject 
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

    Action<InstalledObject> _onChangeCallback;


    protected InstalledObject ()
    {

    }

    static public InstalledObject CreatePrototype(string objectType, float movementCost = 1f, int width = 1, int height = 1, bool linksToNeighbour = false)
    {
        InstalledObject obj = new InstalledObject();
        obj.ObjectType = objectType;
        obj._movementCost = movementCost;
        obj._width = width;
        obj._height = height;
        obj.LinksToNeighbour = linksToNeighbour;

        return (obj);
    }

    //TODO Ref or not ?
    static public InstalledObject PlaceInstance(InstalledObject proto, Tile tile)
    {
        InstalledObject obj = new InstalledObject();
        obj.ObjectType = proto.ObjectType;
        obj._movementCost = proto._movementCost;
        obj._width = proto._width;
        obj._height = proto._height;
        obj.LinksToNeighbour = proto.LinksToNeighbour;

        obj.Tile = tile;

        if (tile.PlaceObject(obj) == false)
        {
            //We werent able to place the object in this tile. It is most likely already occupied.
            return null;
        }

        return (obj);
    }

    public void RegisterOnChangeCallback(Action<InstalledObject> callback)
    {
        _onChangeCallback += callback;
    }

    public void UnregisterOnChangeCallback(Action<InstalledObject> callback)
    {
        _onChangeCallback -= callback;
    }
}
