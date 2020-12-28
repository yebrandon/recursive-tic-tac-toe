using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float scrollFactor = 0.75f; // how much scrolling zooms in or out
    private float normalCameraSpeed = 0.05f; // how fast the camera moves for WASD
    private float startSize;

    // Start is called before the first frame update
    void Start()
    {
        startSize = Camera.main.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        // zoomer
        float scrollChange = Input.mouseScrollDelta.y;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize * Mathf.Pow(scrollFactor, scrollChange), 0.1f, 5f);

        // movement
        float cameraSpeed = normalCameraSpeed * (Camera.main.orthographicSize / startSize); // scale camera speed by zoom factor
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, cameraSpeed), Space.World);
        } 
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(cameraSpeed * -1, 0), Space.World);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, cameraSpeed * -1), Space.World);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(cameraSpeed, 0), Space.World);
        }

        // can't go too far away
        var pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, -4, 4);
        pos.y = Mathf.Clamp(transform.position.y, -4, 4);
        transform.position = pos;


    }

    // todo: move camera w/ click and drag
}
