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

    public Text HealthText;
  //  public GameObject ragdollVaporise;
    // Use this for initialization
    void Start()
    {
        if (GetComponent<PhotonView>().isMine)
        {
            HealthText = GameObject.FindWithTag("HUD").transform.Find("HealthText").GetComponent<Text>();
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
    public void TakeDamage(float amt, string LHP, int killerTeamID)
    {


        lastHitPlayer = LHP;
        amt = Mathf.Round(amt);
        currentHitPoints -= amt;
        HealthText.text = "Health: " + currentHitPoints.ToString();
        if (GetComponent<PhotonView>().isMine)
        {
          //  snm.UIHealth.text = currentHitPoints.ToString();
        }
        if (currentHitPoints <= 0)
        {


            Die(LHP, killerTeamID);
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
    void instantiateRagdoll()
    {
            Instantiate(ragdollNormal, transform.position, transform.rotation);
    }

    public void AddHealth(float amount)
    {
              GameObject.FindWithTag("HUD").transform.Find("HealthText").GetComponent<Text>();
        currentHitPoints += amount;
        if (currentHitPoints > hitPoints)
        {
            currentHitPoints = hitPoints;
        }
    }


    void Die(string LHP, int killerTeamID)
    {

        if (snm.offlineMode == false)
        {
            if (IsDying == false)
            {
                IsDying = true;
                GetComponent<PhotonView>().RPC("instantiateRagdoll", PhotonTargets.All);
                if (GetComponent<PhotonView>().instantiationId == 0)
                {
                    Destroy(gameObject);
                }
                else
                {
                    if (GetComponent<PhotonView>().isMine)
                    {
                        GetComponent<PhotonView>().RPC("DisplayFragText", PhotonTargets.All, LHP, PhotonNetwork.player.NickName, killerTeamID, GetComponent<TeamMember>().teamID);
                        GetComponent<PhotonView>().RPC("UpdateScores", PhotonTargets.AllBuffered, killerTeamID, GetComponent<TeamMember>().teamID);
                        snm.playerRespawnTimer = 5f;
                        snm.standbyCamera.SetActive(true);
                        PhotonNetwork.Destroy(gameObject);
                        //  Invoke("AddDeath", 0.05f);
                        // Invoke("Dest", 0.1f);

                    }
                }
            }
        }
    }
    [PunRPC]
    void DisplayFragText(string fraggerName, string fraggedName, int killerTeamID, int myTeamID)
    {
            string correctly = "";
            if(killerTeamID == myTeamID){
              correctly = " incorrectly!";
            }
            else{
              correctly = " correctly!";
            }
            GameObject killTextInstance = Instantiate(fragText);
            killTextInstance.GetComponentInChildren<Text>().text = (fraggerName + " just fragged " + fraggedName + correctly);
    }

    [PunRPC]
    void UpdateScores(int killerTeamID, int myTeamID){
      GameObject scoreboard = GameObject.FindWithTag("Scoreboard");
      if(killerTeamID == 2 && myTeamID == 2){
        //Take points from blue
        scoreboard.GetComponent<Scoreboard>().addIncorrectKill(2);
      }
      if(killerTeamID == 1 && myTeamID == 1){
        //Take points from red
        scoreboard.GetComponent<Scoreboard>().addIncorrectKill(1);
      }
      if(killerTeamID == 1 && myTeamID == 2){
        //Give to red
        scoreboard.GetComponent<Scoreboard>().addCorrectKill(1);
      }
      if(killerTeamID == 2 && myTeamID == 1){
        //Give to blue
        scoreboard.GetComponent<Scoreboard>().addCorrectKill(2);
      }
    }

    public void updateHUD()
    {
      //  snm.UIHealth.text = currentHitPoints.ToString();
    }
}
