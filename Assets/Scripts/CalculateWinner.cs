using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CalculateWinner : NetworkBehaviour
{
    float enemyScore = 0;
    float playerScore = 0;
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public GameObject[] playerDeck;
    public GameObject[] enemyDeck;
    public PlayerManager PlayerManager;
    public void Winner()
    {
        PlayerArea = GameObject.Find("PlayerArea");
        EnemyArea = GameObject.Find("EnemyArea");
        Debug.Log("winner function called");

        GameObject[] allStageCards = Resources.LoadAll<GameObject>("Prefabs/Stages");

        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        GameObject currentStage = allStageCards[gm.stage];

        Dictionary<string, int> stage_stats =  new Dictionary<string, int>(){
                                  {"dance", currentStage.GetComponent<StageCard>().dance},
                                  {"vocal", currentStage.GetComponent<StageCard>().vocal},
                                  {"visual", currentStage.GetComponent<StageCard>().visual},
                                  {"personality", currentStage.GetComponent<StageCard>().personality}};

        foreach (Transform idol in PlayerArea.transform)
        {
            float score = idol.GetComponent<CardStats>().Score;

            string ability = idol.GetComponent<CardStats>().Ability;

            if (ability == "all")
            {
                Debug.Log("Player score BOOSTED by all-rounder");
                score += 1;
            }
            else
            {
                if (stage_stats.ContainsKey(ability))
                {
                    Debug.Log("Player score BOOSTED by " + stage_stats[ability] + " points");
                    score += stage_stats[ability];
                }
            }
            playerScore += score;
        }

        foreach (Transform idol in EnemyArea.transform)
        {
            float score = idol.GetComponent<CardStats>().Score;
            
            string ability = idol.GetComponent<CardStats>().Ability;

            if (ability == "all")
            {
                Debug.Log("Enemy score BOOSTED by all-rounder");
                score += 1;
            }
            else
            {
                if (stage_stats.ContainsKey(ability))
                {
                    Debug.Log("Enemy score BOOSTED by " + stage_stats[ability] + " points");
                    score += stage_stats[ability];
                }
            }

            enemyScore += score;
        }


        Debug.Log("player score: " + playerScore);
        Debug.Log("enemy score: " + enemyScore);

        if (playerScore > enemyScore)
        {
            Debug.Log("player wins");
        }
        else if (enemyScore > playerScore)
        {
            Debug.Log("enemy wins");
        }
        else
        {
            Debug.Log("tie");
        }

        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();

        PlayerManager.EndGame(playerScore, enemyScore);
    }
}