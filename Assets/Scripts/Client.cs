using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class Client : MonoBehaviour
{
    public string clientName;

    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;
    public GameManager manager;

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
            CloseSocket(e.ToString());
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

                manager.SetUpLobbyUI("");

                for(int i = 1; i < aData.Length; i++)
                {
                    string[] lobbyData = aData[i].Split('~');
                    manager.AddNewGameLobby(int.Parse(lobbyData[0]), lobbyData[1]);
                }

                break;

            case "S/NewGame":

                manager.AddNewGameLobby(int.Parse(aData[1]), "(1/2)");
                break;

            case "S/Disconnect":
                CloseSocket("Server was closed.");
                break;

            default:

                Debug.Log("Unknown data: " + data);
                break;
        }
    }

    private void OnApplicationQuit()
    {
        CloseSocket("");
    }

    private void OnDisable()
    {
        CloseSocket("");
    }

    public void CloseSocket(string message)
    {
        if (!socketReady) return;

        manager.SetUpConnectMenu(message);

        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }
}
