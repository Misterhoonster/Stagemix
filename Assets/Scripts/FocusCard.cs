using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class FocusCard : NetworkBehaviour
{
    public PlayerManager PlayerManager;
    public GameObject DropZone;
    public GameObject PlayerArea;

    public Sprite FocusImage;

    void Start()
    {
        DropZone = GameObject.Find("DropZone");
        PlayerArea = GameObject.Find("PlayerArea");
    }

    public void ChooseCard()
    {
        if (hasAuthority || transform.parent.gameObject == DropZone)
        {
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            PlayerManager = networkIdentity.GetComponent<PlayerManager>();

            if (transform.parent.gameObject == DropZone)
            {
                Debug.Log("Card in DROP ZONE");
                PlayerManager.DiscardCard();
                return;
            }

            foreach (Transform idol in PlayerArea.transform)
            {
                Sprite CardFront = idol.GetComponent<Flip>().CardFront;

                if (idol.GetComponent<Image>().sprite == idol.GetComponent<FocusCard>().FocusImage)
                {
                    idol.GetComponent<Image>().sprite = CardFront;
                }
            }

            Sprite currentSprite = gameObject.GetComponent<Image>().sprite;
            gameObject.GetComponent<Image>().sprite = FocusImage;

            PlayerManager.CmdChooseCard(gameObject);
        }
    }

}