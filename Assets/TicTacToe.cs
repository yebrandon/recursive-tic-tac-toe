using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToe : Box
{
    Box[,] grid = new Box[3, 3];
    int numFilled; //Number of boxes in the grid that are not empty

    // Start is called before the first frame update
    void Start()
    {
        initializeGrid();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void initializeGrid()
    {
        for (int col = 0; col < 3; col++)
        {
            for (int row = 0; row < 3; row++)
            {
                grid[col, row] = Instantiate(GameObject.Find("TheOrigin"), this.transform).GetComponent<Box>();
                grid[col, row].setParent(this);
                grid[col, row].gameObject.GetComponent<Transform>().parent = this.gameObject.GetComponent<Transform>();
                grid[col, row].setPath(getNewPath(col, row));

            }
        }
    }

    private int[,] getNewPath(int col, int row)
    {
        int[,] newPath = new int[path.Length + 1, 2];

        for (int i = 0; i < path.Length; i++)
        {
            newPath[i, 0] = path[i, 0];
            newPath[i, 1] = path[i, 1];
        }

        newPath[path.Length, 0] = col;
        newPath[path.Length, 1] = row;

        return newPath;
    }

    public void checkWin(string type, int[] coords)
    {

    }
}
