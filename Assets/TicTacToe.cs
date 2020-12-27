using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToe : Box
{
    Box[,] grid = new Box[3, 3];
    int numFilled; //Number of boxes in the grid that are not empty

    public static int maxLevel = 1;
    protected int level = 1;

    // Start is called before the first frame update
    void Start()
    {
        initializeGrid();
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void initializeGrid()
    {
        GameObject newBox;

        for (int col = 0; col < 3; col++)
        {
            for (int row = 0; row < 3; row++)
            {
                if (level == maxLevel)
                {
                    newBox = Instantiate(GameObject.Find("TheOrigin"), this.transform);
                    grid[col, row] = newBox.GetComponent<Box>();
                } else
                {
                    newBox = Instantiate(GameObject.Find("TheOriginTTT"), this.transform);
                    newBox.GetComponent<TicTacToe>().setLevel(level + 1);
                    grid[col, row] = newBox.GetComponent<TicTacToe>();
                }

                grid[col, row].setParent(this);
                newBox.GetComponent<Transform>().parent = GetComponent<Transform>();
                grid[col, row].setPath(getNewPath(col, row));

                newBox.GetComponent<Transform>().localScale = new Vector3(0.31f, 0.31f, 0.31f);
                newBox.GetComponent<Transform>().localPosition = new Vector3((col - 1)/3f, (1 - row) / 3f, 0);

                newBox.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;

                if(level != maxLevel)
                {
                    newBox.GetComponent<TicTacToe>().enabled = true;
                }
            }
        }
    }

    private int[,] getNewPath(int col, int row)
    {
        int pathLength = path.GetLength(0);
        int[,] newPath = new int[pathLength + 1, 2];

        for (int i = 0; i < pathLength; i++)
        {
            newPath[i, 0] = path[i, 0];
            newPath[i, 1] = path[i, 1];
        }

        newPath[pathLength, 0] = col;
        newPath[pathLength, 1] = row;

        return newPath;
    }

    public void checkWin(string type, int x, int y)
    {
        if (type == "TTT")
        {
            // pass
        }
        else if (type == "Empty")
        {
            // pass
        }
        else
        {
            numFilled += 1;

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
            if (win || numFilled == 9)
            {
                if (win)
                {
                    // im an x or o
                    base.setType(type);
                    turnOffBoxes();

                    switch (type)
                    {
                        case "X":
                            GetComponent<SpriteRenderer>().sprite = XSprite;
                            break;
                        case "O":
                            GetComponent<SpriteRenderer>().sprite = OSprite;
                            break; 
                    }

                    GetComponent<SpriteRenderer>().enabled = true;

                }
                else
                {
                    // numFilled == 9
                    base.setType("Both");
                }

                int pathLength = base.getPath().GetLength(0);

                if (base.getParent() != null)
                {
                    // get coordinates of this tictactoe grid relative to its parent
                    int thisX = base.getPath()[pathLength - 1, 0];
                    int thisY = base.getPath()[pathLength - 1, 0];

                    base.getParent().checkWin(base.getType(), thisX, thisY);
                }

                // there is no parent
                // end game
            }
            // else: nothing happens
        }
    }

    public void turnOffBoxes()
    {
        for (int col = 0; col < 3; col++)
        {
            for (int row = 0; row < 3; row++)
            {
                grid[col, row].GetComponent<SpriteRenderer>().enabled = false;
                grid[col, row].GetComponent<Collider2D>().enabled = false;
                grid[col, row].enabled = false;
            }
        }
    }

    public void setLevel(int lvl)
    {
        level = lvl;
    }

    public int getLevel()
    {
        return level;
    }
}
