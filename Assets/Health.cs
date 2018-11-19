using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour
{

    public float hitPoints = 100f;
    public float currentHitPoints;
    public float respawnTime = 1f;
    selfNetworkManager snm;
    public string lastHitPlayer;
    public string lastHitType;
    bool IsDying = false;
    public GameObject killedText;
    public GameObject fragText;
    public GameObject ragdollNormal;
  //  public GameObject ragdollVaporise;
    // Use this for initialization
    void Start()
    {
        if (GetComponent<PhotonView>().isMine)
        {
            currentHitPoints = hitPoints;
            snm = GameObject.FindObjectOfType<selfNetworkManager>();
           // snm.UIHealth.text = currentHitPoints.ToString();
        }

    }
    public void setStuff()
    {
        currentHitPoints = hitPoints;
        snm = GameObject.FindObjectOfType<selfNetworkManager>();
      //  snm.UIHealth.text = currentHitPoints.ToString();
    }

    [PunRPC]
    public void TakeDamage(float amt, string LHP, string LHT)
    {
        lastHitPlayer = LHP;
        lastHitType = LHT;
        amt = Mathf.Round(amt);
        currentHitPoints -= amt;

        if (GetComponent<PhotonView>().isMine)
        {
          //  snm.UIHealth.text = currentHitPoints.ToString();
        }
        if (currentHitPoints <= 0)
        {


            Die(LHT, LHP);
        }
    }
    //
   /* [PunRPC]
    void findKillerPlayer(string playerNickname, string LHT) //Finds the appropriate quote to shout
    {
        if (PhotonNetwork.player.NickName == playerNickname)
        {
            selfNetworkManager snm2 = (SelfNetworkManagerII)FindObjectOfType(typeof(SelfNetworkManagerII));
            snm2.myPlayer.GetComponent<DynamicShouting>().CharacterShoutKill(LHT);
        }
    } */
    [PunRPC]
    void instantiateRagdoll(string lastHitType)
    {
            Instantiate(ragdollNormal, transform.position, transform.rotation);
    }

    public void AddHealth(float amount)
    {
        currentHitPoints += amount;
        if (currentHitPoints > hitPoints)
        {
            currentHitPoints = hitPoints;
        }
    }


    void Die(string LHT, string LHP)
    {

        if (snm.offlineMode == false)
        {
            if (IsDying == false)
            {
                IsDying = true;
                GetComponent<PhotonView>().RPC("instantiateRagdoll", PhotonTargets.All, LHT);
                if (GetComponent<PhotonView>().instantiationId == 0)
                {
                    Destroy(gameObject);
                }
                else
                {
                    if (GetComponent<PhotonView>().isMine)
                    {

                        PhotonNetwork.Destroy(gameObject);
                        //  Invoke("AddDeath", 0.05f);
                        // Invoke("Dest", 0.1f);

                    }
                }
            }
        }
    }
    [PunRPC]
    void DisplayFragText(string fraggerName, string fraggedName)
    {
        if (fraggerName == PhotonNetwork.playerName.ToString())
        {
            GameObject killTextInstance = Instantiate(fragText);
            killTextInstance.GetComponentInChildren<Text>().text = ("You just fragged " + fraggedName);
        }
    }

    public void updateHUD()
    {
      //  snm.UIHealth.text = currentHitPoints.ToString();
    }
}
