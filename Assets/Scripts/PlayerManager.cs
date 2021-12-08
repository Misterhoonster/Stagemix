using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public GameObject DropZone;
    public GameObject StageCardZone;
    List<GameObject> stageCards;
    [SyncVar]
    List<GameObject> cards;
    public GameObject FocusedCard;
    public GameObject stage;
    public GameObject GameOver;
    

    public override void OnStartClient()
    {
        base.OnStartClient();
        PlayerArea = GameObject.Find("PlayerArea");
        EnemyArea = GameObject.Find("EnemyArea");
        DropZone = GameObject.Find("DropZone");
        StageCardZone = GameObject.Find("StageCardZone");

        // RpcSetStage();
        // GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        // GameObject currStage = Instantiate(stage, new Vector2(0,0), Quaternion.identity);
        // currStage.transform.SetParent(StageCardZone.transform, false);

        // GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();

        GameObject[] allCards = Resources.LoadAll<GameObject>("Prefabs/Idols");
        cards = new List<GameObject>(allCards);

        GameObject[] allStageCards = Resources.LoadAll<GameObject>("Prefabs/Stages");
        stageCards = new List<GameObject>(allStageCards);

        // stage = stageCards[Random.Range(0, stageCards.Count)];
        // SetStage(stageCards[Random.Range(0, stageCards.Count)]);
        
    }

    public void InitialDeal()
    {
        for (int i = 0; i < 5; i++)
        {
            int idx = Random.Range(0, cards.Count);
            GameObject card = Instantiate(cards[idx], new Vector2(0,0), Quaternion.identity);
            cards.RemoveAt(idx);
            NetworkServer.Spawn(card, connectionToClient);
            RpcShowCard(card, "Dealt");
        }
    }

    [Command]
    public void CmdDrawStageCard()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        GameObject stage = Instantiate(stageCards[gm.stage], new Vector2(0,0), Quaternion.identity);
        NetworkServer.Spawn(stage, connectionToClient);

        RpcShowStageCard(stage);
    }

    [ClientRpc]
    public void RpcShowStageCard(GameObject stage)
    {
        stage.transform.SetParent(StageCardZone.transform, false);
    }

    [Command]
    public void CmdDealCards()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (gm.TurnsPlayed < 2)
        {
            InitialDeal();
            return;
        }

        int idx = Random.Range(0, cards.Count);
        GameObject card = Instantiate(cards[idx], new Vector2(0,0), Quaternion.identity);
        cards.RemoveAt(idx);

        NetworkServer.Spawn(card, connectionToClient);
        RpcShowCard(card, "Dealt");

        UpdateTurnsPlayed();
    }

    [Server]
    void UpdateTurnsPlayed()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.UpdateTurnsPlayed();
        RpcLogToClients("Turns Played: " + gm.TurnsPlayed);
    }

    [Server]
    void IncrementEnemyCardsFlipped()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.IncrementEnemyCardsFlipped();
    }

    [Server]
    void IncrementPlayerCardsFlipped()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.IncrementPlayerCardsFlipped();
    }

    [Server]
    void DecreaseEnemyCardsFlipped()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.DecreaseEnemyCardsFlipped();
    }

    [Server]
    void DecreasePlayerCardsFlipped()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.DecreasePlayerCardsFlipped();
    }

    [Server]
    int GetEnemyCardsFlipped()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        return gm.GetEnemyCardsFlipped();
    }

    [Server]
    int GetPlayerCardsFlipped()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        return gm.GetPlayerCardsFlipped();
    }


    [ClientRpc]
    void RpcLogToClients(string message)
    {
        Debug.Log(message);
    }

    
    public void Flip(GameObject card)
    {
        CmdFlip(card);
    }

    [Command]
    void CmdFlip(GameObject card)
    {
        RpcShowCard(card, "Flipped");
;
    }

    [ClientRpc]
    void RpcShowCard(GameObject card, string type)
    {
        if (type == "Dealt")
        {
            if (hasAuthority)
            {
                card.transform.SetParent(PlayerArea.transform, false);
            }
            else
            {
                card.transform.SetParent(EnemyArea.transform, false);
            }
        }
        else if (type == "Discarded")
        {
            UpdateTurnsPlayed();
            if(hasAuthority)
            {
                DecreasePlayerCardsFlipped();
            }
            else
            {
                DecreaseEnemyCardsFlipped();
            }
            Debug.Log("player cards flipped: " + GetPlayerCardsFlipped());
            Debug.Log("enemy cards flipped: " + GetEnemyCardsFlipped());
            
            card.transform.SetParent(DropZone.transform, false);
            card.GetComponent<Flip>().FlipCard();
        }
        else if (type == "Flipped")
        {
            UpdateTurnsPlayed();
            //card.transform.SetParent(PlayerArea.transform, false);
            if (hasAuthority)
            {
                IncrementPlayerCardsFlipped();
            }
            else
            {
                IncrementEnemyCardsFlipped();
            }
            card.GetComponent<Flip>().FlipCard();
            Debug.Log("player cards flipped: " + GetPlayerCardsFlipped());
            Debug.Log("enemy cards flipped: " + GetEnemyCardsFlipped());

            if (GetPlayerCardsFlipped() == 5 && GetEnemyCardsFlipped() == 5)
            {
                Debug.Log("finished round");
                Debug.Log("Final flipped card: " + card.name);
                card.GetComponent<CalculateWinner>().Winner();
            }

            if (GetPlayerCardsFlipped() == 5 && GetEnemyCardsFlipped() == 5)
            {
                Debug.Log("finished round");
                Debug.Log("Final flipped card: " + card.name);
                card.GetComponent<CalculateWinner>().Winner();
            }
        }
    }

    [Command]
    public void CmdChooseCard(GameObject card)
    {
        RpcChooseCard(card);
    }

    [ClientRpc]
    void RpcChooseCard(GameObject card)
    {
        // card.GetComponent<FocusCard>().FocusedCard = card;
        FocusedCard = card;
        Debug.Log(card.name + " card has been chosen");
    }

    public void DiscardCard()
    {
        CmdDiscardCard();
    }

    [Command]
    void CmdDiscardCard()
    {
        Debug.Log(FocusedCard.name);
        RpcShowCard(FocusedCard, "Discarded");
    }

    public void EndGame(float playerScore, float enemyScore)
    {
        CmdEndGame(playerScore, enemyScore);
    }

    [Command]
    void CmdEndGame(float playerScore, float enemyScore)
    {
        Debug.Log("Ending game!");
        RpcEndGame(playerScore, enemyScore);
    }

    [ClientRpc]
    void RpcEndGame(float playerScore, float enemyScore)
    {
        GameObject GameOverScreen = Instantiate(GameOver, new Vector2(0,0), Quaternion.identity);
        NetworkServer.Spawn(GameOverScreen, connectionToClient);

        GameOverScreen.GetComponent<GameOverScreen>().Setup(playerScore, enemyScore);
    }
}
