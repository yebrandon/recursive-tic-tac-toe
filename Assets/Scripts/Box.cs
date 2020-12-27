using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Box : MonoBehaviour
{
    protected string type = "Empty"; //Types: "X", "O", "Empty" "Both", "TTT"
    protected TicTacToe parent = null;
    protected int[,] path = new int[0, 2];

    public TurnManager turnManager;
    protected SpriteRenderer spriteRenderer;
    public Sprite BothSprite;
    public Sprite OSprite;
    public Sprite XSprite;
    protected Color hoverColor = new Color(0.5f, 1, 0.5f, 1f);
    protected Color baseColor = new Color(1, 1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnMouseEnter()
    {
        if (type == "Empty")
        {
            spriteRenderer.color = hoverColor;
        }

        turnManager.highlightNextTurn(this, true);
    }

    void OnMouseExit()
    {
        spriteRenderer.color = baseColor;
        turnManager.highlightNextTurn(this, false);
    }

    void OnMouseDown()
    {
        // Update sprite and turn player
        if (type == "Empty")
        {
            if (turnManager.turnPlayer == "X")
            {
                spriteRenderer.sprite = XSprite;
                type = "X";
                spriteRenderer.color = baseColor;
            }
            else
            {
                spriteRenderer.sprite = OSprite;
                type = "O";
                spriteRenderer.color = baseColor;
            }
            turnManager.changeTurn(this);
            parent.checkWin(type, path[path.GetLength(0) - 1, 0], path[path.GetLength(0) - 1, 1]);
        }
    }

    public static void printPath(int[,] path)
    {
        string coords = "";

        for (int i = 0; i < path.GetLength(0); i++)
        {
            coords += "[" + path[i, 0] + ", " + path[i, 1] + "] ";
        }

        Debug.Log(coords);
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

    public bool isX()
    {
        return getType() == "X" || getType() == "Both";
    }

    public bool isO()
    {
        return getType() == "O" || getType() == "Both";
    }
}
