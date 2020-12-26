using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Box : MonoBehaviour
{
    protected string type = "Empty"; //Types: "X", "O", "Empty" "Both", "TTT"
    protected TicTacToe parent = null;
    protected int[,] path = new int[0, 2];

    public TurnManager turnManager;
    private SpriteRenderer spriteRenderer;
    public Sprite XSprite;
    public Sprite OSprite;
    private Color hoverColor;
    private Color baseColor;

    // Start is called before the first frame update
    void Start()
    {
        type = "Empty";
        parent = null;
        path = new int[0, 2];
        hoverColor = new Color(0.5f, 1, 0.5f, 1f);
        baseColor = new Color(1, 1, 1, 1);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseOver()
    {
        if (type == "Empty")
        {
            spriteRenderer.color = hoverColor;
        }
    }

    void OnMouseExit()
    {
        spriteRenderer.color = baseColor;
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
            turnManager.changeTurn();
            parent.checkWin(type, path[path.Length - 1, 0], path[path.Length - 1, 1]);
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
