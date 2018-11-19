using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MasterMenu : MonoBehaviour {


    public string CreateRoomName= "Create Room";
    public string RoomBrowserName = "Room Browser";

    public void joinServer()
    {
        SceneManager.LoadScene(RoomBrowserName);
    }
    public void createGame()
    {
        SceneManager.LoadScene(CreateRoomName);
    }
    public void quitGame()
    {
        Application.Quit();
    }


}
