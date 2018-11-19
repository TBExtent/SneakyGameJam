using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerBrowserBack : MonoBehaviour
{

    public void returnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
