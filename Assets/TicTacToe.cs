using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToe : Box
{
    Box[,] grid;
    int numFilled; //Number of boxes in the grid that are not empty

    // Start is called before the first frame update
    void Start()
    {
        grid = new Box[3, 3];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void initializeGrid()
    {

    }

    public void checkWin(string type, int[] coords)
    {

    }
}
