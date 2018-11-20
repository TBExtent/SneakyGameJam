using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingRaycast : MonoBehaviour {
    public Transform localBarrel;
    public Transform Barrel;
    public float fireRate = 0.5f;
    float cooldown;
    public float damage = 10f;
    public AnimationClip shootClip;
    public GameObject viewModel;
    public GameObject beamPrefab;
    public GameObject muzzleFlash;
    public GameObject ricochet;
    public Transform playerView;
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

    void drawBeam(Vector3 start, Vector3 end)
    {

        GameObject beamPrefabInstance = Instantiate(beamPrefab, Vector3.zero, Quaternion.identity);
        beamPrefabInstance.GetComponent<LineRenderer>().SetPosition(0, start);
        beamPrefabInstance.GetComponent<LineRenderer>().SetPosition(1, end);

    }
    [PunRPC]
    void drawBeamServer(Vector3 start, Vector3 end)
    {

        GameObject beamPrefabInstance = Instantiate(beamPrefab, Vector3.zero, Quaternion.identity);
        beamPrefabInstance.GetComponent<LineRenderer>().SetPosition(0, start);
        beamPrefabInstance.GetComponent<LineRenderer>().SetPosition(1, end);

    }
    void Fire()
    {
        cooldown = fireRate;
        //Firstly, we do our raycast to determine the hitPoint of our weapon
        Ray ray = new Ray(playerView.transform.position, playerView.forward);
        RaycastHit hit;
        Transform hitTransform;
        Vector3 hitVector;
        hitTransform = FindClosestHitObject(ray, out hitVector);

        Physics.Raycast(ray.origin, ray.direction, out hit);
        if (hitTransform == null)
        {
            hit.point = playerView.transform.position + (playerView.forward * 150f);
        }

        Debug.Log("We have just shot the position" + hit.point);



        // tempLocal is used to get the position vector of our barrel in the game world to send to other players.

        //Vector3 tempLocal = Barrel.localPosition;
        //Now we do our effects:
         viewModel.GetComponent<Animation>().clip = shootClip;
         viewModel.GetComponent<Animation>().Play();
         MuzzleFlashEffect(localBarrel.transform.position, hit.point);
        drawBeam(localBarrel.position, hit.point);
        GetComponent<PhotonView>().RPC("drawBeamServer", PhotonTargets.Others, localBarrel.position, hit.point);
        GetComponent<PhotonView>().RPC("RicochetEffect", PhotonTargets.All, hit.point);
        //Now we deal damage to the enemy player
        if (hitTransform != null)
        {

            Health h = hitTransform.GetComponent<Health>();

            while (h == null && hitTransform.parent)
            {
                hitTransform = hitTransform.parent;
                h = hitTransform.GetComponent<Health>();
            }

            // Once we reach here, hitTransform may not be the hitTransform we started with!

            if (h != null)
            {
                // This next line is the equivalent of calling:
                //    				h.TakeDamage( damage );
                // Except more "networky"
                PhotonView pv = h.GetComponent<PhotonView>();
                if (pv == null)
                {
                    Debug.LogError("Freak out! There's no health!");
                }
                else
                {
                    TeamMember tm = hitTransform.GetComponent<TeamMember>();
                    TeamMember myTm = this.GetComponent<TeamMember>();
                        //GetComponent<HitSound>().PlaySound(damage);
                     h.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, damage, PhotonNetwork.player.name, myTm.teamID);         //RPC

                }
            }
            else
            {
                Debug.Log("The hit object has got no health");
            }
        }



    }
    void MuzzleFlashEffect(Vector3 startPosition, Vector3 endPosition)
    {
        GameObject muzzleFlashInstance = Instantiate(muzzleFlash, startPosition, Quaternion.identity);
        muzzleFlashInstance.transform.LookAt(endPosition);
    }
    [PunRPC]
    void RicochetEffect(Vector3 hitPosition)
    {
        Instantiate(ricochet, hitPosition, Quaternion.identity);
    }



    [PunRPC]
    void fireTracer(Vector3 startPos, Vector3 endPos)
    {

    }

    Transform FindClosestHitObject(Ray ray, out Vector3 hitPoint)
    {

        RaycastHit[] hits = Physics.RaycastAll(ray);

        Transform closestHit = null;
        float distance = 0;
        hitPoint = Vector3.zero;

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform != this.transform && (closestHit == null || hit.distance < distance))
            {
                // We have hit something that is:
                // a) not us
                // b) the first thing we hit (that is not us)
                // c) or, if not b, is at least closer than the previous closest thing

                closestHit = hit.transform;
                distance = hit.distance;
                hitPoint = hit.point;
            }
        }

        // closestHit is now either still null (i.e. we hit nothing) OR it contains the closest thing that is a valid thing to hit

        return closestHit;

    }
}
