﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Scoreboard : MonoBehaviour {

	public int RedScore = 0;
	public int BlueScore = 0;
	public int pointsToWin = 20;
	public int pointsPerCorrectKill = 1;
	public int pointsPerIncorrectKill = -2;
	public GameObject VictoryCanvasRed;
	public GameObject VictoryCanvasBlue;
	public Text RedScoreText;
	public Text BlueScoreText;
	void Start () {

	}

	public void addCorrectKill(int teamID){
		if(teamID == 1){
			RedScore += pointsPerCorrectKill;
		}
		else if(teamID == 2){
			BlueScore += pointsPerCorrectKill;
		}
		UpdateScoreboard();
		if(PhotonNetwork.isMasterClient == true){
			checkVictory();
		}
	}


	public void addIncorrectKill(int teamID){
		if(teamID == 1){
			RedScore += pointsPerIncorrectKill;
		}
		else if(teamID == 2){
			BlueScore += pointsPerIncorrectKill;
		}
				UpdateScoreboard();
	}
void UpdateScoreboard(){
	RedScoreText.text = RedScore.ToString();
	BlueScoreText.text = BlueScore.ToString();
}
void checkVictory(){
	if(RedScore >= pointsToWin){
		Debug.Log("Red Won");
	  GetComponent<PhotonView>().RPC("RedVictory", PhotonTargets.All);
		Invoke("engageReload", 5f);
	}
	else if(BlueScore >= pointsToWin){
		Debug.Log("Blue Won");
	  GetComponent<PhotonView>().RPC("BlueVictory", PhotonTargets.All);
		Invoke("engageReload", 5f);
	}
}

[PunRPC]
void BlueVictory(){
		Instantiate(VictoryCanvasBlue, transform.position, transform.rotation);
}

[PunRPC]
void RedVictory(){
			Instantiate(VictoryCanvasRed, transform.position, transform.rotation);
}
void engageReload(){
     GetComponent<PhotonView>().RPC("reload", PhotonTargets.AllBuffered);
}
[PunRPC]
void reload(){
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
 	SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
	// Update is called once per frame
	void Update () {

	}
}
