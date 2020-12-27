using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public string turnPlayer = "X";
    //public TicTacToe father;

    public Box hovering;
    public int[,] nextTurnpath;

    // Start is called before the first frame update
    void Start()
    {

        //father.initializeGrid();
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

        int[,] nextTurnPath = new int[clicked.getPath().GetLength(0) - 1, 2];

    }

    public void getNextTurnPath(Box clicked, int[,] nextTurnPath)
    {
        int pathLength = clicked.getPath().GetLength(0);

        for (int i = 1; i < pathLength; i++)
        {
            nextTurnPath[i - 1, 0] = clicked.getPath()[i, 0];
            nextTurnPath[i - 1, 1] = clicked.getPath()[i, 1];
        }
    }
}
