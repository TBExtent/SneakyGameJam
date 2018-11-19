using UnityEngine;
using System.Collections;

public class TeamMember : MonoBehaviour {
    Renderer rend;
    public bool isPlayer = true;
    public int _teamID = 0;
    public int teamID
    {
        get { return _teamID; }
    }
    [Header("Element 1 corresponds to RED, 2 to Blue")]
    public Texture[] CharacterTextures;
    public Transform[] TeamColourTransforms;
    [PunRPC]
    void SetTeamID(int id)
    {
        _teamID = id;
        foreach(Transform t in TeamColourTransforms)
        {
            rend = t.GetComponent<Renderer>();
            rend.material.mainTexture = CharacterTextures[id];
        }

    }

    public void Start()
    {
        // Find termPersist
     //   GameObject.FindWithTag("TeamPersist").GetComponent<TeamPersistClient>().GetTeam();
    }
}
