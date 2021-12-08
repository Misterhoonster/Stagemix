using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Flip : NetworkBehaviour
{
    public Sprite CardFront, CardBack;
    public PlayerManager PlayerManager;
    public GameObject Card;
    //public bool isFlipping = false;
    public void FlipCard()
    {
        Sprite currentSprite = gameObject.GetComponent<Image>().sprite;
        if (currentSprite == CardBack)
        {
            gameObject.GetComponent<Image>().sprite = CardFront;
        }

        //isFlipping = true;
    }
    public void SignalFlip()
    {
        if(hasAuthority)
        {
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            PlayerManager = networkIdentity.GetComponent<PlayerManager>();

            if (gameObject.GetComponent<Image>().sprite == CardBack) {
                PlayerManager.Flip(gameObject);
                float score = gameObject.GetComponent<CardStats>().Score;
                Debug.Log(score);
                string ability = gameObject.GetComponent<CardStats>().Ability;
                Debug.Log(ability);
            }
        }

    }

    /*public void Update() {

        if(isFlipping)
        {
            float roty = Time.deltaTime * 90 * Mathf.Deg2Rad;
            Debug.Log(roty);
            Debug.Log(Time.deltaTime);
            transform.RotateAround(Vector3.up, -roty);
        }
        if (transform.localEulerAngles.y > 180 && transform.localEulerAngles.y < 270)
        {
            Sprite currentSprite = gameObject.GetComponent<Image>().sprite;
            if (currentSprite == CardBack)
            {
                gameObject.GetComponent<Image>().sprite = CardFront;
                transform.RotateAround(Vector3.up, 180 * Mathf.Deg2Rad);
            }
            else{
                isFlipping = false;
            }
        }
        if (transform.localEulerAngles.y > 180 && gameObject.GetComponent<Image>().sprite != CardBack)
        {
            isFlipping = false;
        }
    } */
}
