using UnityEngine;
using System.Collections;

public class NetworkCharacter : Photon.MonoBehaviour {


    Vector3 realPosition = Vector3.zero;
   public  Quaternion realRotation = Quaternion.identity;

    bool gotFirstUpdate = false;

    public float RealAimAngle = 0f;
    // Use this for initialization

    void Start() {

        if (GetComponent<PhotonView>().isMine == true)
        {
            gameObject.layer = 2; // makes it so we can't shoot ourself
            foreach(Transform child in transform)
            {
                child.gameObject.layer = 2;
            }

        }
    }

    void CacheComponents()
    {

    }
    // Update is called once per frame
    void Update() {
        if (photonView.isMine)
        {
            // DO NUFFINK
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, realPosition, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.1f);

        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        CacheComponents();
        if (stream.isWriting)
        {
            //Our player, our position must be sent to the network.

            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);


        }
        else
        {
            //Not our player, position must be received (as of some ms ago, then update position)

            realPosition = (Vector3) stream.ReceiveNext();
            realRotation = (Quaternion)stream.ReceiveNext();

          //  anim.SetBool("Fire", (bool)stream.ReceiveNext());
            //anim.SetBool("Firing", (bool)stream.ReceiveNext());
            RealAimAngle = (float)stream.ReceiveNext();


            if (gotFirstUpdate == false)
            {
                transform.position = realPosition;
                transform.rotation = realRotation;

                gotFirstUpdate = true;
            }
        }
    }
}