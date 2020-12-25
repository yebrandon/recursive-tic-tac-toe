using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public string turnPlayer = "X";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeTurn(){
        if(turnPlayer == "X"){
            turnPlayer = "O";
        }
        else{
            turnPlayer = "X";
        }
    }
}
