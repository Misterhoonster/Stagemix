using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DrawCards : NetworkBehaviour
{
    public PlayerManager PlayerManager;
    public GameObject PlayerArea;

    public void OnClick()
    {
        PlayerArea = GameObject.Find("PlayerArea");

        if (PlayerArea.transform.childCount >= 5) return;

        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();
        PlayerManager.CmdDealCards();
    }

}
