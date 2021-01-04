using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject mainMenu;
    public GameObject serverManu;
    public GameObject connectMenu;

    public GameObject serverPrefab;
    public GameObject clientPrefab;
    public Client client;

    // Start is called before the first frame update
    void Start()
    { 
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ConnectToServerButton()
    {
        string hostAddress = GameObject.Find("HostInput").GetComponent<InputField>().text;

        if (hostAddress == "") hostAddress = "192.168.0.10";

        try
        {
            if(client == null)
            {
                client = Instantiate(clientPrefab).GetComponent<Client>();
            }

            client.clientName = GameObject.Find("NameInput").GetComponent<InputField>().text;
            if (client.clientName == null || client.clientName == "") client.clientName = "Idiot Who Forgot To Put A Name";
            client.ConnectToServer(hostAddress, 7345);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
