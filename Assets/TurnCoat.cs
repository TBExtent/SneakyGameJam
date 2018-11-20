using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCoat : MonoBehaviour {


	Renderer rend;
	public bool isPlayer = true;
	public int teamID;
	public int otherTeamID;
	public float cooldown = 60f;
	public float duration = 15f;
	public float cooldownRemaining = 0f;
	public AudioSource AuxAudio;
	public AudioClip switchSound;
	Text cooldownText;
	[Header("Element 1 corresponds to RED, 2 to Blue")]
	public Texture[] CharacterTextures;
	public Transform[] TeamColourTransforms;
	void Start(){
		teamID = GetComponent<TeamMember>().teamID;
		if(teamID == 2){
			otherTeamID = 1;
		}
		else{
			otherTeamID = 2;
		}
		 cooldownText = GameObject.FindWithTag("HUD").transform.Find("DisguiseCooldown").GetComponent<Text>();
	}
	void Update (){
		if(cooldownRemaining >= 0){
			cooldownRemaining -= Time.deltaTime;
			if(Mathf.Round(cooldownRemaining) == 0){
				cooldownText.text = "Disguise Ready";
			}
			else{
				cooldownText.text = "Disguise: " + (Mathf.Round(cooldownRemaining)).ToString();
			}

		}
		if(Input.GetMouseButtonDown(1) && cooldownRemaining <= 0){

			Debug.Log("Turning Coat");
			GetComponent<PhotonView>().RPC("SetTeamColourTurncoat", PhotonTargets.AllBuffered, otherTeamID);
			Invoke("RevertTeamColour", duration);
			// Maybe play some smokey effects
			cooldownRemaining = cooldown;
		}
	}

	void RevertTeamColour(){
			Debug.Log("Reverting Coat");
			GetComponent<PhotonView>().RPC("SetTeamColourTurncoat", PhotonTargets.AllBuffered, teamID);
	}

	[PunRPC]
	void SetTeamColourTurncoat(int ID)
	{
		AuxAudio.clip = switchSound;
		AuxAudio.Play();
			foreach(Transform t in TeamColourTransforms)
			{
					rend = t.GetComponent<Renderer>();
					rend.material.mainTexture = CharacterTextures[ID - 1];
			}

	}
}
