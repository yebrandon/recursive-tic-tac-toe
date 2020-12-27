using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public string turnPlayer = "X";
    public TicTacToe father;

    // Start is called before the first frame update
    void Start()
    {

        father.initializeGrid();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void changeTurn(Box clicked)
    {

        if (turnPlayer == "X")
        {
            turnPlayer = "O";
        }
        else
        {
            turnPlayer = "X";
        }

        if (TicTacToe.maxLevel == 1) return;

        int[,] nextTurnPath = getNextTurnPath(clicked);
        TicTacToe nextTTT = getTicTacToe(nextTurnPath);

        clicked.getParent().enableBoxes(false);
        nextTTT.enableBoxes(true);
    }

    public void highlightNextTurn(Box clicked, bool highlight)
    {
        if (TicTacToe.maxLevel == 1) return;

        int[,] nextTurnPath = getNextTurnPath(clicked);

        TicTacToe TTT = getTicTacToe(nextTurnPath);
        TTT.highlightBoxes(highlight);
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
            TTT = (TicTacToe)TTT.getBox(path[i, 0], path[i, 1]);
        }

        return TTT;
    }
}
