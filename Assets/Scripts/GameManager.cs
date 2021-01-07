using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameObject connectToServerMenu;
    private GameObject gameLobbyMenu;
    private GameObject serverMenu;

    public GameObject gameButtonPrefab;
    public GameObject clientPrefab;
    public Client client;

    private Text connectingStatus;
    private Text serverLobbyStatus;
    private GameInstanceClient[] games;
    private GameSettings settings;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null) {

            client = Instance.client;
            Instance.client.manager = this;
            settings = Instance.settings;
            Destroy(Instance.gameObject);
        };

        Instance = this;
        DontDestroyOnLoad(gameObject);

        initialize();

        connectToServerMenu.SetActive(true);
        gameLobbyMenu.SetActive(false);
        serverMenu.SetActive(false);

        games = new GameInstanceClient[16];
    }

    public void initialize()
    {
        if (SceneManager.GetActiveScene().name.Equals("ConnectToServer"))
        {
            Transform canvas = GameObject.Find("Canvas").transform;

            connectToServerMenu = canvas.Find("ConnectToServer").gameObject;
            gameLobbyMenu = canvas.Find("GameLobby").gameObject;
            serverMenu = canvas.Find("Server").gameObject;

            connectingStatus = connectToServerMenu.transform.Find("ConnectingLabel").GetComponent<Text>();
            serverLobbyStatus = serverMenu.transform.Find("ServerMessage").GetComponent<Text>();
        }
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
        if (hostAddress.Equals("")) hostAddress = "127.0.0.1";

        try
        {
            if (client == null)
            {
                client = Instantiate(clientPrefab).GetComponent<Client>();
            }

            client.clientName = GameObject.Find("NameInput").GetComponent<InputField>().text;
            if (client.clientName == null || client.clientName.Equals("")) client.clientName = "Idiot Who Forgot To Put A Name";

            client.manager = this;
            client.inGame = false;

            client.ConnectToServer(hostAddress, 6321);
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

    public void SetUpServerMenu(string message)
    {
        connectToServerMenu.SetActive(false);
        gameLobbyMenu.SetActive(false);
        serverMenu.SetActive(true);

        for(int i = 0; i < 16; i++)
        {
            if(games[i] != null)
            {
                Destroy(games[i].gameObject);
                games[i] = null;
            }
        }

        serverMenu.GetComponent<Transform>().Find("NameDisplay").GetComponent<Text>().text = client.clientName;
        serverLobbyStatus.text = message;
    }

    public void SetUpGameLobby(int id, string hostName, string challengerName, string hostStatus, string challengerStatus, int layers, string hostTurn, string isHost)
    {
        connectToServerMenu.SetActive(false);
        gameLobbyMenu.SetActive(true);
        serverMenu.SetActive(false);

        settings = new GameSettings(id, hostName, challengerName, hostStatus, challengerStatus, layers, hostTurn, isHost);

        gameLobbyMenu.transform.Find("Dropdown").GetComponent<Dropdown>().interactable = settings.isHost;
        UpdateGameLobbyUI();
    }

    public void UpdateGameSettings(string challengerName, string hostStatus, string challengerStatus, int layers)
    {
        settings.challengerName = challengerName;
        settings.status[0] = hostStatus;
        settings.status[1] = challengerStatus;
        settings.layers = layers;
        UpdateGameLobbyUI();
    }

    public void UpdateGameLobbyUI()
    {
        gameLobbyMenu.transform.Find("GameLobbyName").GetComponent<Text>().text = "Game Lobby " + settings.id;

        Transform hostInfo = gameLobbyMenu.transform.Find("HostInfo");
        hostInfo.Find("Name").GetComponent<Text>().text = settings.hostName;
        hostInfo.Find("Status").GetComponent<Text>().text = settings.status[0];
        hostInfo.Find("TurnAssignment").GetComponent<Text>().text = settings.turnAssignment[0];

        Transform challengerInfo = gameLobbyMenu.transform.Find("ChallengerInfo");
        challengerInfo.Find("TurnAssignment").GetComponent<Text>().text = settings.turnAssignment[1];
        challengerInfo.Find("Name").GetComponent<Text>().text = settings.challengerName;
        challengerInfo.Find("Status").GetComponent<Text>().text = settings.status[1];

        gameLobbyMenu.transform.Find("Dropdown").GetComponent<Dropdown>().value = settings.layers - 1;
    }

    public void ServerMessage(string message)
    {
        connectingStatus.text = message;
        serverLobbyStatus.text = message;
    }

    public void RequestNewGame()
    {
        client.Send("C/NewGame");
    }

    public void RequestJoinGame(int id)
    {
        client.Send("C/JoinGame|" + id);
    }

    public void RequestServerLobbyData()
    {
        client.Send("C/ServerLobbyData");
    }

    public void ChangeReadyStatus()
    {
        if (settings.isHost)
        {
            settings.flipStatus(0);
            gameLobbyMenu.transform.Find("HostInfo").Find("Status").GetComponent<Text>().text = settings.status[0];
            client.Send("C/UpdateGameSettings|" + settings.id + "|" + settings.status[0] + "|" + settings.layers);
        } else
        {
            settings.flipStatus(1);
            gameLobbyMenu.transform.Find("ChallengerInfo").Find("Status").GetComponent<Text>().text = settings.status[1];
            client.Send("C/UpdateGameSettings|" + settings.id + "|" + settings.status[1] + "|" + settings.layers);
        }
    }

    public void ChangeNumOfLayers(Dropdown drop)
    {
        settings.layers = drop.value + 1;
        client.Send("C/UpdateGameSettings|" + settings.id + "|" + settings.status[0] + "|" + settings.layers);
    }

    public void AddNewGameLobby(int i, string status)
    {
        games[i] = Instantiate(gameButtonPrefab).GetComponent<GameInstanceClient>();
        games[i].transform.SetParent(serverMenu.transform.Find("GameLobbies").transform, false);
        games[i].lobbyID = i;
        games[i].status = status;
        UpdateServerLobbyUI();
    }

    public void DeleteGameLobby(int id)
    {
        Destroy(games[id].gameObject);
        games[id] = null;
        UpdateServerLobbyUI();
    }

    public void UpdateGameStatus(int id, string status)
    {
        games[id].status = status;
        games[id].UpdateUI();
    }

    private void UpdateServerLobbyUI()
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

    public void CallLateCMD()
    {
        if(client != null)
        {
            client.RunLateCMD();
        }
    }

    public void NotifySceneLoaded()
    {
        client.Send("C/SceneLoaded|" + settings.id);
    }

    public void Disconnect()
    {
        client.CloseSocket();
    }

}

public class GameSettings
{
    public int id;
    public string hostName;
    public string challengerName;
    public bool isHost;

    public string[] status = new string[2];
    public int layers;
    public string[] turnAssignment = new string[2];

    public GameSettings(int id, string hostName, string challengerName, string hostStatus, string challengerStatus, int layers, string hostTurn, string isHost)
    {
        this.id = id;
        this.hostName = hostName;
        this.challengerName = challengerName;
        status[0] = hostStatus;
        status[1] = challengerStatus;
        this.layers = layers;

        if(hostTurn.Equals("X"))
        {
            this.turnAssignment[0] = "X";
            this.turnAssignment[1] = "O";
        } else
        {
            this.turnAssignment[0] = "O";
            this.turnAssignment[1] = "X";
        }

        this.isHost = isHost.Equals("host");
    }

    public void flipStatus(int i)
    {
        if(status[i].Equals("Ready"))
        {
            status[i] = "Not Ready";
        } else
        {
            status[i] = "Ready";
        }
    }
}
