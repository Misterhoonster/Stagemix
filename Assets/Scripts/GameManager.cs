using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public int stage;
    public int TurnsPlayed = 0;

    public int enemyCardsFlipped = 0;

    public int playerCardsFlipped = 0;
    
    // public void SetStage(GameObject currentStage)
    // {
    //     stage = currentStage;
    // }

    void Awake()
    {
        stage = Random.Range(0,9);
    }
    public void UpdateTurnsPlayed() 
    { 
        TurnsPlayed++;
    }

    public void IncrementEnemyCardsFlipped()
    {
        enemyCardsFlipped++;
    }

    public void IncrementPlayerCardsFlipped()
    {
        playerCardsFlipped++;
    }

    public void DecreaseEnemyCardsFlipped()
    {
        enemyCardsFlipped--;
    }

    public void DecreasePlayerCardsFlipped()
    {
        playerCardsFlipped--;
    }

    public int GetEnemyCardsFlipped()
    {
        return enemyCardsFlipped;
    }

    public int GetPlayerCardsFlipped()
    {
        return playerCardsFlipped;
    }
}
