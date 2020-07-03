using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Map
{
	Tile[,] tiles;

	Dictionary<string, InstalledObject> _installedObjectPrototypes;

	// The tile width of the world.
	public int Width { get; protected set; }

	// The tile height of the world
	public int Height { get; protected set; }

	Action<InstalledObject> _installedObjectCreatedCallback;

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

		CreateInstalledObjectPrototypes();

	}

	void CreateInstalledObjectPrototypes()
    {
		_installedObjectPrototypes = new Dictionary<string, InstalledObject>();

		InstalledObject wallProto = InstalledObject.CreatePrototype("Wall", 0, 1, 1, true); // links to neighbour

		_installedObjectPrototypes.Add("Wall", wallProto);
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
					tiles[x, y].Type = TileType.Dirt;
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

	public void PlaceInstalledObject(string objectType, Tile t)
    {
		Debug.Log("PlaceInstalledObject");
		if (objectType == null ||t == null)
        {
			Debug.LogError("NUll error");
        }
		if (_installedObjectPrototypes.ContainsKey(objectType) == false)
        {
			Debug.LogError("_installedObjectPrototypes doesn't contain a prototype for key : " + objectType);
			return;
        }
		InstalledObject obj = InstalledObject.PlaceInstance(_installedObjectPrototypes[objectType], t);

		if (obj == null)
        {
			return;
        }

        _installedObjectCreatedCallback?.Invoke(obj);
    }

	public void RegisterInstalledObjectCreated(Action<InstalledObject> callback)
    {
		_installedObjectCreatedCallback += callback;
    }

	public void UnregisterInstalledObjectCreated(Action<InstalledObject> callback)
	{
		_installedObjectCreatedCallback -= callback;
	}
}