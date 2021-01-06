using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject connectToServerMenu;
    public GameObject gameLobbyMenu;
    public GameObject serverMenu;

    public GameObject gameButtonPrefab;
    public GameObject clientPrefab;
    public Client client;

    private Text connectingStatus;
    private Text serverLobbyStatus;
    private GameInstanceClient[] games;

    // Start is called before the first frame update
    void Start()
    { 
        Instance = this;
        DontDestroyOnLoad(gameObject);

        connectToServerMenu.SetActive(true);
        gameLobbyMenu.SetActive(false);
        serverMenu.SetActive(false);

        games = new GameInstanceClient[16];

        connectingStatus = connectToServerMenu.transform.Find("ConnectingLabel").GetComponent<Text>();
        serverLobbyStatus = serverMenu.transform.Find("ServerMessage").GetComponent<Text>();
    }

    public void ConnectToServerButton()
    {
        connectingStatus.text = "Connecting...";
        StartCoroutine(ActuallyConnect());
    }

    private IEnumerator ActuallyConnect()
    {
        yield return new WaitForFixedUpdate();

        string hostAddress = GameObject.Find("HostInput").GetComponent<InputField>().text;
        if (hostAddress == "") hostAddress = "135.0.152.209";

        try
        {
            if (client == null)
            {
                client = Instantiate(clientPrefab).GetComponent<Client>();
            }

            client.clientName = GameObject.Find("NameInput").GetComponent<InputField>().text;
            if (client.clientName == null || client.clientName == "") client.clientName = "Idiot Who Forgot To Put A Name";

            client.manager = this;

            client.ConnectToServer(hostAddress, 6320);
        }
        catch (Exception e)
        {
            Destroy(client.gameObject);
            Debug.Log(e.Message);
            connectingStatus.text = e.Message;
        }
    }

    public void SetUpConnectMenu(string message)
    {
        for (int i = 0; i < 16; i++)
        {
            if (games[i] != null)
            {
                Destroy(games[i].gameObject);
                games[i] = null;
            }
        }

        connectingStatus.text = message;
        connectToServerMenu.SetActive(true);
        gameLobbyMenu.SetActive(false);
        serverMenu.SetActive(false);
    }

    public void SetUpLobbyUI(string message)
    {
        connectToServerMenu.SetActive(false);
        gameLobbyMenu.SetActive(false);
        serverMenu.SetActive(true);

        serverMenu.GetComponent<Transform>().Find("NameDisplay").GetComponent<Text>().text = client.clientName;
        serverLobbyStatus.text = message;
    }

    public void RequestNewGame()
    {
        client.Send("C/NewGame");
    }

    public void AddNewGameLobby(int i, string status)
    {
        games[i] = Instantiate(gameButtonPrefab).GetComponent<GameInstanceClient>();
        games[i].transform.SetParent(serverMenu.transform.Find("GameLobbies").transform, false);
        games[i].lobbyID = i;
        games[i].status = status;
        UpdateGameLobbyUI();
    }

    private void UpdateGameLobbyUI()
    {
        int numOfGames = 0;

        for (int i = 0; i < 16; i++)
        {
            if (games[i] != null)
            {
                games[i].UpdateUI(numOfGames);
                numOfGames++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
