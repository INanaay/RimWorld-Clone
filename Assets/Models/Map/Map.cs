using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Map
{
	Tile[,] tiles;

	Dictionary<string, Furniture> _furniturePrototypes;

	// The tile width of the world.
	public int Width { get; protected set; }

	// The tile height of the world
	public int Height { get; protected set; }

	Action<Furniture> _furnitureCreatedCallback;

	/// <summary>
	/// Initializes a new instance of the <see cref="World"/> class.
	/// </summary>
	/// <param name="width">Width in tiles.</param>
	/// <param name="height">Height in tiles.</param>
	public Map(int width = 100, int height = 100)
	{
		Width = width;
		Height = height;

		tiles = new Tile[Width, Height];

		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				tiles[x, y] = new Tile(this, x, y);
			}
		}

		Debug.Log("Map created with " + (Width * Height) + " tiles.");

		CreateFurniturePrototypes();

	}

	void CreateFurniturePrototypes()
    {
		_furniturePrototypes = new Dictionary<string, Furniture>();

		Furniture wallProto = Furniture.CreatePrototype("Wall", 0, 1, 1, true); // links to neighbour

		_furniturePrototypes.Add("Wall", wallProto);
	}

	

	/// <summary>
	/// A function for testing out the system
	/// </summary>
	public void RandomizeTiles()
	{
		Debug.Log("RandomizeTiles");
		Debug.Log("Width = " + Width + ", Height = " + Height);
		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{

				if (UnityEngine.Random.Range(0, 2) == 0)
				{
					tiles[x, y].Type = TileType.Floor;
				}
				else
				{
					tiles[x, y].Type = TileType.Empty;
				}

			}
		}
	}

	/// <summary>
	/// Gets the tile data at x and y.
	/// </summary>
	/// <returns>The <see cref="Tile"/>.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public Tile GetTileAt(int x, int y)
	{
		if (x > Width || x < 0 || y > Height || y < 0)
		{
			Debug.LogWarning("Tile (" + x + "," + y + ") is out of range.");
			return null;
		}
		return tiles[x, y];
	}

	public void PlaceFurniture(string objectType, Tile t)
    {
		Debug.Log("PlaceFurniture");
		if (objectType == null ||t == null)
        {
			Debug.LogError("NUll error");
        }
		if (_furniturePrototypes.ContainsKey(objectType) == false)
        {
			Debug.LogError("_furniturePrototypes doesn't contain a prototype for key : " + objectType);
			return;
        }
		Furniture obj = Furniture.PlaceInstance(_furniturePrototypes[objectType], t);

		if (obj == null)
        {
			return;
        }

        _furnitureCreatedCallback?.Invoke(obj);
    }

	public void RegisterFurnitureCreated(Action<Furniture> callback)
    {
		_furnitureCreatedCallback += callback;
    }

	public void UnregisterFurnitureCreated(Action<Furniture> callback)
	{
		_furnitureCreatedCallback -= callback;
	}
}