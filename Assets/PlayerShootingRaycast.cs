using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingRaycast : MonoBehaviour {
    public Transform localBarrel;
    public Transform Barrel;
    public float fireRate = 0.5f;
    float cooldown;
    float damage = 10f;
    public AnimationClip shootClip;
    public GameObject viewModel;
    public GameObject muzzleFlash;
    public GameObject ricochet;
    // Use this for initialization
    void Start () {
        cooldown = fireRate;
	}
	
	// Update is called once per frame
	void Update () {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        if(Input.GetMouseButtonDown(0) && cooldown <= 0)
        {
            Fire();
        }
    }

    void Fire()
    {
        //Firstly, we do our raycast to determine the hitPoint of our weapon
        //Now we do our effects:
        viewModel.GetComponent<Animation>().clip = shootClip;
        viewModel.GetComponent<Animation>().Play();

    }
}
