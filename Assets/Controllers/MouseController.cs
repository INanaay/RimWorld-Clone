using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
	public GameObject circleCursorPrefab;

	bool _buildModeObjects = false;
	TileType _buildModeTyle = TileType.Dirt;
	string _buildModeObjectType;

	// The world-position of the mouse last frame.
	Vector3 lastFramePosition;
	Vector3 currFramePosition;

	// The world-position start of our left-mouse drag operation
	Vector3 dragStartPosition;
	List<GameObject> _selectedTilesHintGameObject;
	// Use this for initialization
	void Start()
	{
		_selectedTilesHintGameObject = new List<GameObject>();
	}

	// Update is called once per frame
	void Update()
	{
		currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		currFramePosition.z = 0;

		//UpdateCursor();
		UpdateDragging();
		UpdateCameraMovement();

		// Save the mouse position from this frame
		// We don't use currFramePosition because we may have moved the camera.
		lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		lastFramePosition.z = 0;
	}

	//void UpdateCursor()
	//{
	//	// Update the circle cursor position
	//	Tile tileUnderMouse = MapController.Instance.GetTileAtWorldCoord(currFramePosition);
	//	if (tileUnderMouse != null)
	//	{
	//		circleCursor.SetActive(true);
	//		Vector3 cursorPosition = new Vector3(tileUnderMouse.Position.x, tileUnderMouse.Position.y, 0);
	//		circleCursor.transform.position = cursorPosition;
	//	}
	//	else
	//	{
	//		// Mouse is outside of the valid tile space, so hide the cursor.
	//		circleCursor.SetActive(false);
	//	}
	//}

	void UpdateDragging()
	{
		// If we are over a UI element, forbid
		if (EventSystem.current.IsPointerOverGameObject())
        {
			return;
        }

		// Start Drag
		if (Input.GetMouseButtonDown(0))
		{
			dragStartPosition = currFramePosition;
		}

		int start_x = Mathf.FloorToInt(dragStartPosition.x);
		int end_x = Mathf.FloorToInt(currFramePosition.x);
		int start_y = Mathf.FloorToInt(dragStartPosition.y);
		int end_y = Mathf.FloorToInt(currFramePosition.y);

		// We may be dragging in the "wrong" direction, so flip things if needed.
		if (end_x < start_x)
		{
			int tmp = end_x;
			end_x = start_x;
			start_x = tmp;
		}
		if (end_y < start_y)
		{
			int tmp = end_y;
			end_y = start_y;
			start_y = tmp;
		}

		

		// clean old drag previews
		while (_selectedTilesHintGameObject.Count > 0)
        {
			GameObject go = _selectedTilesHintGameObject[0];
			_selectedTilesHintGameObject.RemoveAt(0);
			SimplePool.Despawn(go);
        }

		if (Input.GetMouseButton(0))
        {
			for (int x = start_x; x <= end_x; x++)
			{
				for (int y = start_y; y <= end_y; y++)
				{
					Tile t = MapController.Instance.Map.GetTileAt(x, y);
					if (t != null)
					{
						//display building hint on top of the tile
						GameObject go = SimplePool.Spawn(circleCursorPrefab, new Vector3(x, y, 0), Quaternion.identity);
						go.transform.SetParent(this.transform, true);
						_selectedTilesHintGameObject.Add(go);
					}
				}
			}
		}

		// End Drag
		if (Input.GetMouseButtonUp(0))
		{
			// Loop through all the tiles
			for (int x = start_x; x <= end_x; x++)
			{
				for (int y = start_y; y <= end_y; y++)
				{

					Tile t = MapController.Instance.Map.GetTileAt(x, y);
					if (t != null)
					{
						if (_buildModeObjects)
						{
							Debug.Log("Build Mode");
							MapController.Instance.Map.PlaceInstalledObject(_buildModeObjectType, t);
						}
						else
						{
							t.Type = _buildModeTyle;
						}
					}
				}
			}
		}
	}

	void UpdateCameraMovement()
	{
		// Handle screen panning
		if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
		{   // Right or Middle Mouse Button

			Vector3 diff = lastFramePosition - currFramePosition;
			Camera.main.transform.Translate(diff);
		}

		Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel");
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 4f, 30f);
	}

	public void SetMode_BuildFloor()
    {
		_buildModeObjects = false;
		_buildModeTyle = TileType.Dirt;
    }

	public void SetMode_Bulldoze()
    {
		_buildModeObjects = false;
		_buildModeTyle = TileType.Empty;
    }

	public void SetMode_BuildInstalledObject(string objectType)
    {
		_buildModeObjects = true;
		_buildModeObjectType = objectType;
	}
} 
