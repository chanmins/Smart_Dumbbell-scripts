using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hooray_playerStatue : MonoBehaviour
{
    public Hooray_Gamemanager gameManager;


    void OnTriggerEnter(Collider other)
    {
        //사람에 부딧힐 경우에만
        if (!gameManager.isEnded)
        {
            if (other.gameObject.CompareTag("Coin"))
            {
                gameManager.score.text = (int.Parse(gameManager.score.text) + other.gameObject.GetComponent<Hooray_ObjectMove>().score).ToString("D4");
                Destroy(other.gameObject);
            }
            else
            {
                gameManager.score.text = (int.Parse(gameManager.score.text) / 2).ToString();
                Destroy(other.gameObject);
            }
        }
    }
}