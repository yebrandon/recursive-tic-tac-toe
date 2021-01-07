using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Client : MonoBehaviour
{
    public string clientName;

    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;
    public GameManager manager;
    public TurnManager turnManager;

    public string serverCommand = null;

    public bool inGame;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public bool ConnectToServer(string host, int port)
    {
        if (socketReady)
        {
            return false;
        }

        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);

            socketReady = true;
        }

        catch (Exception e)
        {
            throw e;
        }

        return socketReady;
    }

    private void Update()
    {
        if (socketReady)
        {
            if (stream.DataAvailable)
            {
                string data = reader.ReadLine();

                if(data != null)
                {
                    OnIncomingData(data);
                }
            }
        }
    }

    public void Send(string data)
    {
        if (!socketReady) return;

        try
        {
            writer.WriteLine(data);
            writer.Flush();
        }
        catch (Exception e)
        {
            CloseSocket();
            manager.SetUpConnectMenu(e.ToString());
        }

        Debug.Log("sent " + data);
    }
    private void OnIncomingData(string data)
    {
        string[] aData = data.Split('|');
        Debug.Log(data);

        switch (aData[0])
        {
            case "S/Who":

                Send("C/Name|" + clientName);
                break;

            case "S/CurrentGames":

                manager.SetUpServerMenu(aData[aData.Length - 1]);

                for(int i = 1; i < aData.Length - 1; i++)
                {
                    string[] lobbyData = aData[i].Split('~');
                    manager.AddNewGameLobby(int.Parse(lobbyData[0]), lobbyData[1]);
                }

                break;

            case "S/NewGame":

                manager.AddNewGameLobby(int.Parse(aData[1]), "(1/2)");
                break;
            case "S/GameLimit":

                manager.ServerMessage("Game limit of 16 reached");
                break;

            case "S/JoinGame":

                manager.SetUpGameLobby(int.Parse(aData[1]), aData[2], aData[3], aData[4], aData[5], int.Parse(aData[6]), aData[7], aData[8]);
                break;

            case "S/UpdateGameSettings":

                manager.UpdateGameSettings(aData[3], aData[4], aData[5], int.Parse(aData[6]));
                break;

            case "S/Disconnect":

                if (inGame)
                {
                    inGame = false;
                    SceneManager.LoadScene("ConnectToServer");
                    serverCommand = data;
                }

                CloseSocket();
                manager.SetUpConnectMenu("Server was closed.");
                break;

            case "S/GameDeleted":
                manager.DeleteGameLobby(int.Parse(aData[1]));
                break;

            case "S/UpdateGameStatus":
                manager.UpdateGameStatus(int.Parse(aData[1]), aData[2]);
                break;

            case "S/LoadGame":

                if (!inGame)
                {
                    PlayButton.maxLevel = int.Parse(aData[1]);
                    inGame = true;
                    SceneManager.LoadScene("TestScene2");
                }

                break;

            case "S/StartGame":

                turnManager = GameObject.Find("TurnManager").GetComponent<TurnManager>();
                turnManager.playerTurn = aData[1];
                turnManager.enableFirstTTT(int.Parse(aData[2]));

                break;

            case "S/OpponentDC":

                serverCommand = data;
                inGame = false;
                SceneManager.LoadScene("ConnectToServer");
                break;

            default:

                Debug.Log("Unknown data: " + data);
                break;
        }
    }

    public void RunLateCMD()
    {
        Debug.Log("help");
        if (!SceneManager.GetActiveScene().name.Equals("ConnectToServer") || serverCommand == null) return;
        Debug.Log("me");

        string[] aServerCommand = serverCommand.Split('|');

        switch (aServerCommand[0]){

            case "S/OpponentDC":


                Debug.Log("here");
                manager.initialize();
                manager.SetUpServerMenu("Opponent disconnected");

                for (int i = 1; i < aServerCommand.Length; i++)
                {
                    string[] lobbyData = aServerCommand[i].Split('~');
                    manager.AddNewGameLobby(int.Parse(lobbyData[0]), lobbyData[1]);
                }
                serverCommand = null;

                break;

            case "S/Disconnect":

                manager.initialize();
                CloseSocket();
                manager.SetUpConnectMenu("Server was closed.");
                serverCommand = null;
                break;

            default:

                Debug.Log("Unknown data: " + serverCommand);
                break;

        }
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }

    private void OnDisable()
    {
        CloseSocket();
    }

    public void CloseSocket()
    {
        if (!socketReady) return;

        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }
}
