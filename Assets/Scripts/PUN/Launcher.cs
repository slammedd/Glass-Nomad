﻿using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] private string gameSceneName = "SCN_Blockout"; // Used to change scene when we are join a room.
    [SerializeField] private GameObject controlPanel = null; // Used to show/hide the play button and input field.
    [SerializeField] private GameObject progressLabel = null; // Used to display "Connecting..." to once the Connect() funtion is called.
    [SerializeField] private GameObject playerItemPrefab = null;
    [SerializeField] private GameObject loadGameButton = null;
    [SerializeField] private Transform playerListPanel = null;

    private byte maxPlayersPerRoom = 5; // Used to set a limit to the number of players in a room.
    private string gameVersion = "1"; // Used to separate users from each other by gameVersion.
    private bool isConnection = false; // Used to stop us from immediately joining the room if we leave it.
    
    private void Awake()
    {
        // Means we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Makes sure the correct elements of the UI are visible.
    private void Start()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public void Connect()
    {
        // Switches which UI elements are visable.
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);

        // The button has been pressed so we want the user to connect to a room.
        isConnection = true;
        
        // Checks if the client is aleady connected
        if (PhotonNetwork.IsConnected)
        {
            // Attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            Debug.Log("Connected, joining a random room");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // Otherwise, connect to Photon Online Server.
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");

        if (isConnection)
        {
            // Try and join a potential existing room, else, we'll be called back with OnJoinRandomFailed().
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("Disconnected with reason:" + cause);
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // We failed to join a random room, maybe none exists or they are all full. So we create a new room.
        Debug.Log("No room found, creating new room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Room joined successfully");
        progressLabel.SetActive(false);


        if (PhotonNetwork.IsMasterClient)
        {
            loadGameButton.SetActive(true);
        }

        UpdatePlayerList();
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
        }

        UpdatePlayerList();
    }


    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
        }

        UpdatePlayerList();
    }

    private void UpdatePlayerList()
    {
        for(int i = 0; i < playerListPanel.childCount; i++)
        {
            Destroy(playerListPanel.GetChild(i).gameObject);

        }
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject go = Instantiate(playerItemPrefab, playerListPanel);
            if (player.IsMasterClient)
            {
                Debug.Log("MASTER IN ROOM:: " + player.NickName);
                go.GetComponentInChildren<Text>().text = "Room owner: " + player.NickName;
            }
            else
            {
                Debug.Log("PLAYER IN ROOM:: " + player.NickName);
                go.GetComponentInChildren<Text>().text = player.NickName;
            }
        }
    }

    public void OnLoadGameClick()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(gameSceneName);
        }
    }
}
