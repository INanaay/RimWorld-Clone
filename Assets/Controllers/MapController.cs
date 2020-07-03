using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapController : MonoBehaviour
{
	public static MapController Instance { get; protected set; }
	public Sprite floorSprite;

	// The world and tile data
	public Map Map { get; protected set; }


	Dictionary<Tile, GameObject> _tileGameObjectMap;
	Dictionary<InstalledObject, GameObject> _installedGameObjectMap;

	Dictionary<string, Sprite> _installedObjectsSprites;


	// Use this for initialization
	void Start()
	{

		_installedObjectsSprites = new Dictionary<string, Sprite>();
		Sprite[] sprites = Resources.LoadAll<Sprite>("Textures/InstalledObjects/Wall");
		Debug.Log("Sprites loaded");

		foreach (Sprite s in sprites)
        {
			_installedObjectsSprites[s.name] = s;
        }

		if (Instance != null)
		{
			Debug.LogError("There should never be two world controllers.");
		}
		Instance = this;

		// Create a world with Empty tiles
		Map = new Map();

		Map.RegisterInstalledObjectCreated(OnInstalledObjectCreated);

		_tileGameObjectMap = new Dictionary<Tile, GameObject>();
		_installedGameObjectMap = new Dictionary<InstalledObject, GameObject>();

		// Create a GameObject for each of our tiles, so they show visually. (and redunt reduntantly)
		for (int x = 0; x < Map.Width; x++)
		{
			for (int y = 0; y < Map.Height; y++)
			{
				// Get the tile data
				Tile tile_data = Map.GetTileAt(x, y);

				// This creates a new GameObject and adds it to our scene.
				GameObject tile_go = new GameObject();

				_tileGameObjectMap.Add(tile_data, tile_go);

				tile_go.name = "Tile_" + x + "_" + y;
				tile_go.transform.position = new Vector3(tile_data.Position.x, tile_data.Position.y, 0);
				tile_go.transform.SetParent(this.transform, true);

				// Add a sprite renderer, but don't bother setting a sprite
				// because all the tiles are empty right now.
				tile_go.AddComponent<SpriteRenderer>();

				
				tile_data.RegisterOnTileTypeChangedCallback(OnTileTypeChanged);
			}
		}

		// Shake things up, for testing.
		Map.RandomizeTiles();
	}

	// Update is called once per frame
	void Update()
	{

	}

	// This function should be called automatically whenever a tile's type gets changed.
	void OnTileTypeChanged(Tile tile_data)
	{
		if (_tileGameObjectMap.ContainsKey(tile_data) == false)
        {
			Debug.LogError("tileGameObjectMap does not contain the tile_data");
			return;
        }
		GameObject tile_go = _tileGameObjectMap[tile_data];

		if (tile_go == null)
        {
			Debug.LogError("tileGameObjectMap's returned GameObject is null");
			return;
		}

		if (tile_data.Type == TileType.Dirt)
		{
			tile_go.GetComponent<SpriteRenderer>().sprite = floorSprite;
		}
		else if (tile_data.Type == TileType.Empty)
		{
			tile_go.GetComponent<SpriteRenderer>().sprite = null;
		}
		else
		{
			Debug.LogError("OnTileTypeChanged - Unrecognized tile type.");
		}


	}

	/// <summary>
	/// Gets the tile at the unity-space coordinates
	/// </summary>
	/// <returns>The tile at world coordinate.</returns>
	/// <param name="coord">Unity World-Space coordinates.</param>
	public Tile GetTileAtWorldCoord(Vector3 coord)
	{
		int x = Mathf.FloorToInt(coord.x);
		int y = Mathf.FloorToInt(coord.y);

		return Map.GetTileAt(x, y);
	}

	public void OnInstalledObjectCreated(InstalledObject obj)
    {
		//create a visual Gameobject linked to an object.
		GameObject obj_go = new GameObject();

		_installedGameObjectMap.Add(obj, obj_go);

		obj_go.name = obj.ObjectType + " " + obj.Tile.Position.x + "_" + obj.Tile.Position.y;
		obj_go.transform.position = new Vector3(obj.Tile.Position.x, obj.Tile.Position.y, 0);
		obj_go.transform.SetParent(this.transform, true);

		// Add a sprite renderer, but don't bother setting a sprite
		// because all the tiles are empty right now.
		obj_go.AddComponent<SpriteRenderer>().sprite = GetSpriteForInstalledObject(obj);

		obj.RegisterOnChangeCallback(onInstalledObjectChange);
	}

	void onInstalledObjectChange(InstalledObject obj)
    {
		Debug.LogError("onInstalledObjectChange NOT IMPLEMENTED");
    }

	Sprite GetSpriteForInstalledObject(InstalledObject obj)
    {
		if (obj.LinksToNeighbour == false)
        {
			return (_installedObjectsSprites[obj.ObjectType]);
        }

		//otherwise, sprite name is more complicated.
		string spriteName = obj.ObjectType + "_";

		Tile t;
		int x = obj.Tile.Position.x;
		int y = obj.Tile.Position.y;

		t = Map.GetTileAt(x, y + 1);
		if (t != null && t.InstalledObject != null && t.InstalledObject.ObjectType == obj.ObjectType)
        {
			spriteName += "N";
        }
		t = Map.GetTileAt(x + 1, y);
		if (t != null && t.InstalledObject != null && t.InstalledObject.ObjectType == obj.ObjectType)
		{
			spriteName += "E";
		}
		t = Map.GetTileAt(x, y - 1);
		if (t != null && t.InstalledObject != null && t.InstalledObject.ObjectType == obj.ObjectType)
		{
			spriteName += "S";
		}
		t = Map.GetTileAt(x - 1, y);
		if (t != null && t.InstalledObject != null && t.InstalledObject.ObjectType == obj.ObjectType)
		{
			spriteName += "W";
		}

		// For example if this object has all four neighbours of
		// the same type, the string will look like :
		// Wall_NESW

		if (_installedObjectsSprites.ContainsKey(spriteName) == false)
        {
			Debug.LogError("GetSpriteForInstalledObject -- No sprites with name : " + spriteName);
			return null;
        }

		return _installedObjectsSprites[spriteName];
    }
}
