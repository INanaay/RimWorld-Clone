using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{

    public Sprite tileSprite;

    Map _map;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("test");
        _map = new Map(100, 100);

        createMap();
        _map.RandomizeTiles();

    }

    //Create map is used to visually represent the tiles. We also set the callback for when a tile type changes.

    void createMap()
    {


        for (int x = 0; x < _map.Width; x++)
        {
            for (int y = 0; y < _map.Height; y++)
            {
                Tile currentTile = _map.GetTileAt(x, y);
                GameObject tileGameObject = new GameObject();
                tileGameObject.name = "Tile (" + x +";" + + y + ")";
                Vector2Int position = currentTile.Position;

                tileGameObject.transform.position = new Vector3(position.x, position.y, 0);
                tileGameObject.transform.SetParent(this.transform, true);

                tileGameObject.AddComponent<SpriteRenderer>();

                currentTile.RegisterOnTileTypeChangedCallback((tile) => { onTileTypeChanged(tile, tileGameObject); });

    
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
     
        
    }

    //when the type of a tile is changed, this callback is called.
    void onTileTypeChanged(Tile tileData, GameObject tileGameObject)
    {
        Debug.Log(tileData.Type);
        if (tileData.Type == Tile.TileType.Dirt)
        {
            Debug.Log("onTileTypeChanged - Setting sprite");

            tileGameObject.GetComponent<SpriteRenderer>().sprite = tileSprite;
        }
        else if (tileData.Type == Tile.TileType.Empty)
        {
            tileGameObject.GetComponent<SpriteRenderer>().sprite = null;

        }
        else
        {
            Debug.LogError("onTileTypeChanged - Unrecognized tile type.");
        }
    }
}
