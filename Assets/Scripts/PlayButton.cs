using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    // Start is called before the first frame update
    public int minInput;
    public int maxInput;
    public InputField inputField;
    public static int maxLevel;

    void Start()
    {
        minInput = 1;
        maxInput = 3;
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
}
