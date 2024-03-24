using UnityEngine;

public class Jumping_prefab_Moving : MonoBehaviour
{
    public float speed;
    GameObject gameManager;
    public GameObject coin;
    public GameObject boob;
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        speed = gameManager.GetComponent<Jumping_GameManager>().speed;
        for (int i = 0; i < 10; i++)
        {
            MakeObjects(i);
        }
    }

    void FixedUpdate()
    {
            transform.position += new Vector3(0, 0, -1 * speed);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Erase"))
        {
            Destroy(gameObject);
        }
    }

    void MakeObjects(int pos)
    {
        if (Time.timeScale == 1)
        {
            if (Random.Range(0, 11) < gameManager.GetComponent<Jumping_GameManager>().makeProbability)
            {
                if (Random.Range(0, 11) > gameManager.GetComponent<Jumping_GameManager>().probability)
                {
                    Instantiate(coin, transform.position + transform.right * pos * 3 + transform.up * Random.Range(0, 3) * 2, transform.rotation);
                }
                else
                {
                    Instantiate(boob, transform.position + transform.right * pos * 3 + transform.up * Random.Range(0, 3) * 2, transform.rotation);
                }
            }
        }
    }
}
