using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float scrollFactor = 0.75f; // how much scrolling zooms in or out

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float scrollChange = Input.mouseScrollDelta.y;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize * Mathf.Pow(scrollFactor, scrollChange), 0.1f, 5f);
    }

    // todo: move camera w/ click and drag
}
