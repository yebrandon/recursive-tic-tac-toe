using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    protected string type = "Empty"; //Types: "X", "O", "Empty" "Both", "TTT"
    protected TicTacToe parent = null;
    protected int[,] path = new int[0, 2];

    // Start is called before the first frame update
    void Start()
    {

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

    public void setPath(int[,] path)
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

    public int[,] getPath()
    {
        return path;
    }
}
