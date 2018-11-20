using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOORScript : MonoBehaviour {
	GameObject[] players;
	float activateDist = 10;
	Vector3 closedPos;
	Vector3 openPos;

	// Use this for initialization
	void Start () {

		closedPos = transform.position;
		openPos = closedPos + transform.up * 10;
	}

	// Update is called once per frame
	void Update () {
		players = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < players.Length; i++) {
			if ((players[i].transform.position - closedPos).sqrMagnitude <= activateDist) {
				transform.position = Vector3.Lerp(transform.position, openPos, 0.2f);
			}
			else {
				transform.position = Vector3.Lerp(transform.position, closedPos, 0.2f);
			}
		}
	}
}
