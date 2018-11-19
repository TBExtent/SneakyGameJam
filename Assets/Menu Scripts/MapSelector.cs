using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
public class MapSelector : MonoBehaviour {
    public GameObject realMapNameObject;
    public GameObject roomNameObject;
    public GameObject playerNameObject;
    public GameObject maxPlayersIndicatorObject;
    public GameObject maxPlayersSlider;
    public string nameOfRoom = "defaultroom";
    // Use this for initialization
    public void Awake()
    {
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.automaticallySyncScene = true;

        // the following line checks if this client was just created (and not yet online). if so, we connect
        if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated)
        {
            // Connect to the photon master-server. We use the settings saved in PhotonServerSettings (a .asset file in this project)
            PhotonNetwork.ConnectUsingSettings("0.2");
        }

        // generate a name for this player, if none is assigned yet
        if (String.IsNullOrEmpty(PhotonNetwork.playerName))
        {
            PhotonNetwork.playerName = "Guest " + Random.Range(1, 99999);
        }

        // if you wanted more debug out, turn this on:
        // PhotonNetwork.logLevel = NetworkLogLevel.Full;
    }
    public void updateMaxPlayers()
    {
        maxPlayersIndicatorObject.GetComponent<Text>().text = maxPlayersSlider.GetComponent<Slider>().value.ToString();
    }
    public void returnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    // Update is called once per frame
    void Update () {
		
	}
    public void Engage()
    {
        if (realMapNameObject.GetComponent<Text>().text != "")
        {
            makeRoom();
        }
    }
    void makeRoom()
    {
        if(playerNameObject.GetComponent<Text>().text != "")
        {
            PhotonNetwork.playerName = playerNameObject.GetComponent<Text>().text;
        }
        else
        {
            PhotonNetwork.playerName = "Host " + Random.Range(1, 99999);
        }


        if (roomNameObject.GetComponent<Text>().text != "")
        {
            nameOfRoom = roomNameObject.GetComponent<Text>().text;
        }
        else
        {
            nameOfRoom = "The host was too lazy to name the server";
        }

        int maxPlayers = int.Parse(maxPlayersIndicatorObject.GetComponent<Text>().text);
        PhotonNetwork.CreateRoom(nameOfRoom, new RoomOptions() { MaxPlayers = Convert.ToByte(maxPlayers) }, null);
    }
    public void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
        string SceneNameGame = realMapNameObject.GetComponent<Text>().text;
        PhotonNetwork.LoadLevel(SceneNameGame); //LOADS THE MAP
    }
}
