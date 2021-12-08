using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DrawStageCard : NetworkBehaviour
{
    public PlayerManager PlayerManager;
    // Start is called before the first frame update
    public void OnClick()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();
        PlayerManager.CmdDrawStageCard();
    }
}
