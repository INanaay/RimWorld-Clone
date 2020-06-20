using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public GameObject circleCursor;
    Vector3 _lastFrameCameraPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currFrameCameraPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currFrameCameraPos.z = 0;

        //update Circle Cursor pos
        circleCursor.transform.position = currFrameCameraPos;

        //Handle screen drag
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            Vector3 diff = _lastFrameCameraPos - currFrameCameraPos;
            Camera.main.transform.Translate(diff);
        }
        _lastFrameCameraPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); ;
        _lastFrameCameraPos.z = 0;
    }
}
