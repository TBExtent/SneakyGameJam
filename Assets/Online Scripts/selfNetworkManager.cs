using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class selfNetworkManager : MonoBehaviour {
    SpawnPoint[] spawnPoints;

    public bool offlineMode = false;
    public string strPlayerGameObject = "basicNetworkCharacter";
    bool connecting = false;

    List<string> chatMessages;
    int maxChatMessages = 5;
    bool hasPickedTeam = false;
    int teamIDSaved = 0;
    public float playerRespawnTimer;

    public GameObject standbyCamera;

    public int DebugTeamID;
    void Start() {
        spawnPoints = GameObject.FindObjectsOfType<SpawnPoint>();
        PhotonNetwork.player.name = PlayerPrefs.GetString("Username", "Player");
        chatMessages = new List<string>();
        if(standbyCamera == null)
        {
            standbyCamera = GameObject.Find("StandbyCamera");
        }


    }
    void OnDestroy()
    {
        PlayerPrefs.SetString("Username", PhotonNetwork.player.name);
    }


    void Connect()
    {
        PhotonNetwork.ConnectUsingSettings("v001");

    }



    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());

        if (PhotonNetwork.connected == false && connecting == false)
        {
            //Not yet connected, ask for online or offline.
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Username: ");
            PhotonNetwork.player.name = GUILayout.TextField(PhotonNetwork.player.name);
            GUILayout.EndHorizontal();



            if (GUILayout.Button("Multi Player"))
            {
                connecting = true;
                Connect();
            }

            if (GUILayout.Button("Single player"))
            {
                connecting = true;
                PhotonNetwork.offlineMode = true;
                OnJoinedLobby();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        if (PhotonNetwork.connected == true && connecting == false)
        {
            if (hasPickedTeam)
            {
                //Fully connected so display chat
                GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
                GUILayout.BeginVertical();
                GUILayout.FlexibleSpace();

                foreach (string msg in chatMessages)
                {
                    GUILayout.Label(msg);
                }

                GUILayout.EndVertical();
                GUILayout.EndArea();

            }
            else
            {
                //Player has not selected team
                GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical();
                GUILayout.FlexibleSpace();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Username: ");
                PhotonNetwork.player.name = GUILayout.TextField(PhotonNetwork.player.name);
                GUILayout.EndHorizontal();


                if (GUILayout.Button("Renegade"))
                {
                    teamIDSaved = 0;
                    SpawnMyPlayer(0);
                }

                if (GUILayout.Button("Red"))
                {
                    teamIDSaved = 1;
                    SpawnMyPlayer(1);
;                }

                if (GUILayout.Button("Blue"))
                {
                    teamIDSaved = 2;
                    SpawnMyPlayer(2);
                }

              //  if (GUILayout.Button("Yellow"))
              //  {
              //      teamIDSaved = 3;
              //      SpawnMyPlayer(3);
              //  }


                if (GUILayout.Button("Auto Assign"))
                {
                    teamIDSaved = Random.Range(1, 3);
                    SpawnMyPlayer(teamIDSaved);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            }

        }
    }

    void OnJoinedLobby()
    {
        Debug.Log("Joined");
        PhotonNetwork.JoinRandomRoom();
    }
    void OnConnectedToMaster()
    {
        Debug.Log("Joined");
        PhotonNetwork.JoinRandomRoom();
    }
    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
    }
    void OnJoinedRoom()
    {
        connecting = false;
    }
    void Update()
    {
        if(playerRespawnTimer > 0)
        {
            playerRespawnTimer -= Time.deltaTime;
            if(playerRespawnTimer <= 0)
            {
                //Respawn player
                SpawnMyPlayer(teamIDSaved);
            }
        }
    }
    void SpawnMyPlayer(int teamID)
    {
        Debug.Log("SpawnMyPlayer void has been called");
        hasPickedTeam = true;
        standbyCamera.SetActive(false);
        SpawnPoint mySpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        while(mySpawnPoint.teamID != teamID)
        {
            mySpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        }

        // This line here references the resource we are going to load for our character:
        GameObject myPlayerGO = PhotonNetwork.Instantiate(strPlayerGameObject, mySpawnPoint.transform.position, mySpawnPoint.transform.rotation, 0);
        // After we have instantiated our character on the network, we should enable all the scripts that we will use to control it:
        GameObject myPlayerGOcam = myPlayerGO.transform.Find("FirstPersonCharacter").gameObject;
        GameObject myPlayerGOviewmodelCam = myPlayerGO.transform.Find("FirstPersonCharacter/ViewmodelCamera").gameObject;
        myPlayerGOcam.GetComponent<Camera>().enabled = true;
        myPlayerGOviewmodelCam.SetActive(true);
        myPlayerGOviewmodelCam.GetComponent<Camera>().enabled = true;
        myPlayerGOcam.GetComponent<AudioListener>().enabled = true;
        myPlayerGOcam.GetComponent<FlareLayer>().enabled = true;
        myPlayerGO.GetComponent<PlayerControls>().enabled = true;
        myPlayerGO.GetComponent<PlayerShootingRaycast>().enabled = true;

        myPlayerGO.GetComponent<PhotonView>().RPC("SetTeamID", PhotonTargets.AllBuffered, teamID);
        // myPlayerGO.GetComponentInChildren<EnhancedMouseLook>().enabled = true;

    //     myPlayerGOcam.GetComponent<AudioListener>().enabled = true;
    //     myPlayerGOcam.GetComponent<FlareLayer>().enabled = true;
     //    myPlayerGO.GetComponent<PlayerMovement>().enabled = true;
     //    myPlayerGO.GetComponent<PlayerShooting>().enabled = true;
      //   Transform viewModelCamera = myPlayerGO.transform.Find("FirstPersonCharacter/ViewModelCamera");
      //   viewModelCamera.gameObject.GetComponent<Camera>().enabled = true;


      //   Transform playerModel = myPlayerGO.transform.Find("PlayerModel");
      //   playerModel.gameObject.layer = 9;
      //   Transform[] playerModelChildren = playerModel.GetComponentsInChildren<Transform>();
       //  foreach (Transform child in playerModelChildren)
       //  {
       //      child.gameObject.layer = 9;
      //   }
      //   Transform viewModels = myPlayerGO.transform.Find("FirstPersonCharacter/ViewModels");
       //  viewModels.gameObject.SetActive(true);


    }


}
