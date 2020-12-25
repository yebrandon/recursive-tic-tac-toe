using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Box : MonoBehaviour
{
    protected string type = "Empty"; //Types: "X", "O", "Empty" "Both", "TTT"
    protected TicTacToe parent = null;
    protected int[,] path = new int[0, 2];

    private TurnManager turnManager;
    private SpriteRenderer spriteRenderer;
    private Sprite XSprite;
    private Sprite OSprite;
    private Color hoverColor;

    // Start is called before the first frame update
    void Start()
    {
        type = "Empty";
        parent = null;
        path = new int[0, 2];
        hoverColor = new Color(0, 1, 0, 1);
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseOver()
    {
        spriteRenderer.color = hoverColor;
    }

    void OnMouseDown()
    {
        if (type == "Empty")
        {
            if (turnManager.turnPlayer == "X")
            {
                spriteRenderer.sprite = XSprite;
                type = "X";
            }
            else
            {
                spriteRenderer.sprite = OSprite;
                type = "O";
            }
            turnManager.changeTurn();
        }
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
