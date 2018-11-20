using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour {

	public int RedScore = 0;
	public int BlueScore = 0;
	public int pointsToWin = 20;
	public int pointsPerCorrectKill = 1;
	public int pointsPerIncorrectKill = -2;

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
	}
	else if(BlueScore >= pointsToWin){
		Debug.Log("Blue Won");
	}
}
	// Update is called once per frame
	void Update () {

	}
}
