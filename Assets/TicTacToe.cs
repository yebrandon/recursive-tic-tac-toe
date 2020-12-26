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
        if (type == "TTT")
        {
            // pass
        }
        else if (type == "EMPTY")
        {
            // pass
        }
        else
        {
            numFilled += 1;
            // type is X or O, check if row/col/diagonal matches 
            int x = coords[0];
            int y = coords[1];

            bool win = false;
            int rowMatching = 0;
            int colMatching = 0;

            for (int i = 0; i < 3; i++)
            {
                if (grid[i, y].getType() == type) // checks if the row matches the box being checked
                {
                    rowMatching += 1;
                }

                if (grid[x, i].getType() == type) // checks if the column matches the box being checked 
                {
                    colMatching += 1;
                }
            }

            if (rowMatching == 3 || colMatching == 3) // 3 in a row = win
            {
                win = true;
            }

            // checks diagonals 2 lazy 2 optimize
            if (grid[0, 2] == grid[1, 1] && grid[1, 1] == grid[2, 0])
            {
                win = true;
            }
            if (grid[0, 0] == grid[1, 1] && grid[1, 1] == grid[2, 2])
            {
                win = true;
            }

            // do the stuff needed to be done if a win/tie is found
            if (win)
            {
                // im an x or o
                base.setType(type);
                int pathLength = base.getPath().GetLength(2);
                int[] thisCoords = new int[]{base.getPath()[pathLength, 0], base.getPath()[pathLength, 1]};
                base.getParent().checkWin(base.getType(), thisCoords);

            }
            else if (numFilled == 0) // not a win, but grid filled -> tie
            {
                // me both now
                base.setType("Both");
                int pathLength = base.getPath().GetLength(2);
                int[] thisCoords = new int[] { base.getPath()[pathLength, 0], base.getPath()[pathLength, 1] };
                base.getParent().checkWin(base.getType(), thisCoords);
            }
            // else: nothing happens
        }
    }
}
