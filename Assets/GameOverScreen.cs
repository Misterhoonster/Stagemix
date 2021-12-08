using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class GameOverScreen : NetworkBehaviour
{
    public GameObject playerPointsText;
    public GameObject enemyPointsText;
    public GameObject resultText;

    // public void Start()
    // {
    //     gameObject.SetActive(false);
    // }
    public void Setup(float playerScore, float enemyScore)
    {
        // gameObject.SetActive(true);
        Debug.Log("GAME OVER SHOULD BE SHOWING!");
        Debug.Log("Player score for end screen: " + playerScore);
        Debug.Log("Enemy score for end screen: " + enemyScore);

        Debug.Log("PLAYER SCORE TEXT: " + playerPointsText.GetComponent<TMPro.TextMeshProUGUI>().text);

        if (playerScore > enemyScore)
        {
            resultText.GetComponent<TMPro.TextMeshProUGUI>().text = "You Won!";
        }
        else
        {
            resultText.GetComponent<TMPro.TextMeshProUGUI>().text = "You Lost";
        }
        playerPointsText.GetComponent<TMPro.TextMeshProUGUI>().text = playerScore.ToString();
        enemyPointsText.GetComponent<TMPro.TextMeshProUGUI>().text = enemyScore.ToString();
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
