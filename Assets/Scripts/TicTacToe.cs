using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToe : Box
{
    private Box[,] grid = new Box[3, 3];
    private int numFilled; //Number of boxes in the grid that are not empty

    public static int maxLevel = PlayButton.maxLevel;
    protected int level = 1;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        type = "TTT";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void initializeGrid()
    {
        maxLevel = PlayButton.maxLevel;
        initializeGridHelper();
    }
    private void initializeGridHelper()
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
                }
                else
                {
                    newBox = Instantiate(GameObject.Find("TheOriginTTT"), this.transform);
                    newBox.GetComponent<TicTacToe>().setLevel(level + 1);
                    grid[col, row] = newBox.GetComponent<TicTacToe>();
                }

                grid[col, row].setParent(this);
                newBox.GetComponent<Transform>().parent = GetComponent<Transform>();
                grid[col, row].setPath(getNewPath(col, row));

                newBox.GetComponent<Transform>().localScale = new Vector3(0.31f, 0.31f, 0.31f);
                newBox.GetComponent<Transform>().localPosition = new Vector3((col - 1) / 3f, (1 - row) / 3f, 0);

                newBox.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;

                if (level != maxLevel)
                {
                    if (level == maxLevel - 1)
                    {
                        TurnManager.maxLevelTTTs.Add(newBox.GetComponent<TicTacToe>());

                    }
                    newBox.GetComponent<TicTacToe>().enabled = true;
                    newBox.GetComponent<TicTacToe>().initializeGridHelper();
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
            bool gameEnded = false;

            int rowX = 0;
            int rowO = 0;
            int colX = 0;
            int colO = 0;
            int diag1X = 0; // diagonal of [0, 0], [1, 1], [2, 2]
            int diag1O = 0;
            int diag2X = 0; // diagonal of [0, 2], [1, 1], [2, 0]
            int diag2O = 0;

            for (int i = 0; i < 3; i++)
            {
                if (grid[i, y].isX()) // checks row for X
                {
                    rowX += 1;
                }
                if (grid[i, y].isO()) // checks row for O
                {
                    rowO += 1;
                }
                if (grid[x, i].isX()) // checks col for X
                {
                    colX += 1;
                }
                if (grid[x, i].isO()) // checks col for O
                {
                    colO += 1;
                }
                if (grid[i, i].isX()) // checks diag1 for X
                {
                    diag1X += 1;
                }
                if (grid[i, i].isO()) // checks diag1 for O
                {
                    diag1O += 1;
                }
                if (grid[i, 2 - i].isX()) // checks diag2 for X
                {
                    diag2X += 1;
                }
                if (grid[i, 2 - i].isO()) // checks diag2 for O
                {
                    diag2O += 1;
                }
            }

            if ((Mathf.Max(rowX, colX, diag1X, diag2X) == 3 && Mathf.Max(rowO, colO, diag1O, diag2O) == 3)) // both won
            {
                setType("Both");
                turnOffBoxes();
                GetComponent<SpriteRenderer>().sprite = BothSprite;
                GetComponent<SpriteRenderer>().enabled = true;
                gameEnded = true;
            }
            else if (Mathf.Max(rowX, colX, diag1X, diag2X) == 3) // X won
            {
                setType("X");
                turnOffBoxes();
                GetComponent<SpriteRenderer>().sprite = XSprite;
                GetComponent<SpriteRenderer>().enabled = true;
                gameEnded = true;
            }
            else if (Mathf.Max(rowO, colO, diag1O, diag2O) == 3) // O won
            {
                setType("O");
                turnOffBoxes();
                GetComponent<SpriteRenderer>().sprite = OSprite;
                GetComponent<SpriteRenderer>().enabled = true;
                gameEnded = true;
            }
            else if (numFilled == 9) // tie
            {
                setType("Both");
                turnOffBoxes();
                GetComponent<SpriteRenderer>().sprite = BothSprite;
                GetComponent<SpriteRenderer>().enabled = true;
                gameEnded = true;
            }

            if (gameEnded && getParent() != null)
            {
                // get parent to check
                int pathLength = getPath().GetLength(0);
                int thisX = getPath()[pathLength - 1, 0];
                int thisY = getPath()[pathLength - 1, 1];

                getParent().checkWin(getType(), thisX, thisY);
            }
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
                if (grid[col, row] is TicTacToe)
                {
                    ((TicTacToe)grid[col, row]).turnOffBoxes();
                }
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

    public Box getBox(int col, int row)
    {
        return grid[col, row];
    }

    new public void destroySelf()
    {
        // recursively destroy the tic tac toe board
        // base case in superclass (box)
        for (int col = 0; col < 3; col++)
        {
            for (int row = 0; row < 3; row++)
            {
                if (grid[col, row] != null)
                {
                    grid[col, row].destroySelf();
                }
                Destroy(this.GetComponent<TicTacToe>());
            }
        }
    }

    public void placeMove(int[,] path)
    {
        // place an X or O at the coordinates
        if (path.GetLength(0) == 1)
        {
            // base case
            this.getBox(path[0, 0], path[0, 1]).placeMove();
        } else
        {
            // recurse
            int[,] newPath = new int[path.Length - 1, 2];
            for (int i = 1; i < path.Length; i++)
            {
                newPath[i - 1, 0] = path[i, 0];
                newPath[i - 1, 1] = path[i, 1];
            }
            ((TicTacToe) this.getBox(path[0, 0], path[0, 1])).placeMove(newPath);
        }
    }
}
