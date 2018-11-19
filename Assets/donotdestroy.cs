using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class donotdestroy : MonoBehaviour

{
        
   
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");

        if (objs.Length > 1)
        {

            Destroy(this.gameObject);
        }

    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if ((SceneManager.GetActiveScene().name == "Main Menu")
            ||(SceneManager.GetActiveScene().name == "Room Browser")
            || (SceneManager.GetActiveScene().name == "Create Room"))
        {
        
         

            DontDestroyOnLoad(this.gameObject);
        }

        else Destroy(this.gameObject);
    }
}
    




