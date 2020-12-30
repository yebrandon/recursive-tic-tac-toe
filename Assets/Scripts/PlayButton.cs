using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public int minInput;
    public int maxInput;
    public InputField inputField;
    public static int maxLevel;

    // Start is called before the first frame update
    void Start()
    {
        minInput = 1;
        maxInput = 4;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateLevel()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            maxLevel = int.Parse(inputField.text);
            maxLevel = Mathf.Clamp(maxLevel, minInput, maxInput);
            inputField.text = maxLevel.ToString();
        }
    }

    public void switchScene()
    {
        if (string.IsNullOrEmpty(inputField.text))
        {
            inputField.text = "1".ToString();
        }
        SceneManager.LoadScene("TestScene2");
    }
}
