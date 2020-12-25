using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private string type; //Types: "X", "O", "Empty" "Both", "TTT"
    private TicTacToe parent;
    private string[,] path;

    // Start is called before the first frame update
    void Start()
    {
        type = "empty";
        parent = null;
        path = new string[0, 2];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setType(string type)
    {
        this.type = type;
    }

    public void setParent(TicTacToe parent)
    {
        this.parent = parent;
    }

    public void setPath(string[,] path)
    {
        this.path = path;
    }

    public string getType()
    {
        return type;
    }

    public TicTacToe getParent()
    {
        return parent;
    }

    public string[,] getPath()
    {
        return path;
    }
}
