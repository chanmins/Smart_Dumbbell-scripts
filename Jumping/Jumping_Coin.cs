using System;
using UnityEngine;

public class Jumping_Coin : MonoBehaviour
{
    public Boolean isCoin;
    public GameObject gameManager;
    public float speed;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        speed = gameManager.GetComponent<Jumping_GameManager>().speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += new Vector3(0, 0, -1 * speed);
        if (isCoin)
        {
            transform.Rotate(Vector3.up);
            }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isCoin)
            {
                gameManager.GetComponent<Jumping_GameManager>().score += 100;
            }
            else
            {
                gameManager.GetComponent<Jumping_GameManager>().score -= 50;
            }
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Erase"))
        {
            Destroy(gameObject);
        }
    }
}
