using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInstanceClient : MonoBehaviour
{
    // Start is called before the first frame update
    public int lobbyID;
    public string status;

    public void UpdateUI(int i)
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.localPosition = new Vector3(0, 261 - i * 30, 0);

        UpdateUI();
    }

    public void UpdateUI()
    {
        GetComponent<Transform>().Find("ID").GetComponent<Text>().text = lobbyID.ToString();
        GetComponent<Transform>().Find("Status").GetComponent<Text>().text = status;
    }

    public void JoinGame()
    {
        if (status.Equals("(1/2)"))
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().RequestJoinGame(lobbyID);
        }
    }
}
