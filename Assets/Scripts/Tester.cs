using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    [SerializeField] private Transform scorePopupTransform;
    // Start is called before the first frame update
    void Start()
    {
        GameObject enemyArea = GameObject.Find("EnemyArea");

        Transform popup = Instantiate(scorePopupTransform, new Vector2(0, 0), Quaternion.identity);
        popup.SetParent(enemyArea.transform, false);
        
        Popup scorePopup = popup.GetComponent<Popup>();
        scorePopup.Setup(55);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
