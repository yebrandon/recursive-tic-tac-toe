using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadedDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().CallLateCMD();
    }
}
