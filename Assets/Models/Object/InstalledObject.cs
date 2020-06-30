using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstalledObject 
{
    Tile _tile;

    string _objectType;
    float _movementCost; //this is a multiplier.

    // For example, a sofa might be a 3x2 object, when the graphics are only 3x1
    int _width;
    int _height;

    public InstalledObject(string objectType, float movementCost = 1f, int width = 1, int height = 1)
    {
        _objectType = objectType;
        _movementCost = movementCost;
        _width = width;
        _height = height;
    }

    public InstalledObject(InstalledObject proto, Tile tile)
    {
        _objectType = proto._objectType;
        _movementCost = proto._movementCost;
        _width = proto._width;
        _height = proto._height;

        _tile = tile;
    }
}
