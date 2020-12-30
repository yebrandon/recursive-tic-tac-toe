using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    protected Color hoverColor = new Color(0.5f, 1, 0.5f, 1f);
    protected Color baseColor = new Color(1, 1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchScene()
    {
        SceneManager.LoadScene("InputScene");
    }
}
