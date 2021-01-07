using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Box : MonoBehaviour
{
    protected string type = "Empty"; //Types: "X", "O", "Empty" "Both", "TTT"
    protected TicTacToe parent = null;
    protected int[,] path = new int[0, 2];

    public TurnManager turnManager;
    public SpriteRenderer spriteRenderer;
    public Sprite BothSprite;
    public Sprite OSprite;
    public Sprite XSprite;
    public Sprite emptySprite;
    protected Color xHoverColor = new Color(0.5f, 1, 0.5f, 1f);
    protected Color oHoverColor = new Color(0.5f, 0.5f, 1, 1f);
    public Color baseColor = new Color(1, 1, 1, 1);
    public Color enabledColor = new Color(1, 1, 1, 1);
    public Color disabledColor = new Color(1, 1, 1, 0.5f);
    public Collider2D boxCollider;
    protected Color xHoverColorFade = new Color(0.3f, 0.7f, 0.3f, 1f);
    protected Color oHoverColorFade = new Color(0.3f, 0.3f, 0.7f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<Collider2D>();
        baseColor = disabledColor;
        spriteRenderer.color = baseColor;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnMouseEnter()
    {
        if (type == "Empty")
        {
            turnManager.highlightNextTurn(this, true);

            if (turnManager.currentTurn == "X")
            {
                highlight(xHoverColor);
                spriteRenderer.sprite = XSprite;
            }
            else
            {
                highlight(oHoverColor);
                spriteRenderer.sprite = OSprite;
            }
        }
    }

    void OnMouseExit()
    {
        if (type == "Empty")
        {
            turnManager.highlightNextTurn(this, false);
            highlight(baseColor);
            spriteRenderer.sprite = emptySprite;
        }
    }

    void OnMouseDown()
    {
        placeMove();
    }

    public void placeMove()
    {
        // Update sprite and turn player
        if (type == "Empty")
        {
            type = turnManager.currentTurn;

            highlight(baseColor);
            parent.checkWin(type, path[path.GetLength(0) - 1, 0], path[path.GetLength(0) - 1, 1]);
            turnManager.changeTurn(this);
        }
    }

    public static string printPath(int[,] path)
    {
        string coords = "";

        for (int i = 0; i < path.GetLength(0); i++)
        {
            coords += "[" + path[i, 0] + ", " + path[i, 1] + "] ";
        }
        return coords;
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

    public void highlight(Color color)
    {
        spriteRenderer.color = color;
    }

    public void setBaseColor(Color color)
    {
        baseColor = color;
    }

    public Color getBaseColor()
    {
        return baseColor;
    }

    public void enableBoxes(bool enabled)
    {
        if (type == "TTT")
        {
            for (int col = 0; col < 3; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    ((TicTacToe)this).getBox(col, row).enableBoxes(enabled);
                }
            }
        }
        else
        {
            GetComponent<Collider2D>().enabled = enabled;

            if (enabled)
            {
                if (turnManager.currentTurn == "X")
                {
                    setBaseColor(xHoverColorFade);
                    highlight(xHoverColorFade);
                }

                else
                {
                    setBaseColor(oHoverColorFade);
                    highlight(oHoverColorFade);
                }
            }
            else
            {
                setBaseColor(new Color(1, 1, 1, 1));
                highlight(new Color(1, 1, 1, 1));
            }
        }
    }

    public void highlightBoxes(bool light, string turnPlayer)
    {
        if (type == "TTT")
        {
            for (int col = 0; col < 3; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    ((TicTacToe)this).getBox(col, row).highlightBoxes(light, turnPlayer);
                }
            }
        }
        else
        {
            if (!light)
            {
                highlight(getBaseColor());
            }
            else
            {
                if (turnPlayer == "X")
                {
                    highlight(oHoverColor);
                }
                else
                {
                    highlight(xHoverColor);
                }
            }
        }
    }

    public void destroySelf()
    {
        Destroy(this.GetComponent<Box>());
    }
}
