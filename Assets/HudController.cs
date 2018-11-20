using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour {
	public Text HealthText;
	public Text AmmoText;
	// Use this for initialization
	void Start () {
		HealthText = GameObject.FindWithTag("HUD").transform.Find("HealthText").GetComponent<Text>();
		AmmoText = GameObject.FindWithTag("HUD").transform.Find("AmmoText").GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {

	}
}
