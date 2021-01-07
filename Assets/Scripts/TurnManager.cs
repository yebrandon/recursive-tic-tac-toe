using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public string currentTurn = "X";
    public string playerTurn = "X";
    public TicTacToe father;
    public static List<TicTacToe> maxLevelTTTs;
    private bool freedom = false;

    // Start is called before the first frame update
    void Start()
    {
        maxLevelTTTs = new List<TicTacToe>();
        father.initializeGrid();
        father.enabled = true;
        //father.initializeGrid();
        //father.enableBoxes(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void enableFirstTTT(int num)
    {
        if(playerTurn == currentTurn)
        {
            if (PlayButton.maxLevel == 1)
            {
                father.enableBoxes(true);
            }
            else
            {
                TicTacToe selection = maxLevelTTTs[num];
                selection.enableBoxes(true);
            }
        }
    }

    public void changeTurn(Box clicked)
    {
        if (currentTurn == "X")
        {
            currentTurn = "O";
        }
        else
        {
            currentTurn = "X";
        }

        if (TicTacToe.maxLevel == 1) return;

        int[,] nextTurnPath = getNextTurnPath(clicked);
        TicTacToe nextTTT = getTicTacToe(nextTurnPath);

        if (freedom)
        {
            father.enableBoxes(false);
            freedom = false;
        }
        else
        {
            clicked.getParent().enableBoxes(false);
        }

        if (nextTTT.getType() != "TTT")
        {
            father.enableBoxes(true);
            freedom = true;
        }
        else
        {
            nextTTT.enableBoxes(true);
        }
    }

    public void highlightNextTurn(Box clicked, bool highlight)
    {
        if (TicTacToe.maxLevel == 1) return;

        int[,] nextTurnPath = getNextTurnPath(clicked);

        TicTacToe TTT = getTicTacToe(nextTurnPath);

        if (TTT.getType() != "TTT")
        {
            father.highlightBoxes(highlight, currentTurn);
        }
        else
        {
            TTT.highlightBoxes(highlight, currentTurn);
        }
    }

    public int[,] getNextTurnPath(Box clicked)
    {
        int pathLength = clicked.getPath().GetLength(0);
        int[,] nextTurnPath = new int[pathLength - 1, 2];

        for (int i = 1; i < pathLength; i++)
        {
            nextTurnPath[i - 1, 0] = clicked.getPath()[i, 0];
            nextTurnPath[i - 1, 1] = clicked.getPath()[i, 1];
        }

        return nextTurnPath;
    }

    private TicTacToe getTicTacToe(int[,] path)
    {
        TicTacToe TTT = father;

        for (int i = 0; i < path.GetLength(0); i++)
        {
            if (TTT.getType() != "TTT")
            {
                return TTT;
            }

            TTT = (TicTacToe)TTT.getBox(path[i, 0], path[i, 1]);
        }

        return TTT;
    }

    public void destroyTTT()
    {
        for (int col = 0; col < 3; col++)
        {
            for (int row = 0; row < 3; row++)
            {
                father.getBox(col, row).destroySelf();
            }
        }

        currentTurn = "X";
        freedom = false;
        PlayButton.maxLevel = 3;
    }
}
