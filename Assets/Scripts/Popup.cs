using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class Popup : NetworkBehaviour
{
    public TMP_Text textMesh;
    void Awake()
    {
        textMesh = gameObject.GetComponent<TMP_Text>();
        textMesh.text = "placeholder";
        Debug.Log(textMesh.text);
    }
    public void Setup(int score)
    {
        textMesh.text = score.ToString();
    }
}
